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
        public Form1()
        {
            InitializeComponent();
            init.InitializeLinkListsForFile(this.panel1);
            ControlInitializer cc = new ControlInitializer();
            //this.textReader.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           var SelectedFolderObject =  init.PopulateFolderView();
            init.SaveFoldernames(SelectedFolderObject);
            init.InitializeLinkListsForFile(this.panel1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var FoldersToSearch = init.GetSavedFoldersFromFile(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json");
            if (FoldersToSearch.Count > 0)
            {
                if (Directory.Exists(FoldersToSearch[0].FolderName))
                {
                   init.SearchWordFromSavedFiles(this.textReader, this.textBox1, FoldersToSearch, this.tableLayoutPanel2, this);                    
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void linkFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkToOpen = e.Link.LinkData as string;
            var arrayOfStringToUser = linkToOpen.Split('>');
            int index = 0;

            if (File.Exists(arrayOfStringToUser[0]))
            {
                var allText = File.ReadAllText(arrayOfStringToUser[0]);
               // ControlInitializer ControlInit = new ControlInitializer();
               this.textReader.Text = allText;
              //  ControlInit.ViewSearchedResult(arrayOfStringToUser[0], this.textReader, arrayOfStringToUser[1]);
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
                //this is added when a match is found for the search
                index = textReader.Text.IndexOf(arrayOfStringToUser[1], index) + 1;
                //firstWordFound = textReader.Text.IndexOf(arrayOfStringToUser[1], index);
            } while (index < textReader.Text.LastIndexOf(arrayOfStringToUser[1]));
            Console.WriteLine(textReader.Text);
            
        }
  
    }
}
