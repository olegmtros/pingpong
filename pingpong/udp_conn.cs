using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace server_pingpong
{

    class udp_conn
    {
        private const int listenPort = 11000;
        public static string remip = "0";
        public static int stat = 0;

        public static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);
                    remip = Convert.ToString(groupEP.Address);
                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                    string checks = "givetheip";
                    string returnData = Encoding.UTF8.GetString(bytes);
                    if (checks == returnData) { stat = 1; }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
