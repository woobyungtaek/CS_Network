using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_Study_Http
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddr = Dns.GetHostEntry("www.naver.com").AddressList[0];

            EndPoint serverEP = new IPEndPoint(ipAddr, 80);
            socket.Connect(serverEP);

            string request = "GET / HTTP/1.0\r\nHost: www.naver.com\r\n\r\n";
            byte[] sendBuffer = Encoding.UTF8.GetBytes(request);

            socket.Send(sendBuffer);

            MemoryStream ms = new MemoryStream();
            while(true)
            {
                byte[] rcvBuffer = new byte[4096];

                int nRecv = socket.Receive(rcvBuffer);
                if (nRecv == 0)
                {
                    break;
                }

                ms.Write(rcvBuffer, 0, nRecv);
            }

            socket.Close();

            string response = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            Console.WriteLine(response);

            File.WriteAllText("naverpage.html", response);
        }
    }
}
