using CompleetKassa.Views.Modules.Products;
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
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
        }

        private void AddPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProductsViewAdd page = new ProductsViewAdd();
            SidePage.Content = page;

            AddButton.BorderThickness = new Thickness(1, 1, 1, 1);
            CategoryButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CategoryPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
        
            CategoryButton.BorderThickness = new Thickness(1, 1, 1, 1);
            AddButton.BorderThickness = new Thickness(0, 0, 0, 0);
        }
    }
}
