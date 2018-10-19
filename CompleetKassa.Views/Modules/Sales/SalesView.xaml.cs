using System.Windows.Controls;
using System.Windows;
using CompleetKassa.ViewModels;
using System.Diagnostics;
using System;

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
        Stopwatch stopwatch;
        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //stopwatch = new Stopwatch();
            //stopwatch.Start();


            //Console.WriteLine(stopwatch.Elapsed.ToString());
            
            //if (stopwatch.ElapsedMilliseconds >= 1000)
            //{
            //    ds.TryButton2();
            //}
            //else
            //{
            //    ds.TryButton();

            //    // do LongClick
            //}
            //stopwatch.Stop();

        }

        private void Button_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            





        }
    }
}
