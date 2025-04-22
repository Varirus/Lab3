using System;
using System.Collections.Generic;
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
    public partial class MatrixView : Window
    {
        public MatrixView(string name, int rows, int cols, double[,] array2D)
        {
            List<List<double>> lsts = new List<List<double>>();

            for (int i = 0; i < rows; i++)
            {
                lsts.Add(new List<double>());

                for (int j = 0; j < cols; j++)
                {
                    lsts[i].Add(array2D[i,j]);
                }
            }

            InitializeComponent();
            nameBox.Text = name;
            lst.ItemsSource = lsts;

        }
    }
}
