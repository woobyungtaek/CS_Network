using System;
using System.Net;
//-----------------------------------------
using System.Net.Sockets;

namespace CSNetworkTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPAddress ipAddr2 = new IPAddress(new byte[] {127,0,0,1 });
            Console.WriteLine(ipAddr);
            Console.WriteLine(ipAddr2);

            IPEndPoint endPoint = new IPEndPoint(ipAddr, 9000);

            IPHostEntry hostEntry = Dns.GetHostEntry("www.naver.com");

            foreach(IPAddress ipAddress in hostEntry.AddressList)
            {
                Console.WriteLine(ipAddress);
            }

            string myComputer = Dns.GetHostName();

            Console.WriteLine("컴퓨터 이름 : " + myComputer);
            IPHostEntry entry1 = Dns.GetHostEntry(myComputer);

            foreach(IPAddress ipAddress in entry1.AddressList)
            {
                Console.WriteLine(ipAddress.AddressFamily + ": " + ipAddress);
            }

            //--------------------------------------------------------------------------------

            Socket socket_0 = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

            socket_0.Close();
        }
    }
}
