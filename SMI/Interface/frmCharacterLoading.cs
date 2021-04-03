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
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KAI.Declarations;

namespace KAI.Interface
{
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public partial class frmCharacterLoading : XtraForm
    {
        private Packet _receivedPkt;
        private mainFrm _mfrm;
        public frmCharacterLoading()
        {
            ControlBox = false;
            TopLevel = true;
            TopMost = true;
            InitializeComponent();
            Shown += frmCharacterLoading_Shown;
        }

        void frmCharacterLoading_Shown(object senderz, EventArgs ez)
        {

            BringToFront();
            Application.DoEvents();
            var sid = _receivedPkt.Read<short>();

            StaticReference.frmCOMSink.BeginInvoke(new Action(() =>
            {
                var newCharFrm = StaticReference.CreateNewCharacterForm(sid, _mfrm);
                newCharFrm.HandleCreated += (sender, e) =>
                {
                    CheckForIllegalCrossThreadCalls = false;
                    newCharFrm.CurrentUser.AllInfoUpdate(sid, _receivedPkt);
                    newCharFrm.RefreshTracedCharacterInfo();
                    newCharFrm.Text =
                        $"User Tracking | {newCharFrm.CurrentUser.AccountID}:{newCharFrm.CurrentUser.CharacterID}";
                    newCharFrm.RenderInventory();
                    Invoke(new Action(Close));
                    Close();
                };
                newCharFrm.Show();
                newCharFrm.BringToFront();

            }));
          /*  _mfrm.Invoke(new Action(() =>
            {
            new Thread(() =>
            {
                using (var newCharFrm = StaticReference.CreateNewCharacterForm(sid, _mfrm))
                {


                    newCharFrm.Shown += (sender, e) =>
                    {
                        CheckForIllegalCrossThreadCalls = false;
                        newCharFrm.CurrentUser.AllInfoUpdate(sid, _receivedPkt);
                        newCharFrm.RefreshTracedCharacterInfo();
                        newCharFrm.Text =
                            $"User Tracking | {newCharFrm.CurrentUser.AccountID}:{newCharFrm.CurrentUser.CharacterID}";
                        newCharFrm.RenderInventory();
                        Invoke(new Action(Close));
                        Close();
                    };

                    Application.Run(newCharFrm);
                }

                Trace.WriteLine("CharUI Thread exiting");
                }) {
                    IsBackground = true,
                    Priority = ThreadPriority.AboveNormal,
                    Name = $"CharUI Thread({sid})",
                    ApartmentState = ApartmentState.STA
            }
               
            .Start();
            }));*/
        }

        public void LoadCharacter(Packet receivedPacket, mainFrm frm)
        {
            _receivedPkt = receivedPacket;
            _mfrm = frm;
            ShowDialog();

        }


    }
}