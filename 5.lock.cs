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
        static int number = 0;
        static object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; i++)
            {
                //Monitor.Enter(_obj); //문을 잠궈 나만 쓸수 있게 한다(상호 배제) 대신 관리가 어려워짐
                //number++;
                //return; // Exit으로 안풀어주고 나가면 무한대기(데드락) 아래의 트라이 파이널리를 쓰면 해결가능
                //Monitor.Exit(_obj);

               /* try
                {
                    Monitor.Enter(_obj);
                    number++;

                }
                finally // 파이널리는 무조건 한번은 실행됨
                {
                    Monitor.Exit(_obj);
                }*/
               //이걸 또 락으로 쓰기 편하게 가능
               lock (_obj)
                {
                    number++;
                }
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Monitor.Enter(_obj); //이미 잠근 사람이 있다면 잠시 대기
                number--;
                Monitor.Exit(_obj);
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(number);

        }
    }
}
