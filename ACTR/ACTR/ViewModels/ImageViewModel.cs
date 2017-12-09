using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTR
{
    /// <summary>
    /// A view model for each displayed image
    /// </summary>
    class ImageViewModel : BaseViewModel
    {
        /// <summary>
        /// A path to saved image
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Name of an image
        /// </summary>
        public string Name { get; set; }
    }
}
