﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OssigweAssignment
{

    public class Initialization
    {
        const string pathForSavedFolder = @"C:\Users\Emmanuel\Desktop\TestFolderForFiles\Json.json";
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


        List<Control> ControlsToAdd;
        List<Control> FoundItems;
        List<Control> CountFound;
        List<Control> ViewFound;
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
        public List<Folder> InitializeFoldersToSave(List<string> selectedPaths)
        {
            if (selectedPaths == null)
                return null;
            List<Folder> ListOfFoldersToAdd = new List<Folder>();

            foreach (var path in selectedPaths)
            {
                var TempFolderName = path;
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
                    ListOfFoldersToAdd.Add(folder);
                }
            }
            return ListOfFoldersToAdd;
        }
        //Use this method o update the file changes later tomorrow
        public void SaveFoldernames(FolderBrowserDialog folderDialog = null, List<string> SelectedPaths = null)
        {
            List<Folder> FolderToAddToJson = new List<Folder>();
            bool newFolderReturned = false;
            Dictionary<string, long> FileAndLength = new Dictionary<string, long>();
            long totalLengthOfFilesInDirectory = 0;
            if (folderDialog != null)
            {
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
                    totalLengthOfFilesInDirectory = allFIlesInDirectory.Sum(x => x.Length);
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
                        FileAndLength.Add(item.FullName, item.Length);
                        counter++;

                    }
                    FolderToAddToJson.Add(folder);
                }
            }
            if (SelectedPaths != null)
            {
                FolderToAddToJson = InitializeFoldersToSave(SelectedPaths);
                newFolderReturned = true;
            }
            if (!File.Exists(pathForSavedFolder))
            {

                using (StreamWriter file = File.CreateText(pathForSavedFolder))
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
                    BinaryWriter BW = new BinaryWriter(File.Open(PathForBinary, FileMode.OpenOrCreate));

                    BW.Write(TextToWrite);
                    BW.Close();
                    return;
                }
            }
            else if (File.Exists(pathForSavedFolder))
            {
                string UpdatedFileToWrite = "";
                var currentFoldersInFile = GetSavedFoldersFromFile(pathForSavedFolder);
                List<Files> currentFolderFiled = new List<Files>();
                if (currentFoldersInFile != null && currentFoldersInFile.Count > 0)
                {
                    foreach (var item in currentFoldersInFile)
                    {
                        item.Files.ForEach(x => currentFolderFiled.Add(x));
                    }
                    foreach (var item in currentFolderFiled)
                    {
                        FileAndLength.Add(item.FileFullName, item.FileLength);
                    }
                }
                if (currentFoldersInFile != null && currentFoldersInFile.Count > 0)
                {
                    var isPathAlreadyInMonitored = currentFoldersInFile.Any(x => x.FolderName == DirectoryInfo.FullName || x.Files.Any(y => y.RootFolder == DirectoryInfo.FullName));
                    var foldercount = currentFoldersInFile.LastOrDefault().No;
                    if (!isPathAlreadyInMonitored)
                    {
                        folder.No = foldercount + 1;
                        currentFoldersInFile.Add(folder);
                    }
                    else if (isPathAlreadyInMonitored)
                    {
                        Folder newChangedFile = new Folder();
                        var allFileLength = currentFoldersInFile.Sum(X => X.Files.Sum(x => x.FileLength));
                        long currentFileLength = 0;

                        ///this foreach actually was used to make sure that if any file is changed then the indexed words changes accordingly.
                        ///if might be a little bit confusing but the logic is this
                        ///when a file changes either by increasing or decreasing, then the file length changes also.
                        ///so we check if the file length that we indexed and the file length from the system is same
                        ///if they are not, then we we get that folder and update it with the current files.
                        ///this is done so as to always keep track of files since we are not using a windows service to update or indexes.

                        foreach (var item in currentFoldersInFile)
                        {
                            var file = item.Files;
                            foreach (var item2 in file)
                            {
                                if (FileAndLength.ContainsKey(item2.FileFullName))
                                {
                                    var currentFile = FileAndLength[item2.FileFullName];
                                    if (currentFile != item2.FileLength)
                                    {
                                        currentFileLength = currentFile;
                                    }
                                    DirectoryInfo Di = new DirectoryInfo(item.FolderName);
                                    var filesinSystemDirectory = Di.EnumerateFiles("*.txt*", SearchOption.AllDirectories);
                                    var FIleToCheck = filesinSystemDirectory.Where(x => x.FullName == item2.FileFullName).FirstOrDefault();
                                    if (FIleToCheck.Length != item2.FileLength)
                                    {
                                        newChangedFile = item;
                                        break;
                                    }
                                    else
                                    {
                                        newChangedFile = currentFoldersInFile.Where(x => x.FolderName == DirectoryInfo.FullName && x.NoOfSubDirectories != folder.NoOfSubDirectories || x.NoOfTxtFiles != folder.NoOfTxtFiles).FirstOrDefault();
                                    }
                                }
                            }
                        }
                        if (newChangedFile != null)
                        {
                            if (folder != null)
                            {
                                newChangedFile.NoOfFiles = folder.NoOfFiles;
                                newChangedFile.NoOfSubDirectories = folder.NoOfSubDirectories;
                                newChangedFile.Files = folder.Files;
                                newChangedFile.NoOfTxtFiles = folder.NoOfTxtFiles;
                            }
                            else
                            {
                                FolderBrowserDialog ff = new FolderBrowserDialog();
                                ff.SelectedPath = newChangedFile.FolderName;
                                SaveFoldernames(ff);
                            }
                        }
                        else if (newFolderReturned == true)
                        {
                            currentFoldersInFile = FolderToAddToJson;
                        }
                    }
                }
                if (currentFoldersInFile != null && currentFoldersInFile.Count >0 )
                {
                    if (currentFoldersInFile.Count > 0)
                    {
                        UpdatedFileToWrite = JsonConvert.SerializeObject(currentFoldersInFile);
                    }
                }
                else
                {
                    UpdatedFileToWrite = JsonConvert.SerializeObject(FolderToAddToJson);
                }
                using (StreamWriter SW = File.CreateText(pathForSavedFolder))
                {
                    SW.WriteLine(UpdatedFileToWrite);
                    SW.Close();
                }
                //File.WriteAllText(pathForSavedFolder, UpdatedFileToWrite);

                using (BinaryWriter BW = new BinaryWriter(File.Open(PathForBinary, FileMode.OpenOrCreate)))
                {
                    BW.Write(UpdatedFileToWrite);
                    BW.Close();
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
            string AllText = "";
            //var AllText = File.ReadAllText(path);
            using (StreamReader SR = new StreamReader(File.OpenRead(path)))
            {
                AllText = SR.ReadToEnd();
                SR.Close();
            }
            //This reads the binary File
            using (BinaryReader BR = new BinaryReader(File.Open(PathForBinary, FileMode.Open)))
            {
                var newFileFromBinary = BR.ReadString();
                var newJsonObject = JsonConvert.DeserializeObject<List<Folder>>(newFileFromBinary);
                var jsonToAddAgainForTest = JsonConvert.SerializeObject(newJsonObject);
                //For test purpose you can uncomment this line and pass a pass for the temporary file inorder to check if bin file is the same as the json generated file
                //File.WriteAllText(@"C:\Users\Emmanuel\Desktop\TestFolderForFiles\temp.json", jsonToAddAgainForTest);
                BR.Close();
            };
            var AllFoldersInFile = JsonConvert.DeserializeObject<List<Folder>>(AllText);
            ///Am trying to initiatiate file added and file removal


            return AllFoldersInFile;
        }

        public void InitializeLinkListsForFile(Panel panel1, TreeView treeView, ProgressBar progressBar)
        {
            if (!File.Exists(pathForSavedFolder))
            {
                return;
            }
            var Folders = GetSavedFoldersFromFile(pathForSavedFolder);

            if (Folders != null && Folders.Count > 0)
            {
                List<string> NameOfChangedfolders = new List<string>();

                List<string> rootFolder = new List<string>();

                foreach (var folder in Folders)
                {
                    DirectoryInfo Di = new DirectoryInfo(folder.FolderName);
                    //var AllFileCountInDirectory = Di.EnumerateFiles()
                    var CurentFolder = Directory.EnumerateDirectories(folder.FolderName, "*", SearchOption.AllDirectories);
                    var allFilesInCurrentDir = Di.EnumerateFiles("*", SearchOption.AllDirectories);
                    var allTextFileInFolder = allFilesInCurrentDir.Where(x => x.Extension.EndsWith(".txt") || x.Extension.EndsWith(".xml"));
                    var allFileinFolder = folder.Files;
                    var allFIleLengthCount = allFilesInCurrentDir.Where(x => x.Extension == ".txt" || x.Extension == ".xml").Sum(x => x.Length);
                    var bb = allFileinFolder.Sum(x => x.FileLength);
                    var lengthCountOfDirectoryInfo = allTextFileInFolder.Sum(x => x.Length);
                    if (CurentFolder.Count() != folder.NoOfSubDirectories)
                    {
                        NameOfChangedfolders.Add(folder.FolderName);
                    }
                    else if (allTextFileInFolder.Count() != allTextFileInFolder.Count() || lengthCountOfDirectoryInfo != allFileinFolder.Sum(x => x.FileLength))
                    {
                        NameOfChangedfolders.Add(folder.FolderName);
                    }
                    else
                    {
                        foreach (var item in allFilesInCurrentDir)
                        {
                            if (item.Extension.EndsWith(".txt") || item.Extension.EndsWith(".xml"))
                            {
                                var existingItem = folder.Files.Where(X => X.FileFullName == item.FullName).FirstOrDefault();
                                if (existingItem == null)
                                {
                                    NameOfChangedfolders.Add(folder.FolderName);
                                    break;
                                }
                            }

                        }
                    }
                }
                if (NameOfChangedfolders.Count > 0)
                {
                    SaveFoldernames(null, NameOfChangedfolders);
                    InitializeLinkListsForFile(panel1, treeView, progressBar);
                }
                treeView.Nodes.Clear();
                foreach (var folder in Folders)
                {
                    InitializeSearchedFolder init = new InitializeSearchedFolder();

                    init.LoadDirectory(folder.FolderName, treeView, progressBar);
                }

            }
        }

        public Tuple<List<Control>, List<Control>, List<Control>> SearchWordFromSavedFiles(RichTextBox richTextBox, TextBox textBox, List<Folder> folder, Form1 form)
        {
            string[] BlackList = new string[] { "is", "a", "the", "of", "this", "how", "all", "ago", "I", "me", "he", "he’ll", "he’s", "she", "she’ll", "she’s", "am", "in", "so", "is", "be", "let", "mr", "mrs", "we", "us", "you", "oh", "ok", "ex" };
            if (textBox.Text.Length > 1 && !BlackList.Contains(textBox.Text))
            {
                ControlInitializer controlInit = new ControlInitializer();
                richTextBox.Text = null;
                string firstLineofWord = "";
                int totalOccurence = 0;
                int yAxisForFile = 36;
                int yAxisForView = 36;

                int LineNoForFirstMatch = 0;
                List<string> filePathToSearch = new List<string>();
                List<int> totalOCcurenceForSavedFile = new List<int>();
                List<string> foundWordAndFolder = new List<string>();
                bool matchFound = false;
                ControlsToAdd = new List<Control>();
                ViewFound = new List<Control>();
                CountFound = new List<Control>();
                FoundItems = new List<Control>();

                //This first checks the binary file for already indexed words and locations first, before checking the root tree
                var foldersToSearch = folder.Where(x => x.NoOfTxtFiles > 0).ToList();
                if (File.Exists(pathForSaveFile))
                {
                    var FirstReadFileContainingFirstIndex = File.ReadAllText(pathForSaveFile);
                    var FilesToStartSearchingFrom = JsonConvert.DeserializeObject<List<SearchedWord>>(FirstReadFileContainingFirstIndex);
                    var WordAlreadyIndexedObject = FilesToStartSearchingFrom.Where(x => x.Word == textBox.Text).FirstOrDefault();
                    if (WordAlreadyIndexedObject != null)
                    {
                        WordAlreadyIndexedObject.searchedFolders.ForEach(x => filePathToSearch.Add(x.folderLocation));
                    }
                }
                //this part instantiates the files to search by removing all files that does not end with a .txt now form the folders
                if (foldersToSearch.Count > 0)
                {
                    ///---Suggestions
                    ///you can try making this code more optimized by also limiting the folders to go through first
                    ///by considering if the folder has been modified and if not if the previous search root folder includes it 
                    ///
                    ///---Suggestions
                    ///
                    List<Files> FilesToSearch = new List<Files>();
                    foldersToSearch.ForEach(X => FilesToSearch.AddRange(X.Files));
                    //var FilesToSearch = foldersToSearch.Select(x => new {file = x.Files);

                    //var mainFilesTosearch = FilesToSearch.Select(x => x.Select(y=>new { FileSearch = y }));
                    foreach (var item in FilesToSearch)
                    {
                        var isFolderInView = filePathToSearch.Any(x => x == item.FileFullName);

                        if (!isFolderInView)
                        {
                            filePathToSearch.Add(item.FileFullName);
                        }
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
                                    var isFound = string.Equals(textBox.Text, word, StringComparison.OrdinalIgnoreCase);
                                    if (word.ToLowerInvariant().Contains(textBox.Text.ToLower()))
                                    {
                                        totalOccurence++;
                                        if (matchFound == false)
                                        {
                                            firstLineofWord = allTextRead[stringItem];
                                            Label label2 = (Label)controlInit.CreateControlForSearchResult(yAxisForFile, currentFolderToSearch, labelType.col2);
                                            LinkLabel linkLabel = (LinkLabel)controlInit.CreateControlForSearchResult(yAxisForView, allTextRead[stringItem], labelType.col1, form);
                                            LineNoForFirstMatch = stringItem;
                                            matchFound = true;
                                            foundWordAndFolder.Add(currentFolderToSearch);
                                            linkLabel.Links[0].LinkData = currentFolderToSearch + ">" + textBox.Text;
                                            ViewFound.Add(linkLabel);
                                            FoundItems.Add(label2);
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
                        var label = controlInit.CreateControlForSearchResult(yAxisForFile, totalOccurence.ToString(), labelType.col3);
                        CountFound.Add(label);

                        yAxisForFile += 35; yAxisForView += 35;
                    }
                    matchFound = false;
                    totalOccurence = 0;

                };
                SaveSearchedFile(foundWordAndFolder, totalOCcurenceForSavedFile.ToArray(), textBox.Text);
                textBox.Text = ""; //this just makes the textbox blank again...
                return Tuple.Create(FoundItems, CountFound, ViewFound);
            }
            else
            {
                if (textBox.Text.Length == 1)
                {
                    MessageBox.Show("Sorry you can't search for a one letter one");
                    return null;
                }
                if (textBox.Text == "")
                {
                    MessageBox.Show("Please input a word for search");
                    return null;
                }
                MessageBox.Show("Sorry this word is among the blacklist");
                return null;
            }
        }


        public void AddControlToTable(TableLayoutPanel tPanel, int col, int row, Control control)
        {
            tPanel.Controls.Add(control, col, row);
        }
        public List<SearchedWord> SaveSearchedFile(List<string> searchedWordAndFolder, int[] occurence, string word)
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
                    searchedTime = DateTime.Now,
                    Word = word,
                    LastSearchedWord = word
                };
                SW.searchedFolders = SWordListItem;
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
                var previousSearch = result.Where(x => x.Word.ToLower() != null && x.Word.ToLower() == word.ToLower()).FirstOrDefault();
                if (previousSearch != null)
                {
                    previousSearch.NoOfSearchedTime = previousSearch.NoOfSearchedTime + 1;
                    previousSearch.searchedFolders = SW.searchedFolders;
                    previousSearch.searchedTime = DateTime.Now;
                    previousSearch.LastSearchedWord = word;

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

        public Tuple<List<string>, List<string>, List<string>> GetAndReadHtmlPage(string path)
        {
            List<string> AllTitle = new List<string>();
            List<string> AllScript = new List<string>();
            List<string> AllAnchor = new List<string>();
            string readFile = null;
            if (path.StartsWith("http://") || path.StartsWith("www"))
            {
                WebClient web = new WebClient();
                try
                {
                readFile = web.DownloadString(path);
                }
                catch(Exception ex)
                {

                }
            }
            if (File.Exists(path) && path.EndsWith(".html"))
            {
                 readFile = File.ReadAllText(path);
            }
            if (!string.IsNullOrEmpty(readFile))
            {
                MatchCollection allTitle = Regex.Matches(readFile, @"<title>\s*(.+?)\s*</title>", RegexOptions.Singleline);
                MatchCollection allScript = Regex.Matches(readFile, @"<script*(.+?)>\s*(.+?)\s*</script>", RegexOptions.Singleline);
                MatchCollection allATag = Regex.Matches(readFile, @"<a \s*(.+?)>\s*(.+?)\s*</a>", RegexOptions.Singleline);
                foreach (Match item in allTitle.AsParallel())
                {
                    string text = item.Groups[1].Value;
                    AllTitle.Add(text);
                    Console.WriteLine(text);
                }
                foreach (Match item in allScript.AsParallel())
                {                       

                    if (item.Groups[2].Value.Contains("<script"))
                    {
                        AllScript.Add(item.Groups[3].Value);
                        Console.WriteLine(item.Groups[3].Value);
                    }
                    else
                    {
                        AllScript.Add(item.Groups[2].Value);
                        Console.WriteLine(item.Groups[2].Value);
                    }
                }
                foreach (Match item in allATag.AsParallel())
                {
                    if (item.Groups[2].Value.Contains("<a"))
                    {
                        AllAnchor.Add(item.Groups[3].Value);
                        Console.WriteLine(item.Groups[3].Value);
                    }
                    else
                    {
                        AllAnchor.Add(item.Groups[2].Value);
                        Console.WriteLine(item.Groups[2].Value);
                    }
                }
            }
            return Tuple.Create(AllTitle, AllScript, AllAnchor);
        }
    }
}
