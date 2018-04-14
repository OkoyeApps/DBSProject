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
                    panel16.Controls.AddRange(result.Item1.ToArray());
                    panel19.Controls.AddRange(result.Item2.ToArray());
                    panel15.Controls.AddRange(result.Item3.ToArray());
                }
            }
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
            do
            {
                //Searches the text in the files
                textReader.Find(arrayOfStringToUser[1], index, textReader.TextLength, RichTextBoxFinds.None);
                //this just adds color to the 
                textReader.SelectionBackColor = Color.Yellow;
                if (index == 0)
                {
                    //this is added when a match is found for the search
                    FirstWordLocation = textReader.Text.IndexOf(arrayOfStringToUser[1], index);
                }
                //this increaments the index to continue searching for next value
                index = textReader.Text.IndexOf(arrayOfStringToUser[1], index) + 1;
            } while (index < textReader.Text.LastIndexOf(arrayOfStringToUser[1]));
            
            //this points the cursor to the position of the found word
            textReader.SelectionStart = FirstWordLocation;
            tabControl2.SelectTab(1);

            //this is used for the reporting side bar
            if (File.Exists(pathForSaveFile))
            {
                var allSearchText = File.ReadAllText(pathForSaveFile);
                var ListOfSearchedWords = JsonConvert.DeserializeObject<List<SearchedWord>>(allSearchText);
                var mostSearchedOrder =  ListOfSearchedWords.OrderByDescending(x => x.NoOfSearchedTime).ToArray();
                var highestWordOder = ListOfSearchedWords.OrderByDescending(x => x.Word.Length).ToArray();
                label14.Text = mostSearchedOrder[0].Word;
                label16.Text = highestWordOder[0].Word;
                label15.Text = highestWordOder[highestWordOder.Length -1].Word;
                label13.Text = arrayOfStringToUser[1];
            } 
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            //this responds to the click event of the AddFolderToMonitor button
            treeView1.Nodes.Clear();
            toolTip1.ShowAlways = true;
            progressBar1.Value = 0;
            var SelectedFolderObject = init.PopulateFolderView();
            init.SaveFoldernames(SelectedFolderObject);
            init.InitializeLinkListsForFile(this.panel5, this.treeView1, this.progressBar1);
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
    }
}
