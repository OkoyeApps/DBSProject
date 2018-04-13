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
        const string pathForTempFile = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json";
        const string pathForSaveFile = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\savedWords.json";
        const string PathForBinary = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\binary.bin";
        private string allSearchTExt;

        public Form1()
        {
            InitializeComponent();
            init.InitializeLinkListsForFile(this.panel5);
            ControlInitializer cc = new ControlInitializer();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            var FoldersToSearch = init.GetSavedFoldersFromFile(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json");
            if (FoldersToSearch.Count > 0)
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
            Console.WriteLine("clicked");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Console.WriteLine(e.Link.LinkData);
        }

        public void linkFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkToOpen = e.Link.LinkData as string;
            var arrayOfStringToUser = linkToOpen.Split('>');
            int index = 0;
            int FirstWordLocation = 0;
            string searchedWord = "";
            string LastSearchedWord = "";
            string mostSearchedWord = "";
            string LeastSearchedWord = "";
            string LongestSearchedWord = "";
            string ShortestSearchedWord = "";
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
                    FirstWordLocation = textReader.Text.IndexOf(arrayOfStringToUser[1], index);
                }
                //this is added when a match is found for the search
                index = textReader.Text.IndexOf(arrayOfStringToUser[1], index) + 1;
                //firstWordFound = textReader.Text.IndexOf(arrayOfStringToUser[1], index);
            } while (index < textReader.Text.LastIndexOf(arrayOfStringToUser[1]));
            textReader.SelectionStart = FirstWordLocation;
            tabControl2.SelectTab(1);
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
            var SelectedFolderObject = init.PopulateFolderView();
            init.SaveFoldernames(SelectedFolderObject);
            init.InitializeLinkListsForFile(this.panel5);
        }
    }
}
