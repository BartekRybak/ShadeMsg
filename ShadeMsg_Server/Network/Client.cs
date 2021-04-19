using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;
using ShadeMsg.Network;
using ShadeMsg.Security;
using ShadeMsg.Network.Packets;
using System.Drawing;

namespace ShadeMsg_Server.Network
{
    

    class Client
    {
        public string nick;
        public Color nick_color;

        private string password;
        public Socket socket;
        private BackgroundWorker backgroundReceiver;

        public delegate void NewPacket_Delegate(Packet packet,Client client);
        public event NewPacket_Delegate NewPacket;

        public delegate void Disconected_delegate(Client client);
        public event Disconected_delegate Disconected;

        public Color[] AvaliableColors = new Color[]
        {
            Color.Tomato,Color.Aquamarine,Color.Cyan,
            Color.GreenYellow,Color.Magenta,Color.CornflowerBlue
        };

        public Client(Socket socket,string password)
        {
            Random rand = new Random();
            nick = socket.RemoteEndPoint.ToString();
            nick_color = AvaliableColors[rand.Next(AvaliableColors.Length)];

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
                        NewPacket(packet, this);
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
