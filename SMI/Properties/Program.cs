using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;
using KAI.Declarations;
using KAI.Interface;

namespace KAI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();else
            {
                MessageBox.Show(@"This application cannot run on systems older than Windows Vista!", @"Fatal error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            /* 
                 Why I created a seperated thread & sink form;
                 The COM interface, because of its' internal structure, can only be run on
                 threads with STA apartment state.
                 When we try to use any COM features on any occassion (eg. drag & drop feature)
                 the drag and drop registration will fail on forms which are running on MTA apartment
                 state threads.
                 Another caveat is, if you spawn your form in a STA thread, and if that form is disposed at some point,
                 the registered COM objects for this application will be released upon destruction.
                 That means, if you open your form in seperate thread for first time, everything runs smoothly,
                 everything is fine. But after closing the form, if you try to open form again, you will get a nice
                 COM exception.
                 The best solution I come up with to this situation, 
                         + Create a dummy form as sink (COMSink) (Invoke and BeginInvoke functions are our interest)
                         + Create an UI worker thread for the form, set apartment state as STA
                         + Create an application message pump with Application.Run
                 That's it. After this point, if you want to create any form, use following structure;

                        StaticReference.frmCOMSink.BeginInvoke(new Action(() => 
                        {
                                var frm = new DummyForm();
                                frm.Show();
                        } 
                        ));
                 
                 THIS IS STUPID, BUT WE WILL LIVE WITH THAT. THANKS TO MICROSOFT.
                 That's all for now.
  
            */
            new Thread(() =>
            {
                StaticReference.frmCOMSink = new COMSink();
                Application.Run(StaticReference.frmCOMSink);
            }) {ApartmentState = ApartmentState.STA, IsBackground = false, Name = "COM Sink Thread"}.Start();

            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            WindowsFormsSettings.AllowDpiScale = false;
            Application.SetCompatibleTextRenderingDefault(false);
            Control.CheckForIllegalCrossThreadCalls = false;
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Dark Style");
            XtraMessageBox.AllowCustomLookAndFeel = true;
            Application.Run(new frmLogin());

        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (StaticReference.frmCOMSink != null)
                StaticReference.frmCOMSink.Close();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
