using System;
using System.Threading;

/*
Monitor.Enter + Monitor.Exit 조합 사용
Lock 사용
Interlocked 사용 (몇가지 연산에 대해서 원자적 연산 단계를 제공)
EventWaitHandle 사용



스레드 풀도 따로 제공해 준다.
 */

namespace CSNetworkTestProj_Thread
{
    class Program
    {
        private int testCount;
        private EventWaitHandle eventWaitHandle;
        static void Main(string[] args)
        {
            Program pg = new Program();

            // initstate : 생성시 어떤 상태인지 false = non signal / true = signal
            // EventResetMode 
            // ManualReset : 수동으로 Reset을 호출해야 non-signal로 돌아온다.
            // AutoReset : set호출 후 자동으로 non-signal로 돌아온다.
            pg.eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            
            // 스레드풀 사용
            ThreadPool.QueueUserWorkItem(ThreadFunc_Server, pg);
            //Thread thread_server = new Thread(ThreadFunc_Server);
            //thread_server.Start(pg);
            //thread_server.IsBackground = true;
            //thread_server.Join();

            Thread thread_client = new Thread(ThreadFunc_Client);
            thread_client.Start(pg);
            thread_client.IsBackground = true;
            //thread_client.Join();

            // IsBackground 값이
            // false 라면 Exe 프로세스는 해당 스레드들이 모두 종료되어야 종료 된다.
            // true 라면 실행 종료에 영향을 주지 않는다.
            // 따라서 true 라면 메인 스레드가 종료되어 아무것도 볼 수 없는 상태가 될것이다.

            pg.eventWaitHandle.WaitOne();

            Console.WriteLine(String.Format("count {0}", pg.testCount));
        }

        static void ThreadFunc_Server(object obj)
        {
            Program inst = obj as Program;
            for(int idx = 0; idx < 10000; idx++)
            {
                inst.testCount += 1;
                //Thread.Sleep(1000 * 1);
                Console.WriteLine(String.Format("Server call {0}",idx) );
            }
        }
        static void ThreadFunc_Client(object obj)
        {
            Program inst = obj as Program;
            for (int idx = 0; idx < 10000; idx++)
            {
                inst.testCount += 1;
                //Thread.Sleep(1000 * 1);
                Console.WriteLine(String.Format("Client call {0}", idx));
            }
            inst.eventWaitHandle.Set();
        }
    }
}
