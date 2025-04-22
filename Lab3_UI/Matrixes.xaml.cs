using Lab3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab3_UI
{
    public partial class Matrixes : Window
    {
        private Result resultParallel;
        private Result resultThreads;
        private Lab3.Matrix a;
        private Lab3.Matrix b;

        public Matrixes()
        {
            //a = new Lab3.Matrix(10, 10, true);
            InitializeComponent();

        }

        private void OnGenerateInstanceClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                try
                {
                    int size = Int32.Parse(sizeEntry.Text);
                    int threads = Int32.Parse(threadsEntry.Text);
                    int seed = Int32.Parse(seedEntry.Text);
                    if (size <= 0)
                    {
                        MessageBox.Show("Entered size is wrong");
                    }
                    else if(threads <= 0)
                    {
                        MessageBox.Show("Entered threads count is wrong");
                    }
                    else
                    {
                        a = new Lab3.Matrix(size, size, true, seed);
                        b = new Lab3.Matrix(size, size, true, seed + 10);
                        resultThreads = new Result(a, b);
                        resultThreads.MultiplyMatricesThreads(threads);
                        resultParallel = new Result(a, b);
                        resultParallel.MultiplyMatricesParallel(threads);

                        OpenMatrixAButton.Visibility = Visibility.Visible;
                        OpenMatrixBButton.Visibility = Visibility.Visible;
                        OpenMatrixResultThreadsButton.Visibility = Visibility.Visible;
                        OpenMatrixResultParallelButton.Visibility = Visibility.Visible;
                    }
                }
                catch(Exception ee)
                {
                    MessageBox.Show("Entered parameters are wrong. " + ee.Message);
                }
            }
        }

        private void OnOpenMatrixAClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                MatrixView imagesWindow = new MatrixView("Matrix A", a.rows, a.cols, a.array2D);
                imagesWindow.Show();
            }
        }

        private void OnOpenMatrixBClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                MatrixView imagesWindow = new MatrixView("Matrix B", b.rows, b.cols, b.array2D);
                imagesWindow.Show();
            }
        }

        private void OnOpenMatrixResultParallelClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                MatrixView imagesWindow = new MatrixView("Result Parallel", resultParallel.rows, resultParallel.cols, resultParallel.array2D);
                imagesWindow.Show();
            }
        }

        private void OnOpenMatrixResultThreadsClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                MatrixView imagesWindow = new MatrixView("Result Threads", resultThreads.rows, resultThreads.cols, resultThreads.array2D);
                imagesWindow.Show();
            }
        }

        private void OnOpenImagesWindowClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Images imagesWindow = new Images();
                imagesWindow.Show();
                this.Close();
            }
        }


    }
}
