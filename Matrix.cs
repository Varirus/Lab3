using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Matrix
    {
        public double[,] array2D { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }

        public Matrix (int rows, int cols, bool randomFill = false, int seed = 0)
        {
            array2D = new double[rows, cols];
            this.rows = rows;
            this.cols = cols;
            if (randomFill) {
                Random rand = new Random(seed);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        array2D[i, j] = rand.Next(0, 10000);
            }
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
