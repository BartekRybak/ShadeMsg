using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using Newtonsoft.Json;
using System.IO;

namespace ShadeMsg.Security
{
    class PacketEncryption
    {
        /// <summary>
        /// Encrypt packet before send
        /// </summary>
        /// <param name="packet">packet to crypt</param>
        /// <param name="password">password to crypt</param>
        /// <returns>Crypted Json/Baset64 Packet</returns>
        public static string EncryptPacket(Packet packet, string password)
        {
            string json_packet = JsonConvert.SerializeObject(packet);

            byte[] iv = Encryption.GenerateIV();
            byte[] crypted_packet = Encryption.Encrypt(json_packet, Encryption.CreateKey(password), iv);
            byte[] hmac = Encryption.GetHMAC(crypted_packet, Encryption.CreateKey(password));

            using(BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
            {
                binaryWriter.Write(crypted_packet);
                binaryWriter.Write(iv);
                binaryWriter.Write(hmac);

                using(BinaryReader binaryReader = new BinaryReader(binaryWriter.BaseStream))
                {
                    binaryReader.BaseStream.Position = 0;
                    return Convert.ToBase64String(binaryReader.ReadBytes(crypted_packet.Length + iv.Length + hmac.Length));
                }
            }
        }

        /// <summary>
        /// Decrypt Packet
        /// </summary>
        /// <param name="data">packet as base64 string</param>
        /// <param name="password">password to decrypt</param>
        /// <returns>Just a packet</returns>
        public static Packet DecryptPacket(string data, string password)
        {
            byte[] crypted_byte_packet_with_iv = Convert.FromBase64String(data);

            using(BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
            {
                binaryWriter.Write(crypted_byte_packet_with_iv);

                using(BinaryReader binaryReader = new BinaryReader(binaryWriter.BaseStream))
                {
                    binaryReader.BaseStream.Position = 0;
                    byte[] clear_byte_packet = binaryReader.ReadBytes(crypted_byte_packet_with_iv.Length - 80);
                    byte[] iv = binaryReader.ReadBytes(16);
                    byte[] hmac1 = binaryReader.ReadBytes(64);
                    byte[] hmac2 = Encryption.GetHMAC(clear_byte_packet, Encryption.CreateKey(password));

                    if(Encryption.CompareHMAC(hmac1,hmac2))
                    {
                        string decrypted_json = Encryption.Decrypt(clear_byte_packet, Encryption.CreateKey(password), iv);
                        return JsonConvert.DeserializeObject<Packet>(decrypted_json);
                    }
                    return Packet.Empty;
                }
            }
        }
    }
}
