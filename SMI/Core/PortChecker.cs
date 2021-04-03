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
using System.Net;
using System.Net.Sockets;

namespace KAI.Core
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    internal sealed class PortChecker : IDisposable
    {
        public delegate void ResultCallback(PortChecker reference, bool result);

        private TcpClient _client = new TcpClient();
        private readonly string _ip;
        private readonly ushort _port;

        public PortChecker(string ip, ushort port, ResultCallback callbackFunction)
        {
            _ip = ip;
            _port = port;
            OnResultReceived += callbackFunction;
        }

        public void Dispose()
        {
            if (_client.Connected)
                _client.GetStream().Close();
            _client.Close();
            _client = null;
        }

        private event ResultCallback OnResultReceived;

        public void Check()
        {
            Dns.BeginGetHostEntry(_ip, ResolveCallbacks, null);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _client.EndConnect(ar);
                OnResultReceived.Invoke(this, true);
            }
            catch (Exception ex)
            {
                OnResultReceived.Invoke(this, false);
            }
        }

        private void ResolveCallbacks(IAsyncResult ar)
        {
            try
            {
                var ihe = Dns.EndGetHostEntry(ar);
                if (ihe.AddressList.Length == 0)
                    throw new Exception();
                _client.BeginConnect(ihe.AddressList, _port, ConnectCallback, null);
            }
            catch (Exception)
            {
                _client.BeginConnect(_ip, _port, ConnectCallback, null);
            }
        }
    }
}