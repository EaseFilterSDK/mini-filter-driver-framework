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

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace RegMon
{
    public partial class RegUnitTest : Form
    {

        bool isUnitTestCompleted = false;
        //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
        string licenseKey = "******************************************";

        public RegUnitTest(string _licenseKey)
        {
            InitializeComponent();
            this.licenseKey = _licenseKey;
        }


        public void StartFilterUnitTest()
        {
            try
            {
                FilterAPI.ResetConfigData();
                RegistryUnitTest.RegistryFilterUnitTest(richTextBox_TestResult,licenseKey);                

                GlobalConfig.Load();

              
            }
            catch (Exception ex)
            {
                richTextBox_TestResult.Text = "Filter test exception:" + ex.Message;
            }
        }

        private void RegUnitTest_Activated(object sender, EventArgs e)
        {
            if (!isUnitTestCompleted)
            {
                StartFilterUnitTest();
                isUnitTestCompleted = true;
            }
        }

    }
}
