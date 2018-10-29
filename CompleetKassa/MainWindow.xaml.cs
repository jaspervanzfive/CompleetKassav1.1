using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using CompleetKassa.Views.Modules;
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
            
        }
      


        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                e.Handled = true;
            }
           
          

        }



      
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // Console.WriteLine(viwbox.ActualHeight +" wd"+viwbox.ActualWidth);
        }
    }
}
