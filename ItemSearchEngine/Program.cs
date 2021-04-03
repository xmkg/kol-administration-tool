/**
 * ______________________________________________________
 * This file is part of ko-administration-tool project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2017)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace KAI.ISEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            else
            {
                MessageBox.Show(@"This application cannot run on systems older than Windows Vista!", @"Fatal error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           // BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            Application.Run(mainForm: new frmSearchItem());
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
