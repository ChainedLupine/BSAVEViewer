using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace EgaViewer_v2
{
    struct BLOADHeader
    {
        public Byte magic;
        public UInt16 baseAddr;
        public UInt16 offsetAddr;
        public UInt16 len;
    }

    struct PixelFragment
    {
        public UInt16 width;
        public UInt16 height;
        public Byte[] pixelData;
    }

    class BSAVELoader
    {
        public enum DataFormatEnum {  Unknown, TDP, GET_FRAGMENT, SINGLE_EGA_PLANE, EGA_FNT }
        public enum FragmentColorPackingEnum { CGA, EGA, Unknown }

        static int[] PALETTE_EGA = {
	        // r, g, b
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 0, 0, 170,
	        /* 2  */ 0, 170, 0,
	        /* 3  */ 0, 170, 170,
	        /* 4  */ 170, 0, 0,
	        /* 5  */ 170, 0, 170,
	        /* 6  */ 170, 85, 0,
	        /* 7  */ 170, 170, 170,
	        /* 8  */ 85, 85, 85,
	        /* 9  */ 85, 85, 170,
	        /* 10 */ 85, 255, 85,
	        /* 11 */ 85, 255, 255,
	        /* 12 */ 255, 85, 85,
	        /* 13 */ 255, 0, 255,
	        /* 14 */ 255, 255, 0,
	        /* 15 */ 255, 255, 255
        };

        static int[] PALETTE_CGA = {
            // type 1 (Palette 0)
	        // r, g, b
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 0, 170, 170,
	        /* 2  */ 170, 0, 170,
	        /* 3  */ 170, 170, 170,

            // type 2 (Palette 0 High Intensity)
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 85, 255, 255,
	        /* 2  */ 255, 85, 255,
	        /* 3  */ 255, 255, 255,

            // type 3 (Palette 1)
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 0, 170, 0,
	        /* 2  */ 170, 0, 0,
	        /* 3  */ 170, 85, 0,

            // type 4 (Palette 1 High Intensity)
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 85, 255, 85,
	        /* 2  */ 255, 85, 85,
	        /* 3  */ 255, 255, 85,

            // type 5 (Palette 2)
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 0, 170, 170,
	        /* 2  */ 170, 0, 0,
	        /* 3  */ 170, 170, 170,

            // type 6 (Palette 2 High Intensity)
	        /* 0  */ 0, 0, 0,
	        /* 1  */ 85, 255, 255,
	        /* 2  */ 255, 85, 85,
	        /* 3  */ 255, 255, 255,
        };


        BLOADHeader header;

        public FragmentColorPackingEnum FragmentColorPacking ;

        private int _width = -1;
        private int _height = -1;
        private int _dataLength;
        private DataFormatEnum _imageType;

        private int _palette = 0;

        public DataFormatEnum ImageType
        {
            get { return _imageType; }
        }

        public int DataLength
        {
            get { return _dataLength; }
        }

        public int Palette
        {
            set
            {
                _palette = Math.Min (6, Math.Max(1, value));
            }
        }

        List<PixelFragment> fragments = new List<PixelFragment>();


        public BSAVELoader (byte[] data, FragmentColorPackingEnum defaultPacking = FragmentColorPackingEnum.Unknown, int defaultPalette = -1)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);

            header.magic = reader.ReadByte();
            header.baseAddr = reader.ReadUInt16();
            header.offsetAddr = reader.ReadUInt16();
            header.len = reader.ReadUInt16();

            _imageType = DataFormatEnum.Unknown;
            _width = _height = -1;

            FragmentColorPacking = defaultPacking == FragmentColorPackingEnum.Unknown ? FragmentColorPackingEnum.EGA : defaultPacking;
            _palette = defaultPalette == -1 ? 0 : defaultPalette;

            if (header.magic != 0xFD)
                throw new Exception("Magic number is not 0xFD!");

            int actualLen = data.Length - Marshal.SizeOf(typeof (BLOADHeader));
            if (actualLen < header.len - 1)
            {
                throw new Exception(string.Format ("Header length mismatch, expected {0} but got {1}.", header.len, actualLen));
            }
            _dataLength = header.len;

            long bodyStartPos = stream.Position;

            // Assume this is a GET fragment
            int fragmentWidth = reader.ReadUInt16();
            int fragmentHeight = reader.ReadUInt16();
            Console.WriteLine(String.Format("W={0} H={1}", fragmentWidth, fragmentHeight));

            Console.WriteLine("Data size=" + header.len);

            /*
                Format notes:

                === TDP ============================================================================================================

                TDP is full-screen EGA format used in my PantherTek/Turing Degree games.
                    
                TDP stands for Turing Degree Picture.

                TDP is 10 GET fragments, each 320x20 pixels.  So each GET should be 3204 bytes (320/2 = 160 bytes per scanline, 
                x20 scalines for 3200 bytes plus 4 for GET header).

                BASIC code to load and display a TDP:
                    DIM TDP(16020)
                    (BLOAD it)
                    FOR I = 0 TO 180 STEP 20
                    PUT (0, I), TDP(((I \ 20) * 1602))
                    NEXT
                    
                So if TDP, len should be 32040 bytes.  10 GET fragments should follow, exactly 3204 bytes each and with a pixel size
                of 320x20.

                Why did I design this full-screen format this way?  I don't recall, other than something to do with GET/PUT causing
                GWBASIC heap issues if I tried to grab too much screen memory at once.


                === GET fragment ====================================================================================================

                This is a standard GWBASIC/QUICKBASIC GET/PUT screen fragment, dumped straight to disk via BSAVE command.

                Example on how to save one under SCREEN 7:
                    
                FragmentSize% = INT(CINT(15/2) * 15) + 4
                GET (0, 0)-(14, 14), Fragment%
                DEF SEG = VARSEG(Fragment%(0))
                BSAVE "Filename", VARPTR(Fragment%(0)), FragmentSize%
                DEF SEG
                    
                Internally, a GET fragment is just a 4-byte header (Width, height) plus raw pixel data.

                Pixel format is usually straight-up dump from video RAM.  There are no indicators in the GET data
                structure itself to tell you what kind of format it is in.  It's just assumed you will have issued the proper
                SCREEN command so the video memory format matches what's in the BSAVE file.
                    
                EGA is 4-bits per pixel, 2 pixels per byte.  Data is non-linear, in 4 planes.  Each plane represents one RGBI bit.

                CGA is 2-bits per pixel, 4 pixels per byte.  Data is linear.  (Internally, CGA is banked in two interlaced frames,
                accessible as different even/odd planes, but GWBASIC/QB seems to hide that from us and just lets us write linearly.)

                Unused bits are padded to get us up to byte boundaries.  These will always be zero.

                Note: Some of my array calculations were off in my original games, or I re-used arrays which were larger than
                strictly necessary for saving the data, so there will be extra data padding at the end of some my BSAVEs.  So the
                fragment length cannot be trusted to be close to the required size.  (It will always be at least the minimum
                required size, and this checks that.)

                Another note:  CGA width is double in fragment header, so divide it by two when in CGA format.


                === EGA Plane Dump ===================================================================================================

                I don't use many of these types of images, but they are B&W single-bit plane dumps from EGA memory.  They are not GET
                fragments.

                They are decoded the same as EGA GET fragments, except that they only have just one plane's worth of data, so they
                have a fixed plane width of 40 bytes per scanline.

                It's not easy to detect these dumps.  At the moment, they are detected if the data length is exactly 8000 bytes.  So
                far this simple check works, as I rarely saved very big GET fragment sprites.


                === FNT EGA font =====================================================================================================

                Data length of 2340 bytes, arranged in 65 characters of 8x8 GET fragments (each 36 bytes in length).

                Each GET fragment is just a 8x8 chunk of EGA memory.

                I used FNTs in a lot of my early EGA games.  The character set is really simple:  Just A-Z 0-9 with punctuation and
                no lower case.
                           

                === VGA Linear Dump ==================================================================================================

                These are pretty straight forward.  They are just 64000 byte dumps of VGA memory at 0xA000 while in Mode 13 (ie:
                chained linear mode).

                Palette is stored in a seperate .PAL file.

                Currently not decoding these, as I can only remember one time I used this format before I switched to the
                PGF format with my VGA-era games.   (PGF = PantherTek Graphics Format, which was an indexed chunked binary format.)

                    
            */

            if (fragmentWidth == 320 && fragmentHeight == 20 && header.len == 32040)
            {
                Console.WriteLine("We think this is a TDP.");

                FragmentColorPacking = FragmentColorPackingEnum.EGA; // Force EGA packing

                // let's verify our theory that this is an EGA TDP by examining the next GET fragment and see if it still has a size of 320x20
                stream.Position = bodyStartPos + 3200 + 4;
                int fW = reader.ReadUInt16();
                int fH = reader.ReadUInt16();
                if (fW != 320 && fH != 20)
                {
                    throw new Exception(String.Format("Attempted to detect TDP, but second chunk had wrong width/height.  Expected 320,20 but got {0},{1}.", fW, fH));
                }
                _imageType = DataFormatEnum.TDP;
                _width = 320;
                _height = 200;

                // decode these as a series of streams, we should get 10 fragments
                stream.Position = bodyStartPos; // reset stream
                for (int i = 0; i < 10; i++)
                {
                    PixelFragment frag = new PixelFragment();
                    frag.width = reader.ReadUInt16();
                    frag.height = reader.ReadUInt16();

                    if (frag.width != 320 || frag.height != 20)
                        throw new Exception("GET fragment in TDP chunk #" + i + " had unusual w/h: " + frag.width + " " + frag.height);

                    int chunkLen = (int)Math.Round(320.0F * 20.0F / 2.0F);

                    frag.pixelData = new Byte[chunkLen];

                    int dataRead;
                    if ((dataRead = stream.Read(frag.pixelData, 0, chunkLen)) != chunkLen)
                        throw new Exception("Tried to read TDP fragment, but not enough data.  Expected " + header.len + " but got " + dataRead + " bytes.");

                    fragments.Add(frag);
                }
            }   // end TDP
            else if (header.len == 8000 && ((fragmentWidth <= 0 || fragmentWidth >= 320) || (fragmentHeight <= 0 || fragmentHeight >= 200)))
            {
                // Special case, singular EGA plane dump
                _imageType = DataFormatEnum.SINGLE_EGA_PLANE;
                _width = 320;
                _height = 200;

                stream.Position = bodyStartPos; // reset to beginning

                PixelFragment frag = new PixelFragment();
                frag.width = 320;
                frag.height = 200;

                frag.pixelData = new Byte[header.len];
                int amtToRead = header.len;

                int dataRead;
                if ((dataRead = stream.Read(frag.pixelData, 0, amtToRead)) != amtToRead)
                    throw new Exception("Tried to read plane dump, but not enough data.  Expected " + amtToRead + " but got " + dataRead + " bytes.");

                fragments.Add(frag);
            }   // end EGA planar dump
            else if (header.len == 2340 && fragmentWidth == 8 && fragmentHeight == 8)
            {
                // Special case, EGA FNT

                // Check to see if the next chunk is also 8x8
                stream.Position = bodyStartPos + 36;
                int fW = reader.ReadUInt16();
                int fH = reader.ReadUInt16();
                if (fW != 8 && fH != 8)
                    throw new Exception(String.Format("Attempted to detect EGA FNT, but second chunk had wrong width/height.  Expected 8,8 but got {0},{1}.", fW, fH));

                _imageType = DataFormatEnum.EGA_FNT;
                _width = 320;
                _height = 200;

                stream.Position = bodyStartPos; // reset to beginning

                for (int i = 0; i < 65; i++)
                {
                    PixelFragment frag = new PixelFragment();
                    frag.width = reader.ReadUInt16();
                    frag.height = reader.ReadUInt16();

                    if (frag.width != 8 || frag.height != 8)
                        throw new Exception("GET fragment in EGA FNT chunk #" + i + " had unusual w/h: " + frag.width + " " + frag.height);

                    int chunkLen = 32;
                    frag.pixelData = new Byte[chunkLen];

                    int dataRead;
                    if ((dataRead = stream.Read(frag.pixelData, 0, chunkLen)) != chunkLen)
                        throw new Exception("Tried to read EGA FNT fragment, but not enough data.  Expected " + header.len + " but got " + dataRead + " bytes.");

                    fragments.Add(frag);
                }
            }   // end EGA FNT
            else  // Otherwise, if all other cases fail, we're just a normal GET fragment
            {
                _imageType = DataFormatEnum.GET_FRAGMENT;
                _width = fragmentWidth;
                _height = fragmentHeight;

                PixelFragment frag = new PixelFragment();
                frag.width = (ushort)fragmentWidth;
                frag.height = (ushort)fragmentHeight;

                frag.pixelData = new Byte[header.len];
                int amtToRead = header.len - 4; // minus four bytes for width/height

                int dataRead;
                if ((dataRead = stream.Read (frag.pixelData, 0, amtToRead)) != amtToRead)
                    throw new Exception ("Tried to read singular fragment, but not enough data.  Expected " + amtToRead + " but got " + dataRead + " bytes.");

                fragments.Add(frag);
            } // GET fragment
        }


        public int GetPlaneWidth (int width, FragmentColorPackingEnum format)
        {
            if (format == FragmentColorPackingEnum.CGA)  // no planes in CGA, so it's just 1:1
                return 1;
            if (format == FragmentColorPackingEnum.EGA)  // EGA is stored in 4 planes per scanline, so width of each plane is pixelWidth / (4 planes * 2 pixels per byte)
                return (int)Math.Ceiling ((float)width / 8.0F);

            throw new Exception("GetPixelsPerByte: Unknown format.");
        }

        public int GetBytesPerScanline(int width, FragmentColorPackingEnum format)
        {
            if (format == FragmentColorPackingEnum.CGA)
                return (int)Math.Ceiling((float)width / 4.0F); // just width of pixels/4
            if (format == FragmentColorPackingEnum.EGA)
                return GetPlaneWidth (width, format) * 4; // 4 planes, so width of each plane *4

            throw new Exception("GetPixelsPerByte: Unknown format.");
        }

        public void RasterizeFragment (PixelFragment frag, Bitmap targetBitmap, FragmentColorPackingEnum currFormat, Point offset)
        {
            // for CGA, width is double the amount in the fragment data, so adjust accordingly
            int adjustedWidth = currFormat == FragmentColorPackingEnum.CGA ? frag.width / 2 : frag.width;

            int planeWidth = GetPlaneWidth(adjustedWidth, currFormat);
            int bytesPerScanline = GetBytesPerScanline(adjustedWidth, currFormat);

            // check to make sure there is enough data for this
            if (frag.pixelData.Length < frag.height * bytesPerScanline)
                throw new Exception("Not enough data to unpack:  Requesting " + (frag.height * bytesPerScanline) + " bytes for " + FragmentColorPacking.ToString() + " format, but only have " + frag.pixelData.Length + " bytes.");

            if (currFormat == FragmentColorPackingEnum.EGA)
            {
                for (int py = 0; py < frag.height; py++)
                {
                    for (int px = 0; px < adjustedWidth; px++)
                    {
                        Color color = Color.Black;
                        // Each EGA byte presents two pixels, and each bit in the byte presents a single EGA plane access
                        int bitMask = (1 << (7 - (px % 8)));

                        // Shuffle data from the different 4 planes (which are stored sequentially for each scanline)
                        int data_plane0 = (frag.pixelData[(py * bytesPerScanline) + ((px / 8)) + (planeWidth * 0)] & bitMask) > 0 ? 1 : 0; // Blue plane
                        int data_plane1 = (frag.pixelData[(py * bytesPerScanline) + ((px / 8)) + (planeWidth * 1)] & bitMask) > 0 ? 1 : 0; // Green plane
                        int data_plane2 = (frag.pixelData[(py * bytesPerScanline) + ((px / 8)) + (planeWidth * 2)] & bitMask) > 0 ? 1 : 0; // Red plane
                        int data_plane3 = (frag.pixelData[(py * bytesPerScanline) + ((px / 8)) + (planeWidth * 3)] & bitMask) > 0 ? 1 : 0; // Intensity plane

                        // just convert back to index color value and find in our CLUT
                        int pixelValue = data_plane0 + (data_plane1 << 1) + (data_plane2 << 2) + (data_plane3 << 3);
                        color = Color.FromArgb(0xFF, PALETTE_EGA[pixelValue * 3], PALETTE_EGA[pixelValue * 3 + 1], PALETTE_EGA[pixelValue * 3 + 2]);

                        targetBitmap.SetPixel(offset.X + px, offset.Y + py, color);
                    }
                }
            } else if (currFormat == FragmentColorPackingEnum.CGA)
            {
                for (int py = 0; py < frag.height; py++)
                {
                    for (int px = 0; px < adjustedWidth; px++)
                    {
                        Color color = Color.Black;
                        // Each CGA byte presents 4 pixels
                        int bitMask = px % 4;
                        // So access each pixel as a 2-bit value from the byte, from MSB to LSB
                        int pixelValue = frag.pixelData[(px / 4) + (py * bytesPerScanline)] >> ((3 - bitMask) * 2);
                        pixelValue &= 0x3;

                        // just convert back to index color value and find in our CLUT (from 6 different CGA palettes)
                        int palIndex = (pixelValue * 3) + (_palette * 12);
                        color = Color.FromArgb(0xFF, PALETTE_CGA[palIndex], PALETTE_CGA[palIndex + 1], PALETTE_CGA[palIndex + 2]);

                        targetBitmap.SetPixel(offset.X + px, offset.Y + py, color);
                    }
                }
            }
        }

        public void RasterizeRawEGAPlaneDump(PixelFragment frag, Bitmap targetBitmap, FragmentColorPackingEnum currFormat, Point offset)
        {
            // for CGA, width is double the amount in the fragment data
            int bytesPerScanline = (int)Math.Ceiling((float)frag.width / 8.0F);

            // check to make sure there is enough data for this
            if (frag.pixelData.Length < frag.height * bytesPerScanline)
                throw new Exception("Not enough data to unpack:  Requesting " + (frag.height * bytesPerScanline) + " bytes for planar EGA, but only have " + frag.pixelData.Length + " bytes.");

            for (int py = 0; py < frag.height; py++)
            {
                for (int px = 0; px < frag.width; px++)
                {
                    Color color = Color.Black;
                    int bitMask = (1 << (7 - (px % 8)));
                    int data_plane0 = (frag.pixelData[(py * bytesPerScanline) + ((px / 8))] & bitMask) > 0 ? 1 : 0; // Blue

                    if (data_plane0 > 0)
                        color = Color.FromArgb(0xFF, PALETTE_EGA[15 * 3], PALETTE_EGA[15 * 3 + 1], PALETTE_EGA[15 * 3 + 2]);

                    targetBitmap.SetPixel(offset.X + px, offset.Y + py, color);
                }
            }
        }

        public void ConvertToBitmap(Bitmap targetBitmap, FragmentColorPackingEnum currFormat = FragmentColorPackingEnum.Unknown, int palette = -1)
        {            
            if (currFormat == FragmentColorPackingEnum.Unknown)
                currFormat = FragmentColorPacking;

            if (palette != -1)
                _palette = palette;

            // unpack based on format
            if (_imageType == DataFormatEnum.GET_FRAGMENT)
            {
                PixelFragment frag = fragments[0];
                RasterizeFragment(frag, targetBitmap, currFormat, new Point(0, 0));

            }
            else if (_imageType == DataFormatEnum.SINGLE_EGA_PLANE)
            {
                PixelFragment frag = fragments[0];
                RasterizeRawEGAPlaneDump(frag, targetBitmap, currFormat, new Point(0, 0));
            }
            else if (_imageType == DataFormatEnum.EGA_FNT)
            {
                int x = 0, y = 0;
                for (int i = 0; i < 65; i++)
                {
                    RasterizeFragment(fragments[i], targetBitmap, currFormat, new Point(x, y));
                    x += 8;
                    if (x > 200)
                    {
                        x = 0;
                        y += 8;
                    }
                }

            }
            else if (_imageType == DataFormatEnum.TDP)
            {
                for (int i = 0; i < 10; i++)
                    RasterizeFragment(fragments[i], targetBitmap, currFormat, new Point(0, i * 20));
            }
        }

        public override string ToString()
        {
            string info = "";
            info += String.Format("Size: {0}, {1}\nBSAVE format: {2}\n", _width, _height, _imageType.ToString());
            info += String.Format("Color Packing: {0}\n", FragmentColorPacking.ToString());
            if (FragmentColorPacking == FragmentColorPackingEnum.CGA)
                info += String.Format ("CGA Palette: {0}\n", (_palette + 1));
            info += String.Format("GET_FRAGMENTS: {0}\nBSAVE data length: {1}", fragments.Count, _dataLength);

            return info;
        }

        public int GetBitmapWidth(FragmentColorPackingEnum currFormat = FragmentColorPackingEnum.Unknown)
        {
            if (currFormat == FragmentColorPackingEnum.Unknown)
                currFormat = FragmentColorPacking;

            if (_imageType == DataFormatEnum.SINGLE_EGA_PLANE || _imageType == DataFormatEnum.TDP || _imageType == DataFormatEnum.EGA_FNT)
                return 320;

            if (currFormat == FragmentColorPackingEnum.CGA)
                return _width / 2;
            else
                return _width;
        }

        public int GetBitmapHeight(FragmentColorPackingEnum currFormat = FragmentColorPackingEnum.Unknown)
        {
            if (currFormat == FragmentColorPackingEnum.Unknown)
                currFormat = FragmentColorPacking;

            if (_imageType == DataFormatEnum.SINGLE_EGA_PLANE || _imageType == DataFormatEnum.TDP || _imageType == DataFormatEnum.EGA_FNT)
                return 200;

            return _height;
        }

    }
}
