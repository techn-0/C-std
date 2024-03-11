using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquire() // 획득
        {
            while (true)
            {
                /*int original = Interlocked.Exchange(ref _locked, 1);
                if (original == 0)
                    break;*/ //이렇게 중요한 값을 직접 제어하는건 비추

                // CAS(Compare-And-Swap) 이게 국룰
                int expected = 0;
                int desired = 1;

                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)    //값(expected)이 현재 값(_locked)과 같은 경우에만 새로운 값(desired)으로 교체합니다. 만약 교체가 성공적으로 이루어지면, 즉 _locked의 원래 값이 expected와 같았다면, expected를 반환하고 break문을 통해 반복문을 탈출합니다.
                    break;
            }
        }

        public void Release() // 내려놓기
        {
            _locked = 0;
        }
    }
    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 10000000; i++)
            {
                _lock.Acquire();// Acquire 습득하다
                _num++;
                _lock.Release();// Release 풀어주다
            }
        }
        static void Thread_2()
        {
            for (int i = 0; i < 10000000; i++)
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
