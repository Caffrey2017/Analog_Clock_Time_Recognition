using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace ACTR
{
    /// <summary>
    /// Image processing stuff goes here
    /// </summary>
    public static class ImageProcessing
    {
        /// <summary>
        /// Working directory for saving processed images
        /// </summary>
        private static string imagesWorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");

        /// <summary>
        /// Processes source image
        /// </summary>
        public static void ProcessImage(string path)
        {
            // Load source image
            Image<Bgr, Byte> sourceImage;
            sourceImage = LoadImage(path);

            // Get gray image
            Image<Gray, Byte> grayImage;
            grayImage = GetGrayImage(sourceImage);

            // Get blurred image, noise reduction
            Image<Gray, Byte> blurredImage;
            blurredImage = GetBlurredImage(grayImage);

            // Get gradient
            Image<Gray, Single> gradientImage;
            gradientImage = GetGradient(blurredImage);

            // Get clock face
            GetCircle();


            // Read time
            GetTime();
        }

        /// <summary>
        /// Reads time from preprocessed clock face
        /// </summary>
        public static void GetTime()
        {

        }

        /// <summary>
        /// Loads selected image
        /// </summary>
        /// <param name="path"></param>
        public static Image<Bgr, Byte> LoadImage(string path)
        {
            Image<Bgr, Byte> sourceImage;
            string filePath;

            sourceImage = new Image<Bgr, Byte>(path);

            filePath = Path.Combine(imagesWorkingDirectory, "source.png");
            Directory.CreateDirectory(imagesWorkingDirectory);

            CvInvoke.Imwrite(filePath, sourceImage);

            return sourceImage;
        }

        /// <summary>
        /// Gets and saves gray image
        /// </summary>
        public static Image<Gray, Byte> GetGrayImage(Image<Bgr, Byte> sourceImage)
        {
            Image<Gray, Byte> grayImage;
            grayImage = new Image<Gray, byte>(sourceImage.Width, sourceImage.Height);
            string filePath;

            CvInvoke.CvtColor(sourceImage, grayImage, ColorConversion.Bgr2Gray);

            filePath = Path.Combine(imagesWorkingDirectory, "gray.png");
            CvInvoke.Imwrite(filePath, grayImage);

            return grayImage;
        }

        /// <summary>
        /// Gets and saves blurred image
        /// </summary>
        public static Image<Gray, Byte> GetBlurredImage(Image<Gray, Byte> grayImage)
        {
            Image<Gray, Byte> blurredImage;
            blurredImage = new Image<Gray, Byte>(grayImage.Width, grayImage.Height);
            string filePath;

            Size ksize = new Size { Height = 3, Width = 3 };
            CvInvoke.GaussianBlur(grayImage, blurredImage, ksize, 5);

            filePath = Path.Combine(imagesWorkingDirectory, "blurred.png");
            CvInvoke.Imwrite(filePath, blurredImage);

            return blurredImage;
        }


        /// <summary>
        /// Gets and saves gradient of an image
        /// </summary>
        public static Image<Gray, Single> GetGradient(Image<Gray, Byte> blurredImage)
        {
            Image<Gray, Single> blurredSingle = blurredImage.Convert<Gray, Single>();
            Image<Gray, Single> gradientImage;
            string filePath;

            gradientImage = blurredImage.Laplace(5);
            //gradientImage = (blurredSingle.Sobel(1, 0, 3)).Add(blurredSingle.Sobel(0, 1, 3));//.AbsDiff(new Gray(0.0));
                        
            filePath = Path.Combine(imagesWorkingDirectory, "gradient.png");
            CvInvoke.Imwrite(filePath, gradientImage);

            return gradientImage;
        }


        /// <summary>
        /// Gets and saves image of an isolated clock face
        /// </summary>
        public static void GetCircle()
        {

        }



    }
}
