using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClassLibraryHelper.C_
{
    /// <summary>
    /// 性能计数器
    /// 使用 CodeTimer.Time("Thread Sleep", 1, () => { Thread.Sleep(3000); });
    /// 第一个参数 是名称
    /// 第二个 是执行次数
    /// 第三个 是执行方法
    /// 
    /// 返回 名称
    ///      方法执行的时间ms
    ///      CPU分配了多少时间片给这段方法来执行
    /// </summary>
    public static class CodeTimer
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

        }

        public static void Time(string name, int iteration, Action action)
        {
            if (string.IsNullOrEmpty(name))
                return;

            var currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            var gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
                gcCounts[i] = GC.CollectionCount(i);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++)
                action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            stopwatch.Stop();

            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t" + stopwatch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            for (int i = 0; i < GC.MaxGeneration; i++)
            {
                var count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }
            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }
    }
}
