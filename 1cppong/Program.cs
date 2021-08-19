using System;

namespace cppong
{
    class Program
    {
        static void Main(string[] args)
        {
            for ( ; ; )
            {
                udp.Startrx();
                while (true) { udp.Starttx(); }
            }
        }
    }
}
