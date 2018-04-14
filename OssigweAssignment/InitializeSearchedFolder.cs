using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OssigweAssignment
{
    public class InitializeSearchedFolder
    {
        
        public void LoadDirectory(string Dir, TreeView treeview, ProgressBar progressBar)
        {
            DirectoryInfo  di = new DirectoryInfo(Dir);
            //setting progress bar maximum value
            //set this later
            progressBar.Maximum = Directory.GetFiles(Dir, "*.txt*", SearchOption.AllDirectories).Length + Directory.GetDirectories(Dir, "*", SearchOption.AllDirectories).Length;

            TreeNode Tds = treeview.Nodes.Add(di.Name);
            Tds.Tag = di.FullName;
            Tds.StateImageIndex = 0;
            LoadFiles(Dir, Tds, progressBar);
            LoadSubDirectories(Dir, Tds, progressBar);

        }
        public void LoadSubDirectories(string dir, TreeNode treeNode, ProgressBar progressBar)
        {
            //Get all SubDirectories
            string[] subDirectoryEntries = Directory.GetDirectories(dir);
            //loop through them to see if they have any other sub-directories
            foreach (var subDirectory in subDirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subDirectory);
                TreeNode tds = treeNode.Nodes.Add(di.Name);
                tds.StateImageIndex = 0;
                tds.Tag = di.FullName;
                LoadFiles(subDirectory, tds, progressBar);
                LoadSubDirectories(subDirectory, tds, progressBar);
                UpdateProgressBar(progressBar);
            }
        }

        public void LoadFiles(string dir, TreeNode treeNode, ProgressBar progressBar)
        {
            var Files = Directory.GetFiles(dir, "*.txt*");
            //loop through them to see files
            foreach (var file in Files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tds = treeNode.Nodes.Add(fi.Name);
                tds.Tag = fi.FullName;
                tds.StateImageIndex = 1;
                UpdateProgressBar(progressBar);
            } 
        }
        public void UpdateProgressBar(ProgressBar progressBar1)
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
                int percent = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
                progressBar1.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressBar1.Width / 2 - 10, progressBar1.Height / 2 - 7));

                Application.DoEvents();
            }
        }
    }
}
