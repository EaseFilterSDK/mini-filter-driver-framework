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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{
    public partial class AccountForm : Form
    {
        public  static string accountName = string.Empty;
        public  static string password = string.Empty;
        public  static bool isAuthorized = false;

        public AccountForm()
        {           
            InitializeComponent();

            textBox_EmailAddress.Text = GlobalConfig.AccountName;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button_SignIn_Click(object sender, EventArgs e)
        {         
            button_SignIn.Enabled = false;        

            try
            {
                accountName = textBox_EmailAddress.Text.ToLower().Trim();
                password = textBox_Password.Text.Trim();

                if (string.IsNullOrEmpty(accountName))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The email name can't be empty.", "account", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }

                if (string.IsNullOrEmpty(password.Trim()))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The password can't be empty.", "password", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }

                if (accountName.IndexOf('@') <= 0 || accountName.IndexOf('.') <=0 )
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The email name " + accountName + " is invalid.", "account", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
               

                 GlobalConfig.AccountName = accountName;
                 GlobalConfig.SaveConfigSetting();

                string lastError = string.Empty;

                isAuthorized = WebFormServices.SignInAccount(accountName, password, ref lastError);

                if (!isAuthorized)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show(lastError, "SignIn", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                else
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("Log in successfully.", "SignIn", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Sign in account failed with error" + ex.Message, "SignIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button_SignIn.Enabled = true;

                if (isAuthorized)
                {
                    this.Close();
                }
            }
        }


        private void textBox_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_SignIn_Click(null, null);
            }

        }

        private void button_SignUp_Click(object sender, EventArgs e)
        {
            
            button_SignUp.Enabled = false;

            try
            {
                accountName = textBox_EmailAddress.Text.ToLower().Trim();
                password = textBox_Password.Text.Trim();           

                if (string.IsNullOrEmpty(accountName))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The account name can't be empty.", "account", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }

                if (accountName.IndexOf('@') <= 0 || accountName.IndexOf('.') <= 0)
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The email name " + accountName + " is invalid.", "account", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (string.IsNullOrEmpty(textBox_Password.Text.Trim()))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The password can't be empty.", "password", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }             

                if (password.Length < 7 || !Regex.IsMatch(password, @"[\d]", RegexOptions.ECMAScript) || (!Regex.IsMatch(password, @"[a-z]", RegexOptions.ECMAScript) && !Regex.IsMatch(password, @"[A-Z]", RegexOptions.ECMAScript)))
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show("The input password length has to be 8 or greater with alphanumeric characters.", "password", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }

                GlobalConfig.AccountName = textBox_EmailAddress.Text;
                GlobalConfig.SaveConfigSetting();
              

                string lastError = string.Empty;
                bool retVal = WebFormServices.SignUpAccount(accountName,password, ref lastError);
                MessageBoxIcon icon = MessageBoxIcon.Information;
                if (!retVal)
                {
                    icon = MessageBoxIcon.Error;
                }

                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show(lastError, "SignUp", MessageBoxButtons.OK, icon);

                return;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Sign up account failed with error" + ex.Message, "SignUp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button_SignUp.Enabled = true;
                this.Close();
            }
        }

      
    }
}
