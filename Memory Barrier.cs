using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    // 메모리 배리어
    // 1. 코드 재배치 억제
    // 2. 가시성 - 전광판에 커믿해서 남들도 볼수있게

    // 1) Full Memory Barrier(Thread.MemoryBarrier) - store/load 둘다 막음 | 스토어는 변수에 값 매기는거 로드는 변수의 값을 끄집어내느거
    // 2) Store Memory Barrier
    // 3) Load Memory Barrier
    class Program
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1; // store y
            // --------------------------
            Thread.MemoryBarrier();// 풀 메모리 배리어 장벽을세워 못넘어가게 하는것같음



            r1 = x; // load x
        }
        static void Thread_2()
        {
            x = 1; // store x
            // --------------------------
            Thread.MemoryBarrier();


            r2 = y; // load y
        }

        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;

                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;

            }
            Console.WriteLine($"{count}번만에 빠져나옴");
            //탈출이 되는이유 - 하드웨어는 명령이 서로 의존성이 없다 판단하면 스스로 순서를 바꾸는 최적화를 진행하기 떄문에 이런 일이 발생할수 있다


        }
    }
}
