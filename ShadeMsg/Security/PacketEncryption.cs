using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using Newtonsoft.Json;
using System.IO;

namespace ShadeMsg.Security
{
    public class PacketEncryption
    {
        /// <summary>
        /// Encrypt packet before send
        /// </summary>
        /// <param name="packet">packet to crypt</param>
        /// <param name="password">password to crypt</param>
        /// <returns>Crypted Json/Baset64 Packet</returns>
        public static string EncryptPacket(Packet packet, string password)
        {
            try
            {
                string json_packet = JsonConvert.SerializeObject(packet);
                byte[] iv = Encryption.GenerateIV();
                byte[] crypted_packet = Encryption.Encrypt(json_packet, Encryption.CreateKey(password), iv);
                try
                {
                    crypted_packet = Encryption.Encrypt(json_packet, Encryption.CreateKey(password), iv);
                }
                catch
                {
                    throw new Exception("Can't crypt packet. Something is wrong.");
                }

                using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
                {
                    binaryWriter.Write(crypted_packet);
                    binaryWriter.Write(iv);

                    using (BinaryReader binaryReader = new BinaryReader(binaryWriter.BaseStream))
                    {
                        binaryReader.BaseStream.Position = 0;
                        return Convert.ToBase64String(binaryReader.ReadBytes(crypted_packet.Length + iv.Length));
                    }
                }
            }
            catch
            {
                return string.Empty;
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
            try
            {
                byte[] crypted_byte_packet_with_iv = Convert.FromBase64String(data);

                using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream()))
                {
                    binaryWriter.Write(crypted_byte_packet_with_iv);

                    using (BinaryReader binaryReader = new BinaryReader(binaryWriter.BaseStream))
                    {
                        binaryReader.BaseStream.Position = 0;
                        byte[] clear_byte_packet = binaryReader.ReadBytes(crypted_byte_packet_with_iv.Length - 16);
                        byte[] iv = binaryReader.ReadBytes(16);

                        try
                        {
                            string decrypted_json = Encryption.Decrypt(clear_byte_packet, Encryption.CreateKey(password), iv);
                            return JsonConvert.DeserializeObject<Packet>(decrypted_json);
                        }
                        catch
                        {
                            throw new Exception("Can't Decrypt packet. Wrong password or input data");
                        }

                    }
                }
            }
            catch(Exception e)
            {
                return Packet.Empty;
            }
            
        }
    }
}
