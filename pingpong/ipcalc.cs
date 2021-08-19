using System;
using System.Net;
using System.Net.NetworkInformation;

namespace server_pingpong
{
    class ipcalc
    {
        private IPAddress bk;
        public IPAddress BK
        {
            get { return bk; }
            set { bk = value; }
        }
        private IPAddress gwad;
        public IPAddress Gwad
        {
            get { return gwad; }
            set { gwad = value; }
        }
        private IPAddress iphost;
        public IPAddress Iphost
        {
            get { return iphost; }
            set { iphost = value; }
        }
        private IPAddress mask;
        public IPAddress Mask
        {
            get { return mask; }
            set { mask = value; }
        }
        private int portr;
        public int Portr
        {
            get { return portr; }
            set { portr = value; }
        }
        private int ports;
        public int Ports
        {
            get { return ports; }
            set { ports = value; }
        }

        public ipcalc()
        {
            SetConnParameters();
        }

        private void SetConnParameters()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            iphost = IPAddress.Loopback;//IPAddress.Loopback;
            Gwad = IPAddress.Loopback;
            mask = IPAddress.Loopback;
            bk = IPAddress.Loopback; //IPAddress.Loopback;
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                MulticastIPAddressInformationCollection multicast = adapterProperties.MulticastAddresses;
                GatewayIPAddressInformationCollection gw = adapterProperties.GatewayAddresses;
                foreach (GatewayIPAddressInformation gww in gw)
                {
                    if (gww.Address != null)
                    {
                        string c = ":";
                        string b = "127.0.0.1";
                        string d = Convert.ToString(gww.Address);
                        bool n = d.Contains(c);
                        if (b != d && n == false)
                        {
                            foreach (UnicastIPAddressInformation uni in uniCast)
                            {
                                d = Convert.ToString(uni.Address);
                                n = d.Contains(c);
                                if (b != d && n == false)
                                {
                                    gwad = gww.Address;
                                    mask = uni.IPv4Mask;
                                    iphost = uni.Address;
                                }
                            }
                        }
                    }
                } //получение адресов интерфейса
                //преобразование в биты для вычисления конечного адреса подсети
                byte[] g = gwad.GetAddressBytes();
                byte[] m = mask.GetAddressBytes();
                int[,] BITG = new int[g.Length, 8];
                int[,] BITM = new int[m.Length, 8];
                //получаем биты шлюза
                for (int i = 0; i < g.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BITG[i, j] = (g[i] >> j) & 0x01; //совершенно не понимаю что я скопировал но это работает
                        BITM[i, j] = (m[i] >> j) & 0x01;

                    }
                }
                //получаем биты маски
                for (int i = 0; i < g.Length; i++)//переворачивание битов
                {
                    for (int j = 0; j < 8; j++) { if (BITM[i, j] == 0) { BITG[i, j] = 1; } }
                }
                int[] bkb = new int[4];
                int[] matrix = new int[8];
                //формула для матрицы из 2 в 10
                for (int i = 0; i < 8; i++)
                {
                    if (i == 0) { matrix[i] = 1; }
                    else { matrix[i] = matrix[i - 1] * 2; }
                }
                for (int k = 0; k < bkb.Length; k++) //translate to 10 bkast ip
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BITG[k, j] *= matrix[j];
                        bkb[k] += BITG[k, j];
                    }
                }
                //преобразование в IPAddress 
                string ipbk = null;
                for (int i = 0; i < bkb.Length; i++)
                {
                    if (i == bkb.Length - 1)
                    {
                        string ipb = Convert.ToString(bkb[i]);
                        ipbk += String.Concat(ipb);
                    }
                    else
                    {
                        string ipb = Convert.ToString(bkb[i]);
                        ipbk += String.Concat(ipb, ".");
                    }
                }
                bk = IPAddress.Parse(ipbk); portr = 50101; ports = 50100;
            }
        }
    }
}
