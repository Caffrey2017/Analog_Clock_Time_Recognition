using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ACTR
{
    /// <summary>
    /// A view model for each displayed image
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        // TODO 
        // http://faithlife.codes/blog/2008/04/memory_leak_with_bitmapimage_and_memorystream/

        /// <summary>
        /// Currently selected processing step
        /// </summary>
        public int SelectedStep { get; set; } = -1;


        /// <summary>
        /// Source image full path
        /// </summary>
        public BitmapImage SourceFullPath
        {
            get
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                //bi.DecodePixelWidth = 30;
                bi.StreamSource = new MemoryStream(File.ReadAllBytes(Path.Combine(Helper.GetImagesDirectory(), "source.png")));
                bi.EndInit();
                return bi;
            }
            set
            {
                SourceFullPath = value;
            }

        }


        /// <summary>
        /// Processed image full path
        /// </summary>
        public BitmapImage ProcessedFullPath
        {
            get
            {
                if (SelectedStep == -1)
                {
                    return null;
                }
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                //bi.DecodePixelWidth = 30;
                bi.StreamSource = new MemoryStream(File.ReadAllBytes(Path.Combine(Helper.GetImagesDirectory(), (ProcessingSteps.Steps)SelectedStep + ".png")));
                bi.EndInit();               
                return bi;
            }
            set
            {
                ProcessedFullPath = value;
            }
        }


        /// <summary>
        /// Time read from clock face
        /// </summary>
        public string Time { get; set; }

        public ImageViewModel()
        {
           
            this.SelectedStep = -1;
            this.ProcessedFullPath = null;
            this.Time = DateTime.Now.ToString("HH:mm:ss");
        }

    }
}
