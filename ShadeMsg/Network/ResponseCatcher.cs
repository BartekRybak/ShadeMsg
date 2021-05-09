using System;
using System.Collections.Generic;
using System.Text;
using ShadeMsg.Network;
using ShadeMsg.Network.Packets;

namespace ShadeMsg.Network
{
    class ResponseCatcher
    {
        public delegate void CathPacket_Delegate(Packet packet);
        public event CathPacket_Delegate PacketCaught;
        public bool packetWasCaught = false;
        public Packet yourPacket;
        private Packet packetToCath;

        public ResponseCatcher(Packet packet)
        {
            packetToCath = packet;
        }

        public Packet WaitForPacket()
        {
            while(!packetWasCaught) { }
            return yourPacket;
        }

        public void Catch(Packet packet)
        {
            if(packetToCath.name == packet.name)
            {
                if(packet.GetArgument("type").value == "response")
                {
                    yourPacket = packet;
                    PacketCaught(packet);
                    packetWasCaught = true;
                }
            }
        }
    }
}
