using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssigweAssignment
{
    public class SearchedWord
    {
        public SearchedWord()
        {
            searchedFolders = new List<searchedFile>();
        }
        public int Id { get; set; }
        public string Word { get; set; } 
        public int NoOfSearchedTime { get; set; }
        public List<searchedFile> searchedFolders { get; set; }
        public DateTime searched { get; set; }
    }
}
