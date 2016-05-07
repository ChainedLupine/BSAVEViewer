# BSAVEViewer
This program will load and display GWBASIC/QUICKBASIC BSAVE graphics files.

I 2002, I wrote a C++/SDL program which would read the [BSAVE file format][1] I used to save the graphics from my late 80s/early 90s DOS BASIC games.

And in 2016 I re-wrote this program in C#/WinForms so I can view these files again.  However, this version is complete.  My original C++ version only viewed TDP EGA format files, but this one will view TDP, FNT, standard GET fragments, and both EGA and CGA format.

Basically, it can view everything up to my mid-90s Pascal/Assembler era games, where I switched from CGA/EGA to using exclusively MCGA graphics mode (320x200 256 colors).

[1]: https://en.wikipedia.org/wiki/BSAVE_(bitmap_format)

## BSAVE Format

BSAVEs are just dumps of DOS memory, usually inside the BASIC memory space.  GWBASIC and QUICKBASIC use the same format.

The first byte is always 0xFD.  This is the magic number for a BSAVE file.  A header then follows.

* Base Segment (ushort)
* Base Offset (ushort)
* Length of data (ushort)

I can safely ignore the segment/offset numbers.  These were direct pointers into BASIC memory space for the variable which was saved, so I do not care about that.


## TDP

TDP is full-screen EGA format used in my PantherTek/Turing Degree games.
	
TDP stands for Turing Degree Picture.

TDP is 10 GET fragments, each 320x20 pixels.  So each GET should be 3204 bytes (320/2 = 160 bytes per scanline, 
x20 scalines for 3200 bytes plus 4 for GET header).

BASIC code to load and display a TDP:

```
	DIM TDP(16020)
	
	(BLOAD it)
	FOR I = 0 TO 180 STEP 20
	PUT (0, I), TDP(((I \ 20) * 1602))
	NEXT
```

So if TDP, len should be 32040 bytes.  10 GET fragments should follow, exactly 3204 bytes each and with a pixel size
of 320x20.

Why did I design this full-screen format this way?  I don't recall, other than something to do with GET/PUT causing
GWBASIC heap issues if I tried to grab too much screen memory at once.

## GET fragment

This is a standard GWBASIC/QUICKBASIC GET/PUT screen fragment, dumped straight to disk via BSAVE command.

Example on how to save one under SCREEN 7:
	
```
	FragmentSize% = INT(CINT(15/2) * 15) + 4
	GET (0, 0)-(14, 14), Fragment%
	DEF SEG = VARSEG(Fragment%(0))
	BSAVE "Filename", VARPTR(Fragment%(0)), FragmentSize%
	DEF SEG
```

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


## EGA Plane Dump

I don't use many of these types of images, but they are B&W single-bit plane dumps from EGA memory.  They are not GET
fragments.

They are decoded the same as EGA GET fragments, except that they only have just one plane's worth of data, so they
have a fixed plane width of 40 bytes per scanline.

It's not easy to detect these dumps.  At the moment, they are detected if the data length is exactly 8000 bytes.  So
far this simple check works, as I rarely saved very big GET fragment sprites.


## FNT EGA font

Data length of 2340 bytes, arranged in 65 characters of 8x8 GET fragments (each 36 bytes in length).

Each GET fragment is just a 8x8 chunk of EGA memory.

I used FNTs in a lot of my early EGA games.  The character set is really simple:  Just A-Z 0-9 with punctuation and
no lower case.
		   

## VGA Linear Dump

These are pretty straight forward.  They are just 64000 byte dumps of VGA memory at 0xA000 while in Mode 13 (ie:
chained linear mode).

Palette is stored in a seperate .PAL file.

Currently not decoding these, as I can only remember one time I used this format before I switched to the
PGF format with my VGA-era games.   (PGF = PantherTek Graphics Format, which was an indexed chunked binary format.)




