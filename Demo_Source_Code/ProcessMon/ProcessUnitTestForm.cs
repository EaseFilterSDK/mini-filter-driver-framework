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
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace ProcessMon
{
    public partial class ProcessUnitTestForm : Form
    {
       
        bool isUnitTestCompleted = false;

        public ProcessUnitTestForm()
        {
            InitializeComponent();

        }

        private void ProcessUnitTest_Activated(object sender, EventArgs e)
        {
            if (!isUnitTestCompleted)
            {
                isUnitTestCompleted = true;

                ProcessUnitTest.ProcessFilterUnitTest(richTextBox_TestResult);
                GlobalConfig.Load();
                //System.Threading.Tasks.Task.Factory.StartNew(() => { ProcessUnitTest.ProcessFilterUnitTest(richTextBox_TestResult); });

            }
          
        }

    }
}
