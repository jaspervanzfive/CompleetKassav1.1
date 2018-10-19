using CompleetKassa.Views.Modules.Customers;
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

namespace CompleetKassa.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();
        }

        private void AddPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomerViewAddPage page = new CustomerViewAddPage();
            SidePage.Content = page;
            AddButton.BorderThickness = new Thickness(1, 1, 1, 1);
            BulkButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void BulkPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomerViewBulkPage page = new CustomerViewBulkPage();
            SidePage.Content = page;
            BulkButton  .BorderThickness = new Thickness(1, 1, 1, 1);
            AddButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }
    }
}
