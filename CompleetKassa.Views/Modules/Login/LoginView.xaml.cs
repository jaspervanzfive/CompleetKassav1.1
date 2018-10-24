using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CompleetKassa.Views.Modules.Login
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
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
    }
}
