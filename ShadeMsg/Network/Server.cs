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
        private readonly string PASSWORD = "1234";
        public ConnectionHandler Connection;

        public Server(string adress,int port)
        {
            Connection = new ConnectionHandler();
        }

        /// <summary>
        /// Encrypt packet before send
        /// </summary>
        /// <param name="packet">packet to crypt</param>
        /// <param name="password">password to crypt</param>
        /// <returns>Crypted Json/Baset64 Packet</returns>
        private string EncryptPacket(Packet packet,string password)
        {
            string json_packet = JsonConvert.SerializeObject(packet);

            byte[] iv = Encryption.GenerateIV();
            byte[] crypted_packet = Encryption.Encrypt(json_packet, Encryption.CreateKey(password), iv);

            List<byte> final_packet = new List<byte>();

            foreach (byte b in crypted_packet)
            {
                final_packet.Add(b);
            }

            foreach (byte b in iv)
            {
                final_packet.Add(b);
            }

            return Convert.ToBase64String(final_packet.ToArray());
        }

        /// <summary>
        /// Decrypt Packet
        /// </summary>
        /// <param name="data">packet as base64 string</param>
        /// <param name="password">password to decrypt</param>
        /// <returns>Just a packet</returns>
        private Packet DecryptPacket(string data,string password)
        {
            // base64 -> cryptedData
            byte[] crypted_byte_packet_with_iv = Convert.FromBase64String(data);

            // Separate packet/iv
            List<byte> iv = new List<byte>();
            List<byte> clear_byte_packet = new List<byte>();

            for(int i = 0;i<crypted_byte_packet_with_iv.Length;i++)
            {
                if(i < crypted_byte_packet_with_iv.Length - 16)
                {
                    clear_byte_packet.Add(crypted_byte_packet_with_iv[i]);
                }
                else
                {
                    iv.Add(crypted_byte_packet_with_iv[i]);
                }
            }

            string decrypted_json = Encryption.Decrypt(clear_byte_packet.ToArray(), Encryption.CreateKey(password), iv.ToArray());
            return JsonConvert.DeserializeObject<Packet>(decrypted_json);  
        }

        /// <summary>
        /// Request to server
        /// </summary>
        /// <param name="adress">adress</param>
        /// <param name="args">arguments</param>
        /// <returns>response packet</returns>
        public Packet Request(string adress,Packet packet)
        {
            Packet responsePacket = Packet.Empty;
            using(WebClient client = new WebClient())
            {

                
                    
            }
            return responsePacket;
        }
    }
}
