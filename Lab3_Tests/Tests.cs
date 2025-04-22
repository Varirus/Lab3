using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Lab3;
using System.Drawing;
[TestClass]
public class Tests
{
    private const double Tolerance = 0.0001;

    [TestMethod]
    public void MultiplyMatrices_ShouldReturnCorrectResult()
    {
        Matrix a = new Matrix(2, 2);
        Matrix b = new Matrix(2, 2);
        
        double[,] arrayA = { { 1, 2 }, { 3, 4 } };
        double[,] arrayB = { { 5, 6 }, { 7, 8 } };
        a.array2D = arrayA;
        b.array2D = arrayB;
        double[,] expected = { { 19, 22 }, { 43, 50 } };
        Result result = new Result(a, b);

        result.MultiplyMatricesSequential();
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));

        result.MultiplyMatricesParallel(2);
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));

        result.MultiplyMatricesThreads(2);
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));
    }

    [TestMethod]
    public void MultiplyMatrices_ShouldHandleZeroMatrixCorrectly()
    {
        Matrix a = new Matrix(2, 3);
        Matrix b = new Matrix(3, 3);

        double[,] arrayA = { { 1, 2, 3 }, { 4, 5, 6 } };
        double[,] arrayB = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        a.array2D = arrayA;
        b.array2D = arrayB;
        double[,]? expected = arrayB.Clone() as double[,];
        Result result = new Result(a, b);

        result.MultiplyMatricesSequential();
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));

        result.MultiplyMatricesParallel(2);
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));

        result.MultiplyMatricesThreads(2);
        CollectionAssert.AreEqual(expected, result.array2D, new MatrixComparer(Tolerance));
    }

    [TestMethod]
    public void GenerateRandomMatrix_ShouldCreateMatrixWithCorrectDimensionsAndValues()
    {
        int cols = 3;
        int rows = 5;

        Matrix matrix = new Matrix(rows, cols);

        Assert.AreEqual(rows, matrix.array2D.GetLength(0));
        Assert.AreEqual(cols, matrix.array2D.GetLength(1));

        // Check if all values are between 0 and 10000
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Assert.IsTrue(matrix.array2D[i, j] >= 0 && matrix.array2D[i, j] <= 10000);
            }
        }
    }
}

public class MatrixComparer : System.Collections.IComparer
{
    private readonly double _tolerance;

    public MatrixComparer(double tolerance)
    {
        _tolerance = tolerance;
    }

    public int Compare(object x, object y)
    {
        double a = (double)x;
        double b = (double)y;

        if (Math.Abs(a - b) < _tolerance)
        {
            return 0;
        }

        return a.CompareTo(b);
    }
}