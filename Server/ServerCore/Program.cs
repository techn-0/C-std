using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class Lock
    {
        AutoResetEvent _available = new AutoResetEvent(true); // 첫 인자로 게이트를 열고 시작할지 아닐지 선택 (그리도 자동으로 문 닫아줌)

        public void Acquire() // 획득
        {
            _available.WaitOne(); // 입장시도
            //_available.Reset(); // bool = false, 위 코드에 포함되어 있기에 생략 가능


        }

        public void Release() // 내려놓기
        {
            _available.Set(); // 다시 true로 바꿔준다
        }
    }
    class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();// Acquire 습득하다
                _num++;
                _lock.Release();// Release 풀어주다
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine( _num);
        }
    }
}
