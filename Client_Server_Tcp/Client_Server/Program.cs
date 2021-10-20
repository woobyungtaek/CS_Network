using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread clientThread = new Thread(ClientFunc);
            clientThread.IsBackground = true;
            clientThread.Start();

            clientThread.Join();
            Console.WriteLine("종료합니다.");
        }

        private static void ClientFunc(object obj)
        {
            Socket socket =
                new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);

            EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 10200);

            socket.Connect(serverEP);

            string inputStr = "";
            while (true)
            {
                Console.WriteLine("보내기 : s / 종료 : x");
                inputStr = Console.ReadLine();

                if (inputStr == "x")
                {
                    break;
                }
                else if (inputStr == "s")
                {
                    //Send
                    byte[] buf = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                    socket.Send(buf);


                    byte[] recvByte = new byte[1024];
                    int nRecv = socket.Receive(recvByte);
                    string txt = Encoding.UTF8.GetString(recvByte, 0, nRecv);

                    Console.WriteLine(txt);
                    Thread.Sleep(0);
                }
            }

            socket.Close();
        }
    }
}
