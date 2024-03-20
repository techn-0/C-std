using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                // 받는다
                byte[] recvBuff = new byte[1024];
                int recvByte = clientSocket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvByte);
                Console.WriteLine($"[From Client] {recvData}");

                // 보낸다
                byte[] sendBuff = Encoding.UTF8.GetBytes("환영합니다."); // 문자열을 버퍼로 만들어줌
                clientSocket.Send(sendBuff); // 상대가 안받으면 여기서 대기

                //쫓아냄
                clientSocket.Shutdown(SocketShutdown.Both); // 클로즈 전에 예고(없어도 동작은 함)
                clientSocket.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            
        }

        static void Main(string[] args)
        {
            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.init(endPoint, OnAcceptHandler);
            Console.WriteLine(" Listening...");

            while (true)
            {
                ;
            }

        }
    }
}
