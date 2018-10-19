using System.Collections.ObjectModel;
using System.Windows;
using CompleetKassa.DataTypes.Enumerations;

namespace CompleetKassa.ViewModels
{
    public class SelectedProductViewModel : BaseViewModel
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }

      


		private ProductDiscountOptions _discountOption;
		public ProductDiscountOptions DiscountOption
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
                ComputeSubTotal();
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
            ID = 0;
            Label = string.Empty;
            Price = 0.0m;
            Quantity = 0;
            Discount = 0.0m;

            IsSelected = false;
            DiscountVisibility = Visibility.Collapsed;
        }

		public void ComputeSubTotal()
        {

            ShowDiscount();

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
        public void ShowDiscount()
        {
            if (Discount > 0)
                DiscountVisibility = Visibility.Visible;
            else
                DiscountVisibility = Visibility.Collapsed;

        }
    }
}
