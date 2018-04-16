using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Security.AccessControl;

namespace OssigweAssignment
{
    public partial class Form1 : Form
    {
        Initialization init = new Initialization(new Folder(), new Files(), new JsonSerializer());
        const string pathForSaveFolder = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json";
        const string pathForSaveFile = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\savedWords.json";
        const string PathForBinary = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\binary.bin";

        public Form1()
        {
            InitializeComponent();
            treeView1.Nodes.Clear();
            toolTip1.ShowAlways = true;
            progressBar1.Value = 0;
            init.InitializeLinkListsForFile(this.panel5, this.treeView1, this.progressBar1);
            ReportManager();
            //init.ReadHtmlpage(@"C:\Users\Emmanuel\Desktop\test.html");
            //this.progressBar1.Hide();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            var FoldersToSearch = init.GetSavedFoldersFromFile(pathForSaveFolder);
            if (FoldersToSearch != null)
            {
                if (Directory.Exists(FoldersToSearch[0].FolderName))
                {
                    var result = init.SearchWordFromSavedFiles(this.textReader, this.textBox1, FoldersToSearch, this);
                    if (result != null)
                    {
                        panel19.Controls.Clear();
                        panel16.Controls.Clear();
                        panel15.Controls.Clear();
                        SetTableHeaders();
                        panel16.Controls.AddRange(result.Item1.ToArray());
                        panel19.Controls.AddRange(result.Item2.ToArray());
                        panel15.Controls.AddRange(result.Item3.ToArray());
                    }
                    
                    return;
                }
            }
        }
        public void SetTableHeaders()
        {
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(740, 33);
            this.label7.TabIndex = 0;
            this.label7.Text = "Found Files";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            panel16.Controls.Add(label7);

            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(-1, -1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 34);
            this.label8.TabIndex = 0;
            this.label8.Text = "Count";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            panel19.Controls.Add(label8);

            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(-1, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 33);
            this.label9.TabIndex = 1;
            this.panel15.Controls.Add(label9);

        }

        public void Clicked_EVentHandler(object sender, EventArgs e)
        {
            //this listens for the click event of the lik label that shows the view for searches
            Console.WriteLine("clicked");
        }

        public void linkFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkToOpen = e.Link.LinkData as string;
            var arrayOfStringToUser = linkToOpen.Split('>');
            int index = 0;
            int FirstWordLocation = 0;
            SearchedWord SearchWordItem = new SearchedWord();
            ReportClass Report = new ReportClass();
      
            if (File.Exists(arrayOfStringToUser[0]))
            {
               var allText = File.ReadAllText(arrayOfStringToUser[0]);
               this.textReader.Text = allText;
            }
            String temp = textReader.Text;
            textReader.Text = "";
            textReader.Text = temp;
            bool matchfound = false;
            do
            {
                //Searches the text in the files
              var result =   textReader.Find(arrayOfStringToUser[1], index, textReader.TextLength, RichTextBoxFinds.None);
                //this just adds color to the word
                textReader.SelectionBackColor = Color.Yellow;
                if (index == 0)
                {
                    //this is added when a match is found for the search
                    FirstWordLocation = result;
                    //textReader.Text.IndexOf(arrayOfStringToUser[1], index);
                }
                //this increaments the index to continue searching for next value
                index = textReader.Text.IndexOf(arrayOfStringToUser[1], index) + 1;

            } while (index < textReader.Text.LastIndexOf(arrayOfStringToUser[1]));
            
            //this points the cursor to the position of the found word
            textReader.SelectionStart = FirstWordLocation;
            tabControl2.SelectTab(1);

            //this is used for the reporting side bar
            label13.Text = arrayOfStringToUser[1];
            ReportManager();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            //this responds to the click event of the AddFolderToMonitor button
            treeView1.Nodes.Clear();
            toolTip1.ShowAlways = true;
            progressBar1.Value = 0;
            var SelectedFolderObject = init.PopulateFolderView();
            if (SelectedFolderObject != null)
            {
            init.SaveFoldernames(SelectedFolderObject);
            }
            init.InitializeLinkListsForFile(this.panel5, this.treeView1, this.progressBar1);
            ReportManager();
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the node at the current mouse pointer location.  
            TreeNode theNode = this.treeView1.GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.  
            if (theNode != null && theNode.Tag != null)
            {
                // Change the ToolTip only if the pointer moved to a new node.  
                if (theNode.Tag.ToString() != this.toolTip1.GetToolTip(this.treeView1))
                    this.toolTip1.SetToolTip(this.treeView1, theNode.Tag.ToString());

            }
            // Pointer is not over a node so clear the ToolTip.
            else
            {
                this.toolTip1.SetToolTip(this.treeView1, "");
            }
        }
        private void RemoveTreeNode()
        {
            var fileName = this.treeView1.Nodes.Count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveCheckedNodes(this.treeView1.Nodes);
        }
        List<TreeNode> CheckedNodes = new List<TreeNode>();
        List<string> FolderNamesToRemove = new List<string>();
        void RemoveCheckedNodes(TreeNodeCollection nodes)
        {
            bool isSubFolder = false;
            bool isFolder = false;
            //DirectorySecurity ds = new DirectorySecurity();
            //var fs = new FileSystemAccessRule();
            foreach (TreeNode node in nodes)
            {
                if (Directory.Exists(node.Tag.ToString()))
                {
                    if (node.Checked)
                    {
                        CheckedNodes.Add(node);
                        isFolder = false;
                    }
                    else
                    {
                        Console.WriteLine(node.Tag);
                        RecursiveNodeRemoval(node.Nodes);
                        isSubFolder = true;
                    }

                }
            }
            //if (isSubFolder)
            //{
            //    MessageBox.Show("Sorry you cannot select a sub-folder for Removal. \n This is because the system monitors the root folder and with its sub-folders. \n To delete Sub-folder you have to delete the parent folder");
            //}
            if (CheckedNodes.Count > 0)
            {
            foreach (TreeNode nodeToRemove in CheckedNodes)
            {
                var folderName = nodeToRemove.Tag.ToString();
                if (Directory.Exists(folderName))
                {
                    FolderNamesToRemove.Add(folderName);
                    nodes.Remove(nodeToRemove);
                }
            }
                if (FolderNamesToRemove.Count > 0)
                {
                    if (File.Exists(pathForSaveFolder))
                    {
                        var alltextRead = File.ReadAllText(pathForSaveFolder);
                        var allFolderInFile = JsonConvert.DeserializeObject<List<Folder>>(alltextRead);
                        foreach (var item in FolderNamesToRemove)
                        {
                            var FolderToRemove = allFolderInFile.Where(x => x.FolderName == item).FirstOrDefault();
                            if (FolderToRemove != null)
                            {
                                allFolderInFile.Remove(FolderToRemove);

                            }
                        }
                        var newFoldersToAdd = JsonConvert.SerializeObject(allFolderInFile);
                        File.WriteAllText(pathForSaveFolder, newFoldersToAdd);
                        using (BinaryWriter BW = new BinaryWriter(File.Open(PathForBinary, FileMode.OpenOrCreate)))
                        {
                            BW.Write(newFoldersToAdd);
                            BW.Close();
                        }
                    }
                }
            }
            ReportManager();
        }
        //This method was used to recursively check throug child Nodes if it is been checked.
       void RecursiveNodeRemoval(TreeNodeCollection nodes)
        {
            foreach (TreeNode item in nodes)
            {
                if (!item.Tag.ToString().EndsWith(".txt") && !item.Tag.ToString().EndsWith(".xml"))
                {
                    if (item.Checked)
                    {
                        //item.Checked = false;
                        CheckedNodes.Add(item);
                    }
                    else
                    {
                        RecursiveNodeRemoval(item.Nodes);
                    }
                }
            }
            return;
        }

        void ReportManager()
        {
            listBox1.Items.Clear();
            string AllIndexedWord = "";
            long TotalIndexedWord = 0;
            List<Folder> ListOfFolders = new List<Folder>();
            if (File.Exists(pathForSaveFolder))
            {
                 AllIndexedWord = File.ReadAllText(pathForSaveFolder);
                 ListOfFolders = JsonConvert.DeserializeObject<List<Folder>>(AllIndexedWord);
                if (ListOfFolders != null)
                {
                 TotalIndexedWord = ListOfFolders.Sum(x => x.Files.Sum(y => y.FileLength));
                }
            }
            if (File.Exists(pathForSaveFile))
            {
                var allSearchText = File.ReadAllText(pathForSaveFile);
                
               
                //var aa = ListOfFolders[0].Files;
                //var bb = aa.Sum(x => x.FileLength);
                var ListOfSearchedWords = JsonConvert.DeserializeObject<List<SearchedWord>>(allSearchText);
                var mostSearchedOrder = ListOfSearchedWords.OrderByDescending(x => x.NoOfSearchedTime).ToArray();
                var highestWordOder = ListOfSearchedWords.OrderByDescending(x => x.Word.Length).ToArray();
                label14.Text = label22.Text =  mostSearchedOrder[0].Word;
                label16.Text = label20.Text =  highestWordOder[0].Word;
                label15.Text = label21.Text= highestWordOder[highestWordOder.Length - 1].Word;
                
                label12.Text = label18.Text =  TotalIndexedWord.ToString();
                int count = 1;
                foreach (var item in mostSearchedOrder.OrderByDescending(X=>X.searchedTime))
                {
                    if (count <26)
                    {
                        string valueToAdd = $"{count}- Word: {item.Word}, \n Time- {item.searchedTime.ToLocalTime()}";
                        listBox1.Items.Add(valueToAdd);
                    }
                    else
                    {
                        return;
                    }
                    count++;                
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Path = textBox2.Text;
            if (!string.IsNullOrEmpty(Path))
            {
              var result =  init.GetAndReadHtmlPage(Path);
                if (result.Item1 != null)
                {
                        listBox2.DataSource = result.Item1;  
                }
                if (result.Item2 != null)
                {
                        listBox4.DataSource = result.Item2;
                }
                if (result.Item3 != null)
                {
                        listBox3.DataSource = result.Item3;
                }

            }
        }
    }
}
