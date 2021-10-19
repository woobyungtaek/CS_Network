using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_Study
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(ServerFunc);
            serverThread.IsBackground = true;
            serverThread.Start();

            Console.WriteLine("종료하려면 Enter키를 누르세요.");
            Console.ReadLine();
        }

        private static void ServerFunc(object obj)
        {
            // 소켓 생성
            Socket socket =
                new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

            // 서버 끝단 설정 및 바인드
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 10200);
            socket.Bind(endPoint);

            // 클라이언트 끝단 생성
            byte[] recvByte = new byte[1024];
            EndPoint clientEP = new IPEndPoint(IPAddress.None, 0);
            
            while (true)
            {
                // 리시브 받음
                int nRecv = socket.ReceiveFrom(recvByte, ref clientEP);                
                string txt = Encoding.UTF8.GetString(recvByte, 0, nRecv);

                // 응답하기
                byte[] sendByte = Encoding.UTF8.GetBytes("Hello : " + txt);
                socket.SendTo(sendByte, clientEP);
            }

            //socket.Close();
        }

    }
}
