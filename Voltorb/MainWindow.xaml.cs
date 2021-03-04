using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Voltorb
{
    public partial class MainWindow : Window
    {
        //private string totalXml;
        RegistryKey key;
        const string defaultMessage = "¡ {0} !";

        ImageAnimationController controller;

        public MainWindow()
        {
            InitializeComponent();

            //Lector de puntos
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\XMLTracerKiller", true);

            if (key.ValueCount == 0)
            {
                key.SetValue("counter", 0);
            }

            decimal counterValue = decimal.Parse(key.GetValue("counter").ToString());

            setTitle(counterValue);

            //Drag and drop
            Point startPosition = new Point();
            this.PreviewMouseRightButtonDown += (sender, e) =>
            {
                startPosition = e.GetPosition(this);
            };

            this.PreviewMouseMove += (sender, e) =>
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    Point endPosition = e.GetPosition(this);
                    Vector vector = endPosition - startPosition;
                    this.Left += vector.X;
                    this.Top += vector.Y;
                }
            };
        }

        private void image_completed(object sender, RoutedEventArgs e)
        {            
            controller = ImageBehavior.GetAnimationController(explosionGif);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                int sum = 0;
                Process[] runningProcesses = Process.GetProcesses();
                foreach (Process process in runningProcesses)
                {
                    if (process.ProcessName.Equals("xmlmarker"))
                    {
                        process.Kill();
                        sum++;
                    }
                }
                int sumTotalKills = int.Parse(key.GetValue("counter").ToString()) + sum;
                key.SetValue("counter", sumTotalKills);

                setTitle(sumTotalKills);

                gifPlay();
            }
        }

        private void gifPlay()
        {
            controller.GotoFrame(0);
            controller.Play();
        }

        private void setTitle(decimal newValor)
        {
            this.Title = string.Format(defaultMessage, newValor);
        }
    }
}
