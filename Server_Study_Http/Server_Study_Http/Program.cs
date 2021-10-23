using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server_Study_Http
{
    class Program
    {
        static void Main(string[] args)
        {
            // using문은 2가지 경우, 지시자의 경우 네임스페이스를 사용표시
            // 아래와 같은경우 해당 리소스가 범위를 벗어나면 자동으로 Dispose해준다.
            using(Socket srvSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp))
            {
                Console.WriteLine("http://localhost:8000으로 방문해 보세요.");

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8000);

                srvSocket.Bind(endPoint);
                srvSocket.Listen(10);

                while(true)
                {
                    Socket clntSocket = srvSocket.Accept();
                    ThreadPool.QueueUserWorkItem(HttpProcessFunc, clntSocket);
                }

            }            
        }

        private static void HttpProcessFunc(object obj)
        {
            Socket socket = obj as Socket;

            byte[] reqBuf = new byte[4096];
            socket.Receive(reqBuf);

            string header = "http/1.0 200 OK\nContent-Type: text/html; charset=UTF-8\r\n\r\n";
            string body = "<html><body><mark>테스트 HTML</mark>웹 페이지 입니다.</body></html>";

            byte[] respBuf = Encoding.UTF8.GetBytes(header + body);

            socket.Send(respBuf);

            socket.Close();
        }
    }
}
