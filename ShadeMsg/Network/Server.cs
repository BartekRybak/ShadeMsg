using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using ShadeMsg.Security;

namespace ShadeMsg.Network
{
    class Server
    {
        public ConnectionHandler Connection;

        public Server(string adress,int port)
        {
            Connection = new ConnectionHandler();
        }

       

        /// <summary>
        /// Request to server
        /// </summary>
        /// <param name="adress">adress</param>
        /// <param name="args">arguments</param>
        /// <returns>response packet</returns>
        public Packet Request(string adress,Packet packet)
        {

            return new Packet();
        }
    }
}
