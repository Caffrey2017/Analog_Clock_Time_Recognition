using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTR
{
    public class ProcessingSteps
    {
        public enum Steps
        {
            none = -1,
            gray,
            blurred,
            circle,
            gradient,
            lines
        }
    }
}
