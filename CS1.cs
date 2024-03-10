using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++) 
            {
                Console.WriteLine("Hello Thread");
            }
                
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            
            for (int i = 0; i<5; i++)
            {
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
                t.Start();
            }

            /*for (int i = 0; i < 4; i++)
                ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });*/


            ThreadPool.QueueUserWorkItem(MainThread);

            //Thread t = new Thread(MainThread);
            //t.Name = "Test";
            //t.IsBackground = true; //백그라운드가 트루면 메인스레드가 종료될때 영향을받아 같이 종료됨
            //t.Start();


            //Console.WriteLine("스레드 기다리는중");
            //t.Join();   //메인쓰레드가 끝날떄 까지 기다렸다 끝나면 헬로우월드 출력

            //Console.WriteLine("Hello W");
            while (true) { }
        }
    }
}
