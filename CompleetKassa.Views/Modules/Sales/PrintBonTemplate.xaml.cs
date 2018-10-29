using CompleetKassa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for PrintBonTemplate.xaml
    /// </summary>
    public partial class PrintBonTemplate : UserControl
    {
        public PrintBonTemplate()
        {
            InitializeComponent();
        }

        public void PrintBon()
        {
            var dlg = new PrintDialog();
           // dlg.PrintQueue = new PrintQueue(new PrintServer(), "Microsoft Print to PDF");
            dlg.PrintVisual(Grid1x, "s");

            ((SalesViewModel)DataContext).ClosePayScreen(null);
            ((SalesViewModel)DataContext).Pay(null);


        }
    }
}
