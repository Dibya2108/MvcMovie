using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace MvcMovie.Utils
{
    public class ImageManipulate
    {
        public static void ResizeImage(string originalFile, string newFile, int newWidth, int maxHeight)
        {
            Image image = Image.FromFile(originalFile);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);


            int newHeight = image.Height * newWidth / image.Width;

            if (newHeight > maxHeight)
            {

                // Resize with height instead

                newWidth = image.Width * maxHeight / image.Height;

                newHeight = maxHeight;

            }


            Image newImage = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            // Save resized picture

            newImage.Save(newFile, ImageFormat.Png);
            newImage.Dispose();
            image.Dispose();
        }

        public static void ResizeImageIfNeeded(string originalFile, string newFile, int newWidth, int maxHeight)
        {
            Image image = Image.FromFile(originalFile);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);


            if (image.Width > newWidth)
            {
                int newHeight = image.Height * newWidth / image.Width;

                if (newHeight > maxHeight)
                {

                    // Resize with height instead

                    newWidth = image.Width * maxHeight / image.Height;

                    newHeight = maxHeight;

                }

                Image newImage = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
                // Save resized picture

                newImage.Save(newFile, ImageFormat.Png);
                newImage.Dispose();
                image.Dispose();
            }
            else
            {

                image.Save(newFile, ImageFormat.Png);

                image.Dispose();
            }



        }

        public static void ResizeImageCompressed(string originalFile, string newFile, int newWidth, int maxHeight)
        {
            Image image = Image.FromFile(originalFile);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);


            int newHeight = image.Height * newWidth / image.Width;

            if (newHeight > maxHeight)
            {

                // Resize with height instead

                newWidth = image.Width * maxHeight / image.Height;

                newHeight = maxHeight;

            }


            Image newImage = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            // Save resized picture

            newImage.Save(newFile, ImageFormat.Jpeg);
            newImage.Dispose();
            image.Dispose();
        }

        public static void RotateImage(string originalFile, string newFile, int angle)
        {
            Image image = Image.FromFile(originalFile);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);


            //int newHeight = image.Height * newWidth / image.Width;

            //if (newHeight > maxHeight)
            //{

            //    // Resize with height instead

            //    newWidth = image.Width * maxHeight / image.Height;

            //    newHeight = maxHeight;

            //}


            //Image newImage = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            // Save resized picture

            image.Save(newFile, ImageFormat.Png);
            image.Dispose();
            image.Dispose();
        }
    }
}