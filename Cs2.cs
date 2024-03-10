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
        volatile static bool _stop = false; //volatile으로 _stop을 휘발성으로 만듦(최적화 금지)

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");
            while (_stop == false);
            {
                //누가 stop신호 해주길 기다림
            }

            Console.WriteLine("쓰레드 끝!");
        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(2000);

            _stop = true;

            Console.WriteLine("stop호출");
            Console.WriteLine("종료 대기중");

            t.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}
