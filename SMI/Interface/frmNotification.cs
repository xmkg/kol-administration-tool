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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KAI.Interface
{
    public partial class frmNotification : DevExpress.XtraEditors.XtraForm
    {
        const int AW_ACTIVATE = 0X20000;
        //Constants
        const int AW_SLIDE = 0X40000;
        const int AW_HOR_POSITIVE = 0X1;
        const int AW_HOR_NEGATIVE = 0X2;
        const int AW_BLEND = 0X80000;
        public const int AW_HIDE = 0x10000;
        private readonly Thread _toastThread;
        private System.Threading.Timer _closeTimer;
        [DllImport("user32")]
        static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        /* Mutex kullanmamızın sebebi, eğer başka bir toast mesajı zaten görüntüleniyorsa,
        * yeni gelen mesajı diğeri kaybolana kadar bekletmektir. 
        * Classlar arası paylaşımı sağlamak için statik olarak tanımlanmıştır.
        */
        private static readonly Mutex _mut = new System.Threading.Mutex();
        public frmNotification(string title, string message)
        {
          
            InitializeComponent();
            ShowInTaskbar = false;
            ShowIcon = false;
            TopMost = true;
            
            Location = new Point(Screen.PrimaryScreen.Bounds.Right - 320, Screen.PrimaryScreen.Bounds.Height - 140 - (Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height));
            StartPosition = FormStartPosition.Manual;
            ppOnline.Caption = title;
            ppOnline.Description = message;
            new Thread(() => ShowDialog()).Start();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            AnimateWindow(this.Handle, 500,AW_ACTIVATE | AW_BLEND);
            base.OnLoad(e);
        }



        private void ppOnline_Click(object sender, EventArgs e)
        {

        }

        private void frmNotification_Load(object sender, EventArgs e)
        {
            /* Eğer başka bir toast görüntüleniyorsa bekle */
            _mut.WaitOne();
            Application.DoEvents();
            _closeTimer = new System.Threading.Timer(CloseCallback, null, 2500, Timeout.Infinite);
        }
        public delegate void InvokeDelegate();
        private void CloseCallback(object state)
        {

            AnimateWindow(Handle, 500, AW_HIDE | (true ? AW_HOR_POSITIVE | AW_SLIDE : AW_BLEND));

            BeginInvoke(new InvokeDelegate(ReleaseAndClose));

        }

        public void ReleaseAndClose()
        {
            /* Eğer bekleyen herhangi bir toast varsa bilgilendir */
            _mut.ReleaseMutex();
            Close();
            Dispose();
        }
    }
}