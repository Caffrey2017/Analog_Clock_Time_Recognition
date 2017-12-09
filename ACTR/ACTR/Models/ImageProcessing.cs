using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTR
{
    /// <summary>
    /// Image processing stuff goes here
    /// </summary>
    public static class ImageProcessing
    {

        /// <summary>
        /// Processes source image
        /// </summary>
        public static void ProcessImage(string path)
        {
            // Load source image
            LoadImage(path);

            // Get gray image
            GetGrayImage();

            // Get blurred image, noise reduction
            GetBlurredImage();

            // Get gradient
            GetGradient();

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
        public static void LoadImage(string path)
        {


        }

        /// <summary>
        /// Gets and saves gray image
        /// </summary>
        public static void GetGrayImage()
        {

        }

        /// <summary>
        /// Gets and saves blurred image
        /// </summary>
        public static void GetBlurredImage()
        {

        }


        /// <summary>
        /// Gets and saves gradient of an image
        /// </summary>
        public static void GetGradient()
        {

        }


        /// <summary>
        /// Gets and saves image of an isolated clock face
        /// </summary>
        public static void GetCircle()
        {

        }



    }
}
