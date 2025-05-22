using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Word_Search
{
    public partial class Form1 : Form
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Word Documents\dictionary.txt";
        bool changed = false;

        public Form1()
        {
            InitializeComponent();

            pathTextBox.Text = path;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            pathTextBox.Text = openFileDialog1.FileName;
            changed = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                path = pathTextBox.Text;

                if (Dictionary.arrayOfWords == null || changed)
                {
                    Dictionary.form = this;
                    Dictionary.setupList();

                    changed = false;
                }

                Dictionary.chooseWords();
            }
            catch {
                MessageBox.Show("There was an error\t\t\t\t\t", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }
    }
}
