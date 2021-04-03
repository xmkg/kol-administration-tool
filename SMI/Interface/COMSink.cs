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
using System.Windows.Forms;

namespace KAI.Interface
{
    public partial class COMSink : Form
    {
        /* Dummy class , will act as com sink.*/
        public COMSink()
        {
            Location = new Point(-1, -1);
            InitializeComponent();
            this.Shown += COMSink_Shown;
        }

        private void COMSink_Shown(object sender, EventArgs e)
        {
            Hide();
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;

        }

        private void COMSink_Load(object sender, EventArgs e)
        {
            
        }
    }
}
