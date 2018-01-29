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
        DateTime time = DateTime.Now;

        public ProcessingSteps.Steps CurrentStep
        {
            get
            {
               return (ProcessingSteps.Steps)SelectedStep;
            }
            set
            {
                CurrentStep = value;
            }
        }


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
                try
                {
                    bi.StreamSource = new MemoryStream(File.ReadAllBytes(Path.Combine(Helper.GetImagesDirectory(), "source.png")));
                    bi.EndInit();
                }
                catch{}                
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
                try
                {
                    bi.StreamSource = new MemoryStream(File.ReadAllBytes(Path.Combine(Helper.GetImagesDirectory(), CurrentStep + ".png")));
                    bi.EndInit();
                }
                catch { }
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

        public ImageViewModel(DateTime timeRead)
        {
            this.CurrentStep = ProcessingSteps.Steps.none;
            this.SelectedStep = -1;
            this.ProcessedFullPath = null;
            this.Time =timeRead.ToString("HH:mm:ss");
        }

    }
}
