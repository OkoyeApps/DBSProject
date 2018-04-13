using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssigweAssignment
{
    public class FolderInformation
    {
        public int No { get; set; }
        public string RootDirectoryName { get; set; }
        public double NoOfFiles { get; set; }
        public double NoOfTxtFiles { get; set; }
        public List<Folder> SubDirectories { get; set; }
        public double NoOfSubDirectories { get; set; }
        public DateTime TimeFound { get; set; }
    }
}
