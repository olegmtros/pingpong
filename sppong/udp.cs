using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace sppong
{
    class Udp
    {
        private static ipcalc conn = new ipcalc();
        private static int senddo = 0;
        private static IPAddress ripen = null;
        private static string msg = "";
        public static void StartListener_responder()
        {
            Console.WriteLine($"Sever started\nconn parameters {conn.Iphost} {conn.Ports} {conn.BK} {conn.Portr} {conn.Gwad} {conn.Mask}");
            for ( ; ; )
            {
                Listen();
            }
        }
        public static void Listen()
        {
            UdpClient receivingUdpClient = new UdpClient(conn.Ports);
            IPEndPoint RemoteIpEndPoint = null;
            try
            {
                while (true)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);
                    // Преобразуем и отображаем данные
                    StringBuilder sb = new StringBuilder();
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    sb.Insert(0, returnData);
                    string sbb = Convert.ToString(sb);


                    Console.WriteLine($"client in {RemoteIpEndPoint} send msg: {sbb}");
                    //Console.ReadLine();
                    if (sbb == "give ip") { senddo = 1; ripen = RemoteIpEndPoint.Address; msg = Convert.ToString(conn.Iphost);  }
                    if (sbb == Convert.ToString(conn.Iphost)) { senddo = 1; msg = "bye"; }
                    if (senddo != 0)
                    {
                        Transmit(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        public static void Transmit(string msg)
        {
            UdpClient sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(ripen, conn.Portr);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                sender.Send(bytes, bytes.Length, endPoint);
                Console.WriteLine($"ipxe in {conn.Iphost} sended to {ripen} this:{msg}");
                senddo = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally { sender.Close(); }
        }
    }
}

