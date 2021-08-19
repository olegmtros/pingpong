using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace cppong
{
    class udp
    {
        //private const int listenPort = 11000;
        //public static string remip = "0";
        //public static int stat = 0;
        public static ipcalc conn = new ipcalc();
        public static string bk = Console.ReadLine();
        static IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(bk), conn.Ports);
        //public static string bk = Convert.ToString(conn.Portr);
        public static string pr = Convert.ToString(conn.Ports);

        public static void Startrx()
        {
            UdpClient rxUdpClient = new UdpClient();
            rxUdpClient.Client.ReceiveTimeout = 1000;
            try
            {
                Console.WriteLine(bk, "\n", pr);

                    //rxUdpClient.Connect(conn.BK, conn.Portr);
                    string message = Console.ReadLine();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    rxUdpClient.Send(data, data.Length, ipEndPoint);
                    Console.WriteLine("sending");
                    rxUdpClient.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                rxUdpClient.Close();
            }
            Console.Read();
            //in one function tx and rx



        }
        public static void Starttx()
        {
            UdpClient txUdpClient = new UdpClient(conn.Portr);

            try
            {
                //UdpClient txUdpClient = new UdpClient(conn.Portr);
                IPEndPoint RemoteIpEndPoint = null;
                byte[] receiveBytes = txUdpClient.Receive(ref RemoteIpEndPoint);
                string remip = Convert.ToString(RemoteIpEndPoint.Address);
                //rxUdpClient.Connect(conn.BK, conn.Portr);
                string ret = Encoding.Unicode.GetString(receiveBytes);
                Console.WriteLine(ret);
                txUdpClient.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                txUdpClient.Close();
            }

        }

    }
}

