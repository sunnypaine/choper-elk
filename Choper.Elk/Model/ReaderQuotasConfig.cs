using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Model
{
    public class ReaderQuotasConfig
    {
        public int MaxDepth { get; set; }

        public int MaxStringContentLength { get; set; }

        public int MaxArrayLength { get; set; }

        public int MaxBytesPerRead { get; set; }

        public int MaxNameTableCharCount { get; set; }
    }
}
