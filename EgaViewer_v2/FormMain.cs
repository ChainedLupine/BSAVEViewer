using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using FolderSelect;

namespace EgaViewer_v2
{
    public partial class FormMain : Form
    {
        string currPath = Properties.Settings.Default.LastDirectory;
        string currFilePath;

        const string TEXT_DOUBLECLICK = "Double click a file to load.";

        byte[] smileyBitmap = {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 1, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 1, 0, 0, 0, 1, 0,
            0, 0, 1, 1, 1, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };

        byte[] frownyBitmap = {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 1, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 1, 1, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };

        Bitmap EGAimage;

        BSAVELoader loader;

        public FormMain()
        {
            this.DoubleBuffered = true;

            EGAimage = new Bitmap(7, 7, PixelFormat.Format32bppArgb);

            InitializeComponent();
            PopulateFiles();

            toolStripStatusText.Text = TEXT_DOUBLECLICK;

            CreateBlankImage();

            pictureBoxEGA.InterpolationMode = InterpolationMode.NearestNeighbor;
            AutoScaleImage();

            SetPickedFormat(BSAVELoader.FragmentColorPackingEnum.EGA);

        }

        private void PopulateFiles()
        {
            listBoxFiles.Items.Clear();

            string[] egaFiles = Directory.GetFiles(currPath, "*.*")
                                     .Select(path => Path.GetFileName(path))
                                     .ToArray();

            foreach (string file in egaFiles)
            {
                listBoxFiles.Items.Add(file);
            }
        }

        private void SetPickedFormat (BSAVELoader.FragmentColorPackingEnum format)
        {
            var i = 0;
            foreach (ToolStripItem item in pixelFormatToolStripMenuItem.DropDownItems)
            {
                if (i == (int)format)
                    ((ToolStripMenuItem)item).Checked = true;
                else
                    ((ToolStripMenuItem)item).Checked = false;
                i++;
            }


        }

        private BSAVELoader.FragmentColorPackingEnum GetPickedFormat ()
        {
            var i = 0;
            foreach (ToolStripItem item in pixelFormatToolStripMenuItem.DropDownItems)
            {
                if (((ToolStripMenuItem)item).Checked)
                    //return (BSAVELoader.FragmentColorPackingEnum)Enum.Parse(typeof(BSAVELoader.FragmentColorPackingEnum), item.Text);
                    return (BSAVELoader.FragmentColorPackingEnum)i;
                i++;
            }

            return BSAVELoader.FragmentColorPackingEnum.Unknown;
        }

        private int GetPickedPalette()
        {
            var i = 0;
            foreach (ToolStripItem item in CGAPaletteToolStripMenuItem.DropDownItems)
            {
                if (((ToolStripMenuItem)item).Checked)
                    return i;
                i++;
            }

            return -1;

        }

        private void listBoxFiles_DoubleClick(object sender, EventArgs e)
        {
            ListBox control = (ListBox)sender;
            currFilePath = Path.Combine(currPath, control.Text);

            LoadAndRasterizeCurrentFile();
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void SetZoom (int zoom)
        {
            pictureBoxEGA.Image = ResizeImage(EGAimage, EGAimage.Width * zoom, EGAimage.Height * zoom);
            trackBarZoom.Value = zoom;
        }

        private void AutoScaleImage()
        {
            int zoom = (int)Math.Floor (Math.Min((float)pictureBoxEGA.Width / (float)EGAimage.Width, (float)pictureBoxEGA.Height / (float)EGAimage.Height));
            zoom = Math.Max (1, Math.Min(100, zoom));
            SetZoom(zoom);
        }

        private void UpdateImage()
        {
            if (loader != null)
            {
                LoadAndRasterizeCurrentFile();
            }
        }

        private void LoadAndRasterizeCurrentFile()
        {
            toolStripStatusText.Text = "Loading " + currFilePath;

            byte[] data = File.ReadAllBytes(currFilePath);
            try
            {
                loader = new BSAVELoader(data, GetPickedFormat(), GetPickedPalette());
                toolStripStatusText.Text = "Loaded " + currFilePath + " Type: " + loader.ImageType.ToString() + " Dim: " + loader.GetBitmapWidth() + ", " + loader.GetBitmapHeight();
                labelFileInfo.Text = loader.ToString();
                SetPickedFormat(loader.FragmentColorPacking);

                EGAimage = new Bitmap(loader.GetBitmapWidth(), loader.GetBitmapHeight());
                loader.ConvertToBitmap(EGAimage);
                AutoScaleImage();
            }
            catch (Exception ep)
            {
                labelFileInfo.Text = "N/A";
                Console.WriteLine("ERROR: " + ep.Message);
                Console.WriteLine("ERROR: " + ep.StackTrace);
                toolStripStatusText.Text = "ERROR: " + ep.Message;

                EGAimage = new Bitmap(7, 7, PixelFormat.Format32bppArgb);
                CreateErrorImage();
                AutoScaleImage();
            }
        }

        unsafe private void CreateBlankImage()
        {
            BitmapData data = EGAimage.LockBits (new Rectangle (0, 0, 7, 7), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte* dataPtr = (byte*)data.Scan0;
            int smileyIdx = 0;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    dataPtr[0] = (byte)(0xFF & (smileyBitmap[smileyIdx] * 0xFF));
                    dataPtr[1] = (byte)(0xFF & (smileyBitmap[smileyIdx] * 0xFF));
                    dataPtr[2] = (byte)(0xFF & (smileyBitmap[smileyIdx] * 0xFF));
                    dataPtr[3] = 255;
                    smileyIdx++;
                    dataPtr += 4;
                }
            }


            EGAimage.UnlockBits(data);
        }

        unsafe private void CreateErrorImage()
        {
            BitmapData data = EGAimage.LockBits(new Rectangle(0, 0, 7, 7), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte* dataPtr = (byte*)data.Scan0;
            int frownyIdx = 0;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    dataPtr[0] = 0; 
                    dataPtr[1] = 0;
                    dataPtr[2] = (byte)(0xFF & (frownyBitmap[frownyIdx] * 0xFF));
                    dataPtr[3] = 255;
                    frownyIdx++;
                    dataPtr += 4;
                }
            }


            EGAimage.UnlockBits(data);
        }

        private void trackBarZoom_ValueChanged(object sender, EventArgs e)
        {
            TrackBar control = (TrackBar)sender;
            SetZoom(control.Value);
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            AutoScaleImage();
        }

        private void openPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fsd = new FolderSelectDialog();
            fsd.Title = "Select a folder to browse.";
            fsd.InitialDirectory = Properties.Settings.Default.LastDirectory;
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                Console.WriteLine(fsd.FileName);
                currPath = fsd.FileName;
                Properties.Settings.Default.LastDirectory = currPath;
                Properties.Settings.Default.Save();

                PopulateFiles();
            }
        }

        private void MenuItem_Options_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem currItem = sender as ToolStripMenuItem;

            foreach (ToolStripMenuItem item in (currItem.OwnerItem as ToolStripMenuItem).DropDownItems)
            {
                item.Checked = currItem == item;
            }

            UpdateImage();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program was created by ChainedLupine (aka David Grace).\n\nView it at https://github.com/ChainedLupine/BSAVEViewer");
        }
    }
}
