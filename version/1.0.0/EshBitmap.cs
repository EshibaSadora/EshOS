//Версия 1.0.0

using System;
using System.Collections;
using  System.Drawing;
using System.Drawing.Imaging;

namespace Eshiba
{


    class hexbitmap
    {
        bool[] pixelmap;
        byte[] colormap;

        int height;
        int width;

        public hexbitmap(int _width, int _height)
        {
            height = _height;
            width = _width;
            pixelmap = new bool[width * height];
            colormap = new byte[width * height];
        }

        public void SetHex(int posx, int posy, int bmpwidth, int bmpheight, byte[] data)
        {
            BitArray array = new BitArray(data.Length * 8);
            for (int i = 0; i < data.Length; i++)
            {
                for (int a = 0; a < 8; a++)
                {
                    array[i * 8 + a] = GetBit(data[i], a);
                }
            }         

            for (int y = 0; y < bmpheight; y++)
            {
                for (int x = 0; x < bmpwidth; x++)
                {
                    pixelmap[posx + x + ((y + posy) * width)] = array[x * y];
                    colormap[posx + x + ((y + posy) * width)] = 10;
                }
            }
        }

        public System.Drawing.Bitmap GetBitmap()
        {
            // System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height, PixelFormat.Format8bppIndexed);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);

            /*

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);


            for (int i = 0; i < width * height; i++)
            {
                if (pixelmap[i] == true) rgbValues[i] = colormap[i];
                if (pixelmap[i] == false) rgbValues[i] = 0;
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

    */

            for (int y = 0; y < height; y++)
            {
                for(int x =0; x < width; x++)
                {
                    int i = x + (y * width);
                    if (pixelmap[i] == true) bmp.SetPixel(x, y, Color.Cyan);
                }
            }




            return bmp;
        }

        bool GetBit(byte value, int pos)
        {
            //bool a = (bool)(((byte)value & (1 << 7-pos)) != 0); 
            return (bool)(((byte)value & (1 << pos)) != 0);
        }

        void SetBit(ref byte aByte, int pos, bool value)
        {
            if (value)
            {
                aByte = (byte)(aByte | (1 << pos));
            }
            else
            {
                aByte = (byte)(aByte & ~(1 << pos));
            }
        }

    }


}
