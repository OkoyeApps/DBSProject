using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OssigweAssignment
{
    class UnusedCode
    {
        //This is used for dynamic object binding
        //dynamic FolderToSave = new ExpandoObject();
        //dynamic FilesToSave = new List<dynamic>();
        //FolderToSave.No = folder.No;
        //FolderToSave.FolderName = folder.FolderName;
        //FolderToSave.NoOfFiles = folder.NoOfFiles;
        //FolderToSave.RootDirectory = folder.RootDirectory;
        //FolderToSave.TimeOFCreation = folder.TimeFound;
        //FolderToSave.NoOfTxtFiles = folder.NoOfTxtFiles;
        //folder.Files.ForEach(X => FilesToSave.Add(X));
        //FolderToSave.Files = FilesToSave;
        //string json = JsonConvert.SerializeObject(FolderToSave,Formatting.Indented);

        /*
         *         public  void RemoveRow(TableLayoutPanel panel, int rowIndex)
        {
            panel.RowStyles.RemoveAt(rowIndex);

            for (int columnIndex = 0; columnIndex < panel.ColumnCount; columnIndex++)
            {
                var control = panel.GetControlFromPosition(columnIndex, rowIndex);
                panel.Controls.Remove(control);
            }

            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int columnIndex = 0; columnIndex < panel.ColumnCount; columnIndex++)
                {
                    var control = panel.GetControlFromPosition(columnIndex, i);
                   if (control != null) panel.SetRow(control, i - 1);
                }
            }

            panel.RowCount--;
        }
         */
    }
}
