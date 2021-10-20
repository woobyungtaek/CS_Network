using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_Study
{
    public class AsyncStateData
    {
        public byte[] buffer;
        public Socket socket;
    }
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
                    SocketType.Stream,
                    ProtocolType.Tcp);

            // 서버 바인드, 리슨 큐 길이 설정
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 10200);
            socket.Bind(endPoint);
            socket.Listen(10);

            while (true)
            {
                Socket clientSocket = socket.Accept();

                // 비동기식
                AsyncStateData data = new AsyncStateData();
                data.buffer = new byte[1024];
                data.socket = clientSocket;

                // 리시브 걸어두기
                clientSocket.BeginReceive(data.buffer, 0, data.buffer.Length,
                    SocketFlags.None, AsyncReceiveCallBack, data);
                /*
                // 동기식
                byte[] recvByte = new byte[1024];
                // 리시브 받음
                int nRecv = clientSocket.Receive(recvByte);
                string txt = Encoding.UTF8.GetString(recvByte, 0, nRecv);
                // 응답하기
                byte[] sendByte = Encoding.UTF8.GetBytes("Hello : " + txt);
                clientSocket.Send(sendByte);
                clientSocket.Close();
                */
            }
        }

        /// <summary>
        /// 받기 콜백 함수
        /// 메세지를 받으면 실행한다.
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void AsyncReceiveCallBack(IAsyncResult asyncResult)
        {
            try
            {
                AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
                int nRecv = rcvData.socket.EndReceive(asyncResult);
                string txt = Encoding.UTF8.GetString(rcvData.buffer, 0, nRecv);

                // 받은 메세지를 처리하고 그에 맞는 행동을 해야한다.

                byte[] sendBytes = Encoding.UTF8.GetBytes("hello : " + txt);
                rcvData.socket.BeginSend(sendBytes, 0, sendBytes.Length,
                    SocketFlags.None, AsyncSendCallBack, rcvData);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 보내기 콜백 함수
        /// 메세지를 보내고 나면 실행한다.
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void AsyncSendCallBack(IAsyncResult asyncResult)
        {
            AsyncStateData data = asyncResult.AsyncState as AsyncStateData;
            data.socket.EndSend(asyncResult);

            //다시 리시브 걸기
            data.socket.BeginReceive(data.buffer, 0, data.buffer.Length,
                SocketFlags.None, AsyncReceiveCallBack, data);

            // 소켓 종료
            //data.socket.Close();
        }
    }
}
