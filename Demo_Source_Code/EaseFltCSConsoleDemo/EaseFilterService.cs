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
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    public partial class EaseFilterService : ServiceBase
    {
        public EaseFilterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string lastError = string.Empty;

            if (!FilterWorker.StartService(out lastError))
            {
                EventManager.WriteMessage(100, "StartFilter", EventLevel.Error, "Start filter service failed with error " + lastError);
            }
        }

        protected override void OnStop()
        {
            FilterWorker.StopService();
        }
    }
}
