using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public class Initialization
    {
        const string pathForTempFile = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json";
        const string pathForSaveFile = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\savedWords.json";
        const string PathForBinary = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\binary.bin";
        DirectoryInfo DirectoryInfo;
        Folder folder;
        Files file;
        List<Folder> FoldersFromFile;
        List<SearchedWord> SearchedWordFromFile;
        JsonSerializer serializer;
        LinkLabel linkFolder;
        delegate void LinkLabelClickedEventHandler(object sender, EventArgs e);
       
        Tuple<TableLayoutPanel, List<Control>, List<int>, List<int>> ControlElementsToAdd;
        List<int> columnsToAdd;
        List<int> rowsToAdd;
        List<Control> ControlsToAdd;
        public Initialization(Folder FParam, Files FLParam, JsonSerializer JsParam)
        {
            folder = FParam;
            file = FLParam;
            serializer = JsParam;
            FoldersFromFile = new List<Folder>();
        }

        public FolderBrowserDialog PopulateFolderView()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog()
            {
                Description = "Select a folder to monitor"
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                var TempFolderName = folderBrowser.SelectedPath;
                var FolderName = Path.GetDirectoryName(TempFolderName);
                var labelText = TempFolderName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var text = labelText.LastOrDefault();
                var FolderFullPath = Path.GetFullPath(TempFolderName);

                return folderBrowser;
            }
            return null;
        }
        public void SaveFoldernames(FolderBrowserDialog folderDialog)
        {
            if (folderDialog == null)
                return;
            if (string.IsNullOrEmpty(folderDialog.SelectedPath))
                return;

            var TempFolderName = folderDialog.SelectedPath;
            var FolderName = Path.GetDirectoryName(TempFolderName);
            var labelText = TempFolderName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            var text = labelText.LastOrDefault();
            var FolderFullPath = Path.GetFullPath(TempFolderName);

            folder = new Folder();

            DirectoryInfo = new DirectoryInfo(FolderFullPath); //This code is used to the current selected directory
            var allFIlesInDirectory = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            var allSubDirectories = DirectoryInfo.EnumerateDirectories("*", SearchOption.AllDirectories);
            int counter = 1;
            if (DirectoryInfo.Exists)
            {
                folder.FolderName = DirectoryInfo.FullName;
                folder.NoOfFiles = allFIlesInDirectory.Count();
                folder.RootDirectory = DirectoryInfo.Root.ToString();
                folder.Created = DirectoryInfo.CreationTime.Date.ToLocalTime();
                folder.No = counter;
                folder.NoOfTxtFiles = allFIlesInDirectory.Where(x => x.Extension == ".txt").Count();
                folder.TimeFound = DateTime.Now.Date.ToLocalTime();
                folder.NoOfSubDirectories = allSubDirectories.Count();
                folder.Name = text;
                foreach (var item in allFIlesInDirectory)
                {
                    if (item.Extension == ".txt" || item.Extension == ".xml")
                    {
                        file = new Files
                        {
                            FileExtension = item.Extension,
                            FileLength = item.Length,
                            FileName = item.Name,
                            TimeFound = item.CreationTime.Date,
                            No = counter,
                            CurrentDirectory = item.DirectoryName,
                            RootFolder = item.Directory.FullName,
                            FileFullName = item.FullName
                        };
                        folder.Files.Add(file);
                    }
                        counter++;

                }
            }

            List<Folder> FolderToAddToJson = new List<Folder>();
            if (!File.Exists(pathForTempFile))
            {
                FolderToAddToJson.Add(folder);
                using (StreamWriter file = File.CreateText(pathForTempFile))
                {
                    //serialize object directly into file stream
                    serializer = new JsonSerializer()
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, FolderToAddToJson);
                    ///Know that i made this two files for you to understand what is been formatted.
                    ///At first we create the json file, which is what we all can read. then we create the binary file
                    ///with the binarywriter from the above method.
                    ///we then read back the file we created with this method.
                    ///we first read the json which we know the result and then we read the binary which we are expecting the same result as the json result
                    ///we then deserialize it again and write to a temporary file inother to cross check our result.
                    var TextToWrite = JsonConvert.SerializeObject(FolderToAddToJson);
                    BinaryWriter BW = new BinaryWriter(File.Open(PathForBinary, FileMode.CreateNew));
                    
                    BW.Write(TextToWrite);
                    BW.Close();
                    return;
                }
            }
            else if (File.Exists(pathForTempFile))
            {
                var currentFoldersInFile = GetSavedFoldersFromFile(pathForTempFile);
                if (currentFoldersInFile.Count > 0)
                {
                    string UpdatedFileToWrite = "";
                    var isPathAlreadyInMonitored = currentFoldersInFile.Any(x => x.FolderName == DirectoryInfo.FullName || x.Files.Any(y => y.RootFolder == DirectoryInfo.FullName));
                    var foldercount = currentFoldersInFile.LastOrDefault().No;
                    if (!isPathAlreadyInMonitored)
                    {
                        folder.No = foldercount + 1;
                        currentFoldersInFile.Add(folder);      
                    }
                    else if(isPathAlreadyInMonitored)
                    {
                        var newChangedFile = currentFoldersInFile.Where(x => x.FolderName == DirectoryInfo.FullName && x.NoOfSubDirectories != folder.NoOfSubDirectories || x.NoOfTxtFiles != folder.NoOfTxtFiles).FirstOrDefault();
                        if (newChangedFile != null)
                        {
                            newChangedFile.NoOfFiles = folder.NoOfFiles;
                            newChangedFile.NoOfSubDirectories = folder.NoOfSubDirectories;
                            newChangedFile.Files = folder.Files;
                            newChangedFile.NoOfTxtFiles = folder.NoOfTxtFiles;
                        }           
                    }
                    UpdatedFileToWrite = JsonConvert.SerializeObject(currentFoldersInFile);
                    File.WriteAllText(pathForTempFile, UpdatedFileToWrite);
                    using(BinaryWriter BW = new BinaryWriter(File.Open(PathForBinary, FileMode.CreateNew)))
                    {
                        BW.Write(UpdatedFileToWrite);
                        BW.Close();
                    }
                }
            }
        }
        public List<Folder> GetSavedFoldersFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            serializer = new JsonSerializer()
            {
                Formatting = Formatting.Indented
            };
            FoldersFromFile.Clear();
            //This reads the Json file
            var AllText = File.ReadAllText(path);
            //This reads the binary File
            using (BinaryReader BR = new BinaryReader(File.Open(PathForBinary, FileMode.Open)))
            {
                var newFileFromBinary = BR.ReadString();
                var newJsonObject = JsonConvert.DeserializeObject<List<Folder>>(newFileFromBinary);
                var jsonToAddAgainForTest = JsonConvert.SerializeObject(newJsonObject);
                File.WriteAllText(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\temp.json", jsonToAddAgainForTest);
            };
            JArray jsonObject = JArray.Parse(AllText);
            
            foreach (var item in jsonObject.ToList())
            {
                var result = item.ToObject<Folder>(serializer);
                FoldersFromFile.Add(result);
            }
            return FoldersFromFile;
        }

        public void InitializeLinkListsForFile(Panel panel1)
        {
            if (!File.Exists(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json"))
            {
                return;
            }
            var Folders = GetSavedFoldersFromFile(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json");
            List<Control> ControlsToAdd = new List<Control>();
            StringBuilder builder = new StringBuilder();
            if (Folders.Count > 0)
            {
                int yAxis = 38;
                int counter = 3;
                int FolderCounter = 0;

                List<string> rootFolder = new List<string>();
                foreach (var folder in Folders)
                {
                    if (!rootFolder.Exists(x => x.ToString() == folder.FolderName && FolderCounter <= 10))
                    {
                        rootFolder.Add(folder.FolderName);
                        linkFolder = new LinkLabel()
                        {
                            ActiveLinkColor = System.Drawing.Color.Black,
                            Text = folder.Name,
                            Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                            BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                            LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline,
                            LinkColor = System.Drawing.Color.Black,
                            Name = "linkFolder",
                            Size = new System.Drawing.Size(257, 38),
                            TabIndex = counter,
                            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                            TabStop = true,
                            Cursor = Cursors.Hand,
                            Location = new Point(2, yAxis),
                            LinkArea = new LinkArea(0, 100)
                        };
                        linkFolder.Links[0].LinkData = folder.FolderName;
                        //linkFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(controlInit.linkFolder_LinkClicked);
                        ControlsToAdd.Add(linkFolder);

                        if (linkFolder.Text.Length > 50)
                        {
                            linkFolder.Text.Remove(50, (linkFolder.Text.Length - 50));
                            builder.Append(linkFolder.Text);
                            builder.Remove(50, (linkFolder.Text.Length - 50));
                            builder.Append("...");
                            linkFolder.Text = builder.ToString().ToUpper();
                            builder.Clear();
                        }

                        yAxis += 50;
                        counter++;
                        FolderCounter++;
                    }
                }
            }
            if (ControlsToAdd.Count > 0)
            {
                panel1.Controls.AddRange(ControlsToAdd.ToArray());
            }
        }

        public void SearchWordFromSavedFiles(RichTextBox richTextBox, TextBox textBox, List<Folder> folder, TableLayoutPanel TPanel, Form1 form)
        {
            string[] BlackList = new string[] { "is", "a", "the", "of", "this", "how", "all", "ago", "I", "me", "he", "he’ll", "he’s", "she", "she’ll", "she’s", "am", "in", "so", "is", "be", "let", "mr", "mrs", "we", "us", "you", "oh", "ok", "ex" };
            if (textBox.Text.Length > 1 && !BlackList.Contains(textBox.Text))
            {
                ControlInitializer controlInit = new ControlInitializer();
                richTextBox.Text = null;
                string firstLineofWord = "";
                int totalOccurence = 0;
                int columnCount = 2;
                int rowCount = 2;
                int tab = 7;
                int LineNoForFirstMatch = 0;
                List<string> filePathToSearch = new List<string>();
                List<int> totalOCcurenceForSavedFile = new List<int>();
                var foldersToSearch = folder.Where(x => x.NoOfTxtFiles > 0).ToList();
                List<string> foundWordAndFolder = new List<string>();
                bool matchFound = false;
                ControlsToAdd = new List<Control>();
                rowsToAdd = new List<int>();
                columnsToAdd = new List<int>();
                //this part instantiates the files to search by removing all files that does not end with a .txt
                if (foldersToSearch.Count > 0)
                {
                    var FilesToSearch = foldersToSearch.Select(x => x.Files).ToList();
                    int counter = 0;
                    for (int i = 0; i < FilesToSearch[0].Count; i++)
                    {
                        if (FilesToSearch[0][i].FileExtension == ".txt")
                        {
                            filePathToSearch.Add(FilesToSearch[0][i].FileFullName);
                        }
                        counter++;
                    }
                }
                foreach (var currentFolderToSearch in filePathToSearch)
                {
                    if (File.Exists(currentFolderToSearch))
                    {

                        //this just reads the lines one after the other to search line by line at first
                            var allTextRead = File.ReadLines(currentFolderToSearch).ToList();
                            var FullText = File.ReadAllText(currentFolderToSearch);
                           
                            int loopBreaker = 0;
                            do
                            {
                                for (int stringItem = 0; stringItem < allTextRead.Count; stringItem++)
                                {
                                    var match = string.Compare(allTextRead[stringItem], textBox.Text, true);
                                    var wordsInLine = allTextRead[stringItem].Split(new char[] { ' ', '.' });

                                    foreach (var word in wordsInLine.ToList())
                                    {
                                        if (word.ToLower().Contains(textBox.Text.ToLower()))
                                        {

                                            totalOccurence++;
                                            if (matchFound == false)
                                            {
                                                firstLineofWord = allTextRead[stringItem];
                                                Label label2 = (Label)controlInit.labelForTable(tab, allTextRead[stringItem], labelType.col2, form);
                                                LinkLabel linkLabel = (LinkLabel)controlInit.labelForTable(tab + 1, allTextRead[stringItem], labelType.col4, form);
                                                LineNoForFirstMatch = stringItem;
                                                matchFound = true;
                                                foundWordAndFolder.Add(currentFolderToSearch);
                                                linkLabel.Links[0].LinkData = currentFolderToSearch + ">"+textBox.Text;
                                               

                                            ControlsToAdd.Add(linkLabel);
                                            columnsToAdd.Add(1);
                                            rowsToAdd.Add(rowCount);
                                            //Notice this was added twice because we are creating two different elements and they both need a row and a column
                                            ControlsToAdd.Add(label2);
                                            columnsToAdd.Add(0);
                                            rowsToAdd.Add(rowCount);


                                        }
                                        }
                                        loopBreaker = stringItem;
                                    }
                                }
                            } while (matchFound == false && loopBreaker > allTextRead.Count);
               
                        if (!string.IsNullOrWhiteSpace(richTextBox.Text))
                        {
                            //MessageBox.Show($"No match Found");
                        }
                    }
                    if (matchFound)
                    {
                        totalOCcurenceForSavedFile.Add(totalOccurence);
                        var label = controlInit.labelForTable(tab, totalOccurence.ToString(), labelType.col3,form);
                        ControlsToAdd.Add(label);
                        rowsToAdd.Add(rowCount);
                        columnsToAdd.Add(2);

                        SaveSearchedFile(foundWordAndFolder, totalOCcurenceForSavedFile.ToArray(), textBox.Text);
                        columnCount++; rowCount++;
                    }
                    matchFound = false;
                    totalOccurence = 0;
                    
                }

                controlInit.AddTableLayoutControl(ControlsToAdd, TPanel, columnsToAdd, rowsToAdd, form);
                textBox.Text = ""; //this just makes the textbox blank again...
            }
            else
            {
                if (textBox.Text.Length == 1)
                {
                    MessageBox.Show("Sorry you can't search for a one letter one");
                    return;
                }
                MessageBox.Show("Sorry this word is among the blacklist");
                return;
            }
        }


        public void AddControlToTable(TableLayoutPanel tPanel, int col, int row, Control control)
        {
            tPanel.Controls.Add(control, col, row);
        }
        public List<SearchedWord> SaveSearchedFile(List<string> searchedWordAndFolder,int[] occurence, string word)
        {
            SearchedWord SW = null;
            SearchedWordFromFile = null;
            searchedFile Sf = null;
            List<searchedFile> SWordListItem = new List<searchedFile>();
            List<SearchedWord> searchedWordListItem = new List<SearchedWord>();
            if (searchedWordAndFolder != null)
            {
                for (int i = 0; i < searchedWordAndFolder.Count; i++)
                {
                        Sf = new searchedFile()
                        {
                            folderLocation = searchedWordAndFolder[i],
                            wordOccurrenceInFile = occurence[i]
                        };
                    SWordListItem.Add(Sf);
                }
                SW = new SearchedWord()
                {
                    Id = 1,
                    NoOfSearchedTime = 1,
                    searched = DateTime.Now,
                    Word = word
                };
                SW.searchedFolders= SWordListItem;
                searchedWordListItem.Add(SW);
            }
            //This part saves the word as json in a file
                serializer = new JsonSerializer()
                {
                    Formatting = Formatting.Indented
                };
                string AllSavedFileText = "";

            if (File.Exists(pathForSaveFile))
            {
                AllSavedFileText = File.ReadAllText(pathForSaveFile);
               
                var result = JsonConvert.DeserializeObject<List<SearchedWord>>(AllSavedFileText);
                var previousSearch = result.Where(x => x.Word.ToLower() == word.ToLower()).FirstOrDefault();
                if (previousSearch != null)
                {
                    previousSearch.NoOfSearchedTime = previousSearch.NoOfSearchedTime + 1;
                    previousSearch.searchedFolders = SW.searchedFolders;
                    previousSearch.searched = DateTime.Now;
                    File.WriteAllText(pathForSaveFile, JsonConvert.SerializeObject(result));
                    return result;
                }
                else if (previousSearch == null)
                {
                    result.ForEach(x => searchedWordListItem.Add(x));

                    var resultToSave = JsonConvert.SerializeObject(searchedWordListItem);
                    File.WriteAllText(pathForSaveFile, resultToSave);
                }
            }
            else
            {
                AllSavedFileText = JsonConvert.SerializeObject(searchedWordListItem);
                File.WriteAllText(pathForSaveFile, AllSavedFileText);
            }
            return SearchedWordFromFile;
        }
    }
}
