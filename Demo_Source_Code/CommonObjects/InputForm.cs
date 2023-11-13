///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EaseFilter.CommonObjects
{
    public partial class InputForm : Form
    {
        public string InputText = string.Empty;

        public InputForm(string caption, string promptText,string inputText)
        {
            InitializeComponent();
            this.Text = caption;
            label_InputPrompt.Text = promptText;
            textBox_Input.Text = inputText;
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            InputText = textBox_Input.Text;
        }       

        private void button_GetFilePath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Input.Text = openFileDialog1.FileName;
            }
        }
    }
}
