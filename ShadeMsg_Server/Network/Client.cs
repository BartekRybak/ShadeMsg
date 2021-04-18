using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using ShadeMsg.Network;
using ShadeMsg.Security;

namespace ShadeMsg_Server.Network
{
    class Client
    {
        private string password;
        public Socket socket;
        private BackgroundWorker backgroundReceiver;

        public delegate void NewPacket_Delegate(Packet packet,Socket socket);
        public event NewPacket_Delegate NewPacket;

        public delegate void Disconected_delegate(Client client);
        public event Disconected_delegate Disconected;

        public Client(Socket socket,string password)
        {
            Random rand = new Random();
            this.password = password;
            this.socket = socket;
            backgroundReceiver = new BackgroundWorker();
            backgroundReceiver.WorkerSupportsCancellation = true;
            backgroundReceiver.DoWork += BackgroundReceiver_DoWork;
            backgroundReceiver.RunWorkerAsync();
        }

        public void Stop()
        {
            backgroundReceiver.CancelAsync();
        }

        private void BackgroundReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!backgroundReceiver.CancellationPending)
            {
                byte[] data = new byte[1024];
                
                if(socket.Connected)
                {
                    try
                    {
                        socket.Receive(data);
                        string data_string = Encoding.UTF8.GetString(data).Trim('\0');
                        Packet packet = PacketEncryption.DecryptPacket(data_string, password);
                        NewPacket(packet, socket);
                    }
                    catch
                    {
                        Disconected(this);
                    }
                }
            }
        }
    }
}
