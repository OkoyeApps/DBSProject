using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssigweAssignment
{
  public class HtmlSearchManager
    {
        public void searchWordInHtmlPage(string path)
        {
            if (path.EndsWith(".Html"))
            {
                if (File.Exists(path))
                {
                    var HtmlContent = File.ReadAllText(path);
                }
            }
        }
    }
}
