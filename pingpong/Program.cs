using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server_pingpong
{
    class server
    {
        public static ipcalc conn = new ipcalc();
        public static int stat = 0;
        public static string remip = "0";
        private static void Send()
        {
            // Создаем UdpClient
            UdpClient sender = new UdpClient();
            string msg = Convert.ToString(conn.Iphost);
            // Создаем endPoint по информации об удаленном хосте
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remip), conn.Portr);
            try
            {
                if (stat == 1)
                {
                    // Преобразуем данные в массив байтов
                    byte[] bytes = Encoding.UTF8.GetBytes(msg);
                    sender.Send(bytes, bytes.Length, endPoint);
                    Console.WriteLine(Convert.ToString($"{endPoint} {msg} idk"));
                    stat = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally { sender.Close(); }
        }

        public static void Receiver()
        {
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(conn.Ports);
            try
            {
                while (true)
                {
                    IPEndPoint RemoteIpEndPoint = null;
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);
                    remip = Convert.ToString(RemoteIpEndPoint.Address);
                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    string checks = "givetheip";
                    if (checks == returnData) { stat = 1; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }
        static void Main(string[] args)
        {
            try
            {
                // Создаем поток для прослушивания
                Thread tRec = new Thread(Receiver);
                tRec.Priority = ThreadPriority.Lowest;
                tRec.IsBackground = true;
                //tRec.
                tRec.Start();
                while (true) { Send(); }
                Thread.Sleep(1000);
                tRec.Interrupt();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }
    }
}
