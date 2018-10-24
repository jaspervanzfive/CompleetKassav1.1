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
using CompleetKassa.Views.Modules.Login;



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
        void timer_Tick(object sender, EventArgs e)
        {
            TimeTxt.Text = DateTime.Now.ToString("HH:mm dddd dd MMMM yyyy");

        }
        private void pinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {


            var button = (Button)sender;
            String txtName = "content" + button.Name.Replace("pin", "");
            String txtImage = "black" + button.Name.Replace("pin", "");


            //#363838
            var bc = new BrushConverter();
            button.Background = (Brush)bc.ConvertFrom("#C6C6C6");

           // TextBlock txtContent = button.FindChild<TextBlock>(txtName);

         //   txtContent.Visibility = Visibility.Hidden;

            //Image blackImage = button.FindChild<Image>(txtImage);

          //  blackImage.Visibility = Visibility.Visible;

        }

        private void LockOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //LockPage.Visibility = Visibility.Hidden;
            
        }

    }
}
