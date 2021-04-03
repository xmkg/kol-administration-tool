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
using DevExpress.Skins;
using DevExpress.XtraEditors;

namespace KAI
{
    class InputBox
    {
        
        private readonly object _initialValue = "";
        private XtraForm _dialog = null;
        private readonly string _title;

        public delegate void DialogCloseDelegate(object x);
        public event DialogCloseDelegate OnDialogClosed;
        private SimpleButton _confirmButton;
        private SimpleButton _cancelButton;
        private TextEdit _editBox;
        private bool _confirmed = false;
        public InputBox(string title,object initialValue)
        {

            _initialValue = initialValue;
            _title = title;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
           // _layoutPanel = new FlowLayoutPanel() {Dock = DockStyle.Fill,AutoSize = true,AutoSizeMode =  AutoSizeMode.GrowAndShrink};
            _confirmButton = new SimpleButton { Text = "Confirm" };
            _cancelButton = new SimpleButton { Text = "Cancel" };
            _editBox = new TextEdit {Text = Convert.ToString(_initialValue)};
            _dialog = new XtraForm
            {
                Size = new Size(250, 115),
                Text = _title,
                StartPosition = FormStartPosition.CenterParent,
                ControlBox = false,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                
            };
            _dialog.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
            _dialog.LookAndFeel.UseDefaultLookAndFeel = false;
            _editBox.Size = new Size(200, 22);
            _editBox.Location = new Point(15, 15);
            _confirmButton.Location = new Point(25, 45);
            _cancelButton.Location = new Point(130, 45);
            _dialog.Controls.Add(_editBox);
            _dialog.Controls.Add(_confirmButton);
            _dialog.Controls.Add(_cancelButton);
      
            _dialog.Controls.Add(_editBox);
            _dialog.Controls.Add(_confirmButton);
            _dialog.Controls.Add(_cancelButton);
            _cancelButton.Click += _cancelButton_Click;
            _confirmButton.Click += _confirmButton_Click;
        }

        public void Show()
        {
            _dialog.ShowDialog();
            OnDialogClosed?.Invoke(_confirmed ? _editBox.Text : _initialValue);
        }
        private void _confirmButton_Click(object sender, EventArgs e)
        {
            _confirmed = true;
            _dialog.Close();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            _confirmed = false;
            _dialog.Close();
        }

    }
}
