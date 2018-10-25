using System.Windows.Controls;
using System.Windows;
using CompleetKassa.ViewModels;
using System.Diagnostics;
using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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


        private void Quantity_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void Button_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine(col1.ActualWidth + " " + col2.ActualWidth);

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

                if(button_name.Equals("DeleteButton"))
                    ButtonOpenDelete.Command.Execute(ButtonOpenDelete.CommandParameter);
                else if (button_name.Equals("ButtonQuantitySelection"))
                    ButtonQuantityLongPress.Command.Execute(ButtonQuantityLongPress.CommandParameter);
                else
                    ButtonSelectAll.Command.Execute(ButtonSelectAll.CommandParameter);
            }

        }

        string button_name= "";
   
        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        

            var button = (Button)sender;
            button_name = button.Name.ToString();


            time = 0;
            dt.Start();




        }

        private void Button_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dt.Stop();
            if (time < 1)
            {
                if (button_name.Equals("DeleteButton"))
                    ButtonDelete.Command.Execute(ButtonDelete.CommandParameter);
                else if (button_name.Equals("ButtonQuantitySelection"))
                    ButtonQuantityClick.Command.Execute(ButtonQuantityClick.CommandParameter);
                else
                    ButtonSelectMultiple.Command.Execute(ButtonSelectMultiple.CommandParameter);


            }




        }


        //For LongTouch Purposes
        private void DeleteButton_StylusDown(object sender, StylusDownEventArgs e)
        {
            

            var button = (Button)sender;
            button_name = button.Name.ToString();

            time = 0;
            dt.Start();


        }

        private void DeleteButton_StylusUp(object sender, StylusEventArgs e)
        {
            dt.Stop();
            if (time < 1)
            {

                if (button_name.Equals("DeleteButton"))
                    ButtonDelete.Command.Execute(ButtonDelete.CommandParameter);
                else if (button_name.Equals("ButtonQuantitySelection"))
                    ButtonQuantityClick.Command.Execute(ButtonQuantityClick.CommandParameter);
                else
                    ButtonSelectMultiple.Command.Execute(ButtonSelectMultiple.CommandParameter);


            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
