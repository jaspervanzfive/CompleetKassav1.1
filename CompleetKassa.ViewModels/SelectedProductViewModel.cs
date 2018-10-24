using System;
using System.Collections.ObjectModel;
using System.Windows;
using CompleetKassa.DataTypes.Enumerations;

namespace CompleetKassa.ViewModels
{
    public class SelectedProductViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

      


		private string _discountOption;
		public string DiscountOption
		{
			get
			{
				return _discountOption;
			}

			set { SetProperty (ref _discountOption, value); }
		}

		private decimal _subTotal;
        public decimal SubTotal
        {
            get
            {
                return _subTotal;
            }

            set { SetProperty(ref _subTotal, value); }
        }

        private decimal _origTotal;
        public decimal OrigTotal
        {
            get
            {
                return _origTotal;
            }

            set { SetProperty(ref _origTotal, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }

            set {
                SetProperty(ref _quantity, value);
                ComputeSubTotal();
            }
        }

        private decimal _discount;
        public decimal Discount
        {
            get { return _discount; }

            set
            {
                SetProperty(ref _discount, value);
               // ComputeSubTotal();
            }
        }

        private int _discountPercentage;
        public int DiscountPercentage
        {
            get { return _discountPercentage; }

            set
            {

                SetProperty(ref _discountPercentage, value);
             //   ComputeSubTotal();
            }
        }


        private string _discountText = "";
        public string DiscountText
        {
            get { return _discountText; }
            set
            {
                SetProperty(ref _discountText, value);
            }
        }


        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
 
        public SelectedProductViewModel() : base(string.Empty, string.Empty, string.Empty)
        {
            DiscountOption = string.Empty;
            ID = 0;
            Name = string.Empty;
            Price = 0.0m;
            Quantity = 0;
            Discount = 0.0m;
            DiscountPercentage = 0;

            IsSelected = false;
            DiscountVisibility = Visibility.Collapsed;
        
         

        }

        public void ComputeSubTotal()
        {
            ComputeDiscount();



            OrigTotal = Price * Quantity;

            
            SubTotal = OrigTotal - Discount;


            //SubTotal = (Price - Discount) * Quantity;
        }

        private Visibility _discountVisibility;
        public Visibility DiscountVisibility
        {
            get { return _discountVisibility; }
            set
            {
                SetProperty(ref _discountVisibility, value);
            }
        }

        public void ComputeDiscount()
        {
             
           

            if (DiscountOption == "Dollar")
            {
                Discount = Discount;

                DiscountText = "Discount Dollar";
            }
            else if (DiscountOption == "Percent")
            {

                Discount = (Price * Quantity) * (Convert.ToDecimal(DiscountPercentage) / 100.0m);
                DiscountText = "Discount " + DiscountPercentage.ToString()+"%";
            }
         

            ShowDiscount();


        }

        public void ShowDiscount()
        {
            if (Discount > 0 || DiscountPercentage>0)
                DiscountVisibility = Visibility.Visible;
            else
                DiscountVisibility = Visibility.Collapsed;

        }
    }
}
