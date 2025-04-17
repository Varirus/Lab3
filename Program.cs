using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Lab3_Tests")]
[assembly: InternalsVisibleTo("Lab3_UI")]
namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] matrixSizes = {100, 200, 300, 500 , 1000, 1200, 1300};
            int[] threadCounts = {2, 4, 8, 16, 32, 64, 128, 256, 512};

            Console.WriteLine("Benchmark");
            Console.WriteLine($"System: {Environment.ProcessorCount} logical processors\n");

            foreach (int size in matrixSizes)
            {
                Matrix a = new Matrix(size, size, true);
                Matrix b = new Matrix(size, size, true);
                Result result = new Result(a, b);
                Result resultSeq = new Result(a, b);

                Console.WriteLine($"Matrix size: {size}x{size}");

                Stopwatch sequentialWatch = Stopwatch.StartNew();
                resultSeq.MultiplyMatricesSequential();
                sequentialWatch.Stop();
                double sequentialTime = sequentialWatch.ElapsedMilliseconds;

                Console.WriteLine($"Sequential time: {sequentialTime} ms");
                Console.Write("----------------------------------------");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine($"{"",13} | {"Parallel",37} | {"Threads",37}");
                Console.Write("----------------------------------------");
                Console.WriteLine("-----------------------------------------------------");
                Console.Write("Threads | Time Parallel (ms) | SpeedUp | ");
                Console.WriteLine("Time Threads (ms) | SpeedUp");
                Console.Write("----------------------------------------");
                Console.WriteLine("-----------------------------------------------------");

                bool diffrent = false;
                foreach (int threads in threadCounts)
                {
                    double totalParallelTime = 0;
                    double totalThreadsTime = 0;

                    Stopwatch watch = Stopwatch.StartNew();
                    result.MultiplyMatricesParallel(threads);
                    watch.Stop();

                    totalParallelTime += watch.ElapsedMilliseconds;
                    double speedupParallel = sequentialTime / totalParallelTime;

                    watch = Stopwatch.StartNew();
                    result.MultiplyMatricesThreads(threads);
                    watch.Stop();

                    totalThreadsTime += watch.ElapsedMilliseconds;
                    double speedupThreads = sequentialTime / totalThreadsTime;

                    Console.WriteLine($"{threads,13} | {totalParallelTime,20} | {speedupParallel,13:F2}x | {totalThreadsTime,20} | {speedupThreads,13:F2}x");
                    
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            if (result.array2D[i, j] != resultSeq.array2D[i, j])
                                diffrent = true;
                }
                
                if (diffrent)
                {
                    Console.WriteLine("Results are diffrent");
                    Console.WriteLine("----------------Matrix A----------------------");
                    Console.WriteLine(a);
                    Console.WriteLine("----------------Matrix B----------------------");
                    Console.WriteLine(b);
                    Console.WriteLine("----------------Result Seq--------------------------");
                    Console.WriteLine(resultSeq);
                    Console.WriteLine("----------------Result--------------------------");
                    Console.WriteLine(result);
                    Console.WriteLine("-----------------------------------------------");
                    Thread.Sleep(1000);
                }
                    
            }
        }
    }
}