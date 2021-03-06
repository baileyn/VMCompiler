﻿using HackCompiler.Hack;
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

namespace HackGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                tbFilePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if(tbFilePath.Text == null)
            {
                MessageBox.Show("No file to compile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Program program = new Program(tbFilePath.Text);
                    Compiler compiler = new Compiler(program);

                    try
                    {
                        compiler.Compile();
                        compiler.Write();
                    }
                    catch (CompilationException compilationException)
                    {
                        MessageBox.Show(compilationException.Message, "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    MessageBox.Show("The compilation was a success! Output files written to:\n" + compiler.OutputFile, "Compilation Success");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
    }
}
