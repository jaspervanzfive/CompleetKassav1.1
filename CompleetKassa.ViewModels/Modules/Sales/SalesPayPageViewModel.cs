using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CompleetKassa.Models;
using CompleetKassa.ViewModels.Commands;

namespace CompleetKassa.ViewModels
{
    public class SalesPayPageViewModel : BaseViewModel
    {
     
     

        public string _orderName="";
        public string OrderName
        {
            get { return _orderName; }
            set
            {
                _orderName = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnPrintReceipt { get; private set; }
     
        public SalesPayPageViewModel(string order) : base("Lock", "#1F2E3C", "Icons/lock.png")
        {
            OrderName = order;


            OnPrintReceipt = new BaseCommand(PrintReceipt);
        }



        public void PrintReceipt(object obj)
        {
            MessageBox.Show(OrderName);
           



            //foreach (var item in svm.PurchasedProducts)
            //{

            //    MessageBox.Show(item.Name.ToString());
            //}
        }

    }
}
