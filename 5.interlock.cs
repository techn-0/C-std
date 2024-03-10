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

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; i++)
            {
                //number++; // 어셈블리로 보면 이작업은 사실 3개로 나누어져있다. 아래주석 (원자성문제로 실행결과가 달라짐)
                // int temp = number;
                // temp += 1;
                // number = temp;
                Interlocked.Increment(ref number); // 이렇게하면 한번에 실행되어 원자성문제 해결 하지만 성능문제 발생, 순서보장
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Interlocked.Decrement(ref number);
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
