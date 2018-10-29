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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CompleetKassa.Views.Modules;
using MahApps.Metro.Controls;

namespace CompleetKassa.Views
{
    /// <summary>
    /// Interaction logic for LockView.xaml
    /// </summary>
    public partial class LockView : UserControl
    {
        public LockView()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        string BG_image1 = "pack://application:,,,/CompleetKassa;component/Icons/LockBackground/BG1.png";
        string BG_image2= "pack://application:,,,/CompleetKassa;component/Icons/LockBackground/BG2.png";
        string BG_image3 = "pack://application:,,,/CompleetKassa;component/Icons/LockBackground/BG3.png";

        int timing = 0;
        int bg_order = 1;
        void timer_Tick(object sender, EventArgs e)
        {
            timing++;

            TimeTxt.Text = DateTime.Now.ToString("HH:mm dddd dd MMMM yyyy");

            //Change background every 30 secs
            if (timing == 1000)
            {
                if (bg_order == 1)
                {
                    BGImage.Source= new BitmapImage(new Uri(BG_image2));
                    bg_order = 2;

                }
                else if (bg_order == 2)
                {
                    BGImage.Source = new BitmapImage(new Uri(BG_image3));
                    bg_order = 3;
                }
                else if (bg_order == 3)
                {
                    BGImage.Source = new BitmapImage(new Uri(BG_image1));
                    bg_order = 1;

                }
                timing = 0;

            }

        }
        private void pinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {


            var button = (Button)sender;
            String txtName = "content" + button.Name.Replace("pin", "");
            String txtImage = "black" + button.Name.Replace("pin", "");


            //#363838
            var bc = new BrushConverter();
            button.Background = (Brush)bc.ConvertFrom("#C6C6C6");

         TextBlock txtContent = button.FindChild<TextBlock>(txtName);

            txtContent.Visibility = Visibility.Hidden;

            Image blackImage = button.FindChild<Image>(txtImage);

           blackImage.Visibility = Visibility.Visible;

        }

        private void LockOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //LockPage.Visibility = Visibility.Hidden;
            
        }

    }
}
