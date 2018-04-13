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
    public enum labelType
    {
        col1 = 1, col2, col3, col4,col5
    }
    public class ControlInitializer
    {
        public Control labelForTable(int tab, string text, labelType Ltype, Form1 form)
        {
            Label label = null;
            LinkLabel linkLabel = null;
            //this is the label for the found files
            if (Ltype == labelType.col2)
            {
                label = new Label();
                label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label.Location = new System.Drawing.Point(60, 15);
                label.Name = "labelForTable";
                label.Size = new System.Drawing.Size(510, 13);
                label.TabIndex = 2;
                label.Text = text;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            else if (Ltype == labelType.col3)
            {
                label = new Label();
                label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                label.Location = new System.Drawing.Point(575, 15);
                label.Name = "label5";
                label.Size = new System.Drawing.Size(94, 13);
                label.TabIndex = 3;
                label.Text = text;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (Ltype == labelType.col4)
            {
                linkLabel = new LinkLabel();
                linkLabel.ActiveLinkColor = System.Drawing.Color.Maroon;
                linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                linkLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                linkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                linkLabel.LinkColor = System.Drawing.Color.Black;
                linkLabel.Location = new System.Drawing.Point(674, 50);
                linkLabel.Name = "linkLabel1";
                linkLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                linkLabel.Size = new System.Drawing.Size(97, 30);
                linkLabel.TabIndex = 0;
                linkLabel.TabStop = true;
                linkLabel.Text = "View";
                linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                
                linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(form.linkFolder_LinkClicked);
                linkLabel.Click += new System.EventHandler(form.Clicked_EVentHandler);
            }
            return label != null ? label : linkLabel;
        }
        public void Clicked_EVentHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked");
        }
        public void ViewSearchedResult(string path, RichTextBox richTextBox, string text)
        {
            int index = 0;
            String temp = richTextBox.Text;
            richTextBox.Text = "";
            richTextBox.Text = temp;
            int firstWordFound = 0;
            int totalWords = 0;

                while (index < richTextBox.Text.LastIndexOf(text))
                {
                    //Searches the text in the files
                    richTextBox.Find(text, index, richTextBox.Text.Length, RichTextBoxFinds.WholeWord);
                    //this just adds color to the 
                    richTextBox.SelectionBackColor = Color.Yellow;
                    //this is added when a match is found for the search
                    index = richTextBox.Text.IndexOf(text, index) + 1;
                    firstWordFound = richTextBox.Text.IndexOf(text, index);
                }      
        }

        public Control CreateControlForSearchResult(int yAxis, string labelText, labelType lType, Form1 form = null)
        {
            Label label = null;
            if (lType == labelType.col2)
            {
                label = new Label()
                {
                    BackColor = System.Drawing.SystemColors.ControlLightLight,
                    Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                    Location = new System.Drawing.Point(3, yAxis),
                    Name = "label10",
                    Size = new System.Drawing.Size(729, 25),
                    TabIndex = 1,
                    Text = labelText,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };
                return label;
            }
            if (lType == labelType.col1)
            {
                var linkLabel = new LinkLabel();
                linkLabel.ActiveLinkColor = System.Drawing.Color.Maroon;
                //linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                linkLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                linkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                linkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                linkLabel.LinkColor = System.Drawing.Color.Black;
                linkLabel.Location = new System.Drawing.Point(3, yAxis);
                linkLabel.Name = "linkLabel1";
                linkLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                linkLabel.Size = new System.Drawing.Size(64, 25);
                linkLabel.TabIndex = 0;
                linkLabel.TabStop = true;
                linkLabel.Text = "View";
                linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(form.linkFolder_LinkClicked);
                linkLabel.Click += new System.EventHandler(form.Clicked_EVentHandler);
                return linkLabel;
            };
            if (lType == labelType.col3)
            {
                label = new Label();
                label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                label.Location = new System.Drawing.Point(8, yAxis);
                label.Name = "linkLabel2";
                label.Size = new System.Drawing.Size(64, 25);
                label.TabIndex = 3;
                label.TabStop = true;
                label.Text = "Testzlb2";
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                return label;
            }
            return null;
        }
    }
}
