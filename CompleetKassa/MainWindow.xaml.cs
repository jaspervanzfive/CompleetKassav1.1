using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using CompleetKassa.Views.Modules.Login;
using System.Windows.Threading;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CompleetKassa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //LoginView page = new LoginView();

            //FirstPage.Content = page; 

            //Time and Date timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            TimeTxt.Text = DateTime.Now.ToString("HH:mm dddd dd MMMM yyyy");
         
        }


        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                e.Handled = true;
            }
            if (e.Key == Key.NumPad6)
            {
                LockPage.Visibility = Visibility.Visible;
            }
          

        }



        #region Login Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginPage.Visibility = Visibility.Hidden;
        }

        private void PasswordForgotten_Event(object sender, MouseButtonEventArgs e)
        {
            EmailBorder.Visibility = Visibility.Visible;
        }

        private void BacktoLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EmailBorder.Visibility = Visibility.Hidden;
            SecretQuestionPage.Visibility = Visibility.Hidden;
        }

        private void RevealPassEvent(object sender, MouseButtonEventArgs e)
        {
            RevealPasswordPage.Visibility = Visibility.Visible;
        }

        private void RevealPassPage(object sender, MouseButtonEventArgs e)
        {
            SecretQuestionPage.Visibility = Visibility.Visible;
        }

        private void Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginPage.Visibility = Visibility.Hidden;
        }
        #endregion



        private void pinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
           

            var button = (Button)sender;
            String txtName = "content"+button.Name.Replace("pin","");
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
            LockPage.Visibility = Visibility.Hidden;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // Console.WriteLine(viwbox.ActualHeight +" wd"+viwbox.ActualWidth);
        }
    }
}
