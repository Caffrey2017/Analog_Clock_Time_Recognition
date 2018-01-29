using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACTR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Directory.Exists(Helper.GetImagesDirectory()))
            {
                Directory.Delete(Helper.GetImagesDirectory(), true);
            }
            // Set DataContext to ViewModel
            this.DataContext = new ImageViewModel(DateTime.Now);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string sourceImagePath;

            sourceImagePath = Helper.GetImageDialog();

            var process = new ImageProcessing();
            var timeRead = process.ProcessImage(sourceImagePath);
 

            this.DataContext = new ImageViewModel(timeRead);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
