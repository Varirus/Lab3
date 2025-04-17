using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Result
    {
        public double[,] array2D { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }
        internal Matrix a;
        internal Matrix b;

        public Result(Matrix a, Matrix b) {
            array2D = new double[b.rows, a.cols];
            rows = a.rows;
            cols = b.cols;
            this.a = a;
            this.b = b;
        }

        internal void MultiplyMatricesSequential()
        {
            array2D = new double[b.rows, a.cols];
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < a.cols; k++)
                    {
                        temp += a.array2D[i, k] * b.array2D[k, j];
                    }
                    array2D[i, j] = temp;
                }
            }
        }

        internal void MultiplyMatricesParallel(int maxDegreeOfParallelism)
        {
            array2D = new double[b.rows, a.cols];
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            Parallel.For(0, a.rows, options, i =>
            {
                for (int j = 0; j < b.cols; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < a.cols; k++)
                    {
                        temp += a.array2D[i, k] * b.array2D[k, j];
                    }
                    array2D[i, j] = temp;
                }
            });
        }
        internal void threadFunction(int x,int interval)
        {
            //Console.WriteLine(x);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = x; j < b.cols; j+=interval)
                {
                    double temp = 0;
                    for (int k = 0; k < a.cols; k++)
                    {
                        temp += a.array2D[i, k] * b.array2D[k, j];
                    }
                    array2D[i, j] = temp;
                }
            }
        }

        internal void MultiplyMatricesThreads(int threadsAmount)
        {
            array2D = new double[b.rows, a.cols];
            Thread[] threads = new Thread[threadsAmount];
            for (int i = 0; i < threadsAmount; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() => threadFunction(threadIndex, threadsAmount));
            }
            foreach (Thread x in threads) x.Start();
            foreach (Thread x in threads) x.Join();
        }
        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < rows; i++)
            {
                output += "[";

                for (int j = 0; j < cols; j++)
                {
                    output += array2D[i, j] + ", ";
                }
                output += "]\n";
            }
            return output;
        }
    }
}
