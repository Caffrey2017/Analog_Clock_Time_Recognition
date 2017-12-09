using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTR
{
    class Helper
    {
        public static string GetImageDialog()
        {
            Microsoft.Win32.OpenFileDialog dialogBox;

            dialogBox = new Microsoft.Win32.OpenFileDialog();

            dialogBox.DefaultExt = ".png";
            dialogBox.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dialogBox.ShowDialog();

            if (result == true)
            {
                string filename = dialogBox.FileName;
                return filename;
            }
            return null;
        }
        public static string GetImagesDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Images");
        }
    }
}
