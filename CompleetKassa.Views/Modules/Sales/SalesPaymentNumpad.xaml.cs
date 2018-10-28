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

namespace CompleetKassa.Views.Modules.Sales
{
    /// <summary>
    /// Interaction logic for SalesPaymentNumpad.xaml
    /// </summary>
    public partial class SalesPaymentNumpad : UserControl
    {
        public SalesPaymentNumpad()
        {
            InitializeComponent();
        }

        public void Message()
        {
            MessageBox.Show(gagi.Text);
        }
    }
}
