using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab3_UI
{
    public partial class Images : Window
    {
        private BitmapImage? img;
        public Images()
        {
            InitializeComponent();
        }

        private void OnOpenFileClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".bmp";
                dialog.Filter = "Bitmap file (.bmp)|*.bmp";

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string filename = dialog.FileName;
                    Uri fileUri = new Uri(filename);
                    img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = fileUri;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    SourceImageView.Source = img;

                    ImageAfterOperation1.Visibility = Visibility.Collapsed;
                    ImageAfterOperation2.Visibility = Visibility.Collapsed;
                    ImageAfterOperation3.Visibility = Visibility.Collapsed;
                    ImageAfterOperation4.Visibility = Visibility.Collapsed;
                    SourceImageView.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnStartConvertionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {

                Thread[] threads = new Thread[4];
                threads[0] = new Thread(() => negative());
                threads[1] = new Thread(() => mirror());
                threads[2] = new Thread(() => threshold(100));
                threads[3] = new Thread(() => grayscale());
                //threads[0].Start();
                foreach (Thread x in threads) x.Start();
            }
        }

        private void OnOpenMatrixesWindowClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Matrixes matrixesWindow = new Matrixes();
                matrixesWindow.Show();
                this.Close();
            }
        }
        private void negative()
        {
            int width = 0, height = 0, stride = 0, bytesPerPixel = 0;
            byte[] pixels = new byte[height * stride];
            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                width = writable.PixelWidth;
                height = writable.PixelHeight;

                // bytes per row
                stride = writable.BackBufferStride;
                bytesPerPixel = (writable.Format.BitsPerPixel + 7) / 8;
                pixels = new byte[height * stride];

                writable.CopyPixels(pixels, stride, 0);
            });

            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                pixels[i] = (byte)(255 - pixels[i]);
                pixels[i + 1] = (byte)(255 - pixels[i+1]);
                pixels[i + 2] = (byte)(255 - pixels[i+2]);
            }

            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                writable.WritePixels(new Int32Rect(0, 0, width, height),pixels, stride, 0);
                ImageAfterOperation1.Source = writable;
                ImageAfterOperation1.Visibility = Visibility.Visible;
            });
        }
        // Zdaje sobie sprawę że disptacher invoke to wywołuje się na głównym wątku, ale tylko tak mam dostęp do elementów UI.
        // Mimo to, że znaczna część funkcji musi być przekazana do Dispatchera, to iteracja po pikselach i ich zmienianie wywołuje się na osobnych wątkach, co realizuje
        // wielowątkowość zadania.

        private void mirror()
        {
            int width = 0, height = 0, stride = 0, bytesPerPixel = 0;
            byte[] pixels = new byte[height * stride];
            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                width = writable.PixelWidth;
                height = writable.PixelHeight;

                // bytes per row
                stride = writable.BackBufferStride;
                bytesPerPixel = (writable.Format.BitsPerPixel + 7) / 8;
                pixels = new byte[height * stride];

                writable.CopyPixels(pixels, stride, 0);
            });
            byte[] mirroredPixels = new byte[height * stride];

            for (int y = 0; y < height; y++)
            {
                int sourceRow = y * stride;
                int targetRow = (height - 1 - y) * stride;

                for (int x = 0; x < width; x++)
                {
                    int sourceIndex = sourceRow + x * bytesPerPixel;
                    int targetIndex = targetRow + x * bytesPerPixel;

                    for (int b = 0; b < bytesPerPixel; b++)
                    {
                        mirroredPixels[targetIndex + b] = pixels[sourceIndex + b];
                    }
                }
            }

            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                writable.WritePixels(new Int32Rect(0, 0, width, height), mirroredPixels, stride, 0);
                ImageAfterOperation2.Source = writable;
                ImageAfterOperation2.Visibility = Visibility.Visible;
            });
        }

        private void threshold(byte thresholdValue = 128)
        {
            int width = 0, height = 0, stride = 0, bytesPerPixel = 0;
            byte[] pixels = new byte[height * stride];
            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                width = writable.PixelWidth;
                height = writable.PixelHeight;

                // bytes per row
                stride = writable.BackBufferStride;
                bytesPerPixel = (writable.Format.BitsPerPixel + 7) / 8;
                pixels = new byte[height * stride];

                writable.CopyPixels(pixels, stride, 0);
            });

            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                byte blue = pixels[i];
                byte green = pixels[i + 1];
                byte red = pixels[i + 2];
                byte brightness = (byte)((red + green + blue) / 3);

                byte value = (brightness > thresholdValue) ? (byte)255 : (byte)0;

                pixels[i] = value;
                pixels[i + 1] = value;
                pixels[i + 2] = value;
            }

            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                writable.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
                ImageAfterOperation3.Source = writable;
                ImageAfterOperation3.Visibility = Visibility.Visible;
            });
        }
        private void grayscale()
        {
            int width = 0, height = 0, stride = 0, bytesPerPixel = 0;
            byte[] pixels = new byte[height * stride];
            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                width = writable.PixelWidth;
                height = writable.PixelHeight;

                // bytes per row
                stride = writable.BackBufferStride;
                bytesPerPixel = (writable.Format.BitsPerPixel + 7) / 8;
                pixels = new byte[height * stride];

                writable.CopyPixels(pixels, stride, 0);
            });

            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                byte blue = pixels[i];
                byte green = pixels[i + 1];
                byte red = pixels[i + 2];
                byte brightness = (byte)((red + green + blue) / 3);

                pixels[i] = brightness;
                pixels[i + 1] = brightness;
                pixels[i + 2] = brightness;
            }

            Dispatcher.Invoke(() =>
            {
                WriteableBitmap writable = new WriteableBitmap(img);
                writable.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
                ImageAfterOperation4.Source = writable;
                ImageAfterOperation4.Visibility = Visibility.Visible;
            });
        }
    }
}

