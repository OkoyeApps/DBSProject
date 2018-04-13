using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssigweAssignment
{
    public class Files
    {
        public int No { get; set; }
        public string FileName { get; set; }
        public string FileFullName { get; set; }
        public long FileLength { get; set; }
        public string RootFolder { get; set; }
        public string CurrentDirectory { get; set; }
        public string FileExtension { get; set; }
        public DateTime TimeFound { get; set; }
    }
}
