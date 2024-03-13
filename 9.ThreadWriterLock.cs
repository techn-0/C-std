using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class Lock
    {
        const int EMTPY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;
        // 
        int _flag = EMTPY_FLAG;

        public void WriteLock()
        {
            // 아무도 라이트락 리드락을 획득하고 있지 않을때 경합해서 소유권을 얻는다.
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while(true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {

                    if (Interlocked.CompareExchange(ref _flag, desired, EMTPY_FLAG) == EMTPY_FLAG)
                        return;

                    // 시도를 해서 성공하면 return
                    //if (_flag == EMTPY_FLAG)
                    //    _flag = desired;
                }
                Thread.Yield();
            }
        }
        public void WriteUnlock()
        {
            Interlocked.Exchange(ref _flag, EMTPY_FLAG);
        }
        public void ReadLock()
        {
            // 아무도 라이트락을 획득하고 있지 않으면 리드카운트를 1 늘린다
            while (true)
            {
                for (int i = 0;i < MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);
                    if (Interlocked.CompareExchange(ref _flag, expected + 1,expected) == expected)
                        return;

                    //if ((_flag & WRITE_MASK) == 0)
                    //{
                    //    _flag = _flag + 1;
                    //}
                }

                Thread.Yield();
            }
        }
        public void ReadUnlock()
        {

        }
    }
}
