using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace consock
{
    class Udp
    {
        private static ipcalc conn = new ipcalc();
        private static int senddo = 1;
        private static IPAddress ripen = conn.BK;
        private static string msg = "give ip";
        public static void StartListener_responder()
        {
            Console.WriteLine($"client started\nconn parameters {conn.Iphost} {conn.Ports} {conn.BK} {conn.Portr} {conn.Gwad} {conn.Mask}");
            for ( ; ; )
            {
                if (senddo != 0) { Listen(); }
                else { Console.WriteLine("the end"); Console.ReadLine(); }
            }
        }
        private static void Listen()
        {
            UdpClient receivingUdpClient = new UdpClient(conn.Portr);
            receivingUdpClient.Client.ReceiveTimeout = 2000;
            IPEndPoint RemoteIpEndPoint = null;
            string returnData = null;
            string ipxe = null;

            try
            {
                while (true)
                {
                    if (senddo == 3 && returnData == "bye") { senddo = 0; Console.WriteLine(senddo); Console.Out.WriteLine(ipxe); Environment.Exit(0); }
                    if (senddo == 2 && Convert.ToString(ripen) == returnData)  { ipxe = msg = Convert.ToString(ripen); senddo = 3; } //
                    if (senddo == 1) { msg = "give ip";  senddo = 2; }
                    if (senddo != 0) { Transmit(msg); }
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                    returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine($"server {RemoteIpEndPoint} send answer: {returnData}");
                    ripen = RemoteIpEndPoint.Address;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: приема" + ex.ToString() + "\n  " + ex.Message);
            }
            finally { receivingUdpClient.Close(); }
        }

        private static void Transmit(string msg)
        {
            UdpClient sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(ripen, conn.Ports);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                sender.Send(bytes, bytes.Length, endPoint);
                Console.WriteLine($"client in {conn.Iphost} sended answer to {ripen} this: {msg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: передачи" + ex.ToString() + "\n  " + ex.Message);
            }
            finally { sender.Close(); }
        }
    }
}

