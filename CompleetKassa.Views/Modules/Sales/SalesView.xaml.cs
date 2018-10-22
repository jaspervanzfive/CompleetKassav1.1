using System.Windows.Controls;
using System.Windows;
using CompleetKassa.ViewModels;
using System.Diagnostics;
using System;
using System.Windows.Threading;

namespace CompleetKassa.Views
{
    /// <summary>
    /// Interaction logic for ShoesView.xaml
    /// </summary>
    public partial class SalesView : UserControl
    {
        SalesViewModel ds = new SalesViewModel();

        public SalesView()
        {
            InitializeComponent();

            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
        }

        private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
          
        }

        private void Quantity_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void Button_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
      
        }


        //Timer for long press in delete button
        DispatcherTimer dt;
        int time = 0;
        private void dtTicker(object sender,EventArgs e)
        {
            time++;

            if (time>=1)
            {

                dt.Stop();

                ButtonOpenDelete.Command.Execute(ButtonOpenDelete.CommandParameter);
            }

        }
   
        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            time = 0;
            dt.Start();


         

        }

        private void Button_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dt.Stop();
            if (time<1)
            {
               
                ButtonDelete.Command.Execute(ButtonDelete.CommandParameter);
            }




        }

        private void Button_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            time = 0;
            dt.Start();
        }

        private void Button_TouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            dt.Stop();
            if (time < 1)
            {

                ButtonDelete.Command.Execute(ButtonDelete.CommandParameter);
            }
        }
    }
}
