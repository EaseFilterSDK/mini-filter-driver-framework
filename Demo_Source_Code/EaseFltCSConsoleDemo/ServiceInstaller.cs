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
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "EaseFilter Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // This must be identical to the WindowsService.ServiceBase name
            // set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "EaseFilter Service";
            serviceInstaller.Description = "EaseFilter File System Service";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
