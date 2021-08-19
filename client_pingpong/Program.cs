using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client_pingpong
{
    class Program
    {
        static void Main(string[] args)
        {
            udp_conn.StartListener();
        }
    }
}
