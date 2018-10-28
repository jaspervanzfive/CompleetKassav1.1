using CompleetKassa.ViewModels;
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
    /// Interaction logic for SalesPayScreen.xaml
    /// </summary>
    public partial class SalesPayScreen : UserControl
    {
        public SalesPayScreen()
        {
          

            InitializeComponent();
        
        }

        // SalesView ds = new SalesView();
        private void Button_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {


            SalesPaymentNumpad aw = new SalesPaymentNumpad();
            aw.Message();

            //var dlg = new PrintDialog();
            //dlg.
            //dlg.PrintVisual(Receiptx, "s");


            //var dlg = new PrintDialog();
            //dlg.PrintVisual(Receiptx, "s");
            //((SalesViewModel)DataContext).ClosePayScreen(null);

        }
    }
}
