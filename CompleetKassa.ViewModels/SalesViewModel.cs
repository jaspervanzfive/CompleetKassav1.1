using CompleetKassa.Models;
using CompleetKassa.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using CompleetKassa.DataTypes.Enumerations;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;


namespace CompleetKassa.ViewModels
{
    public class SalesViewModel : BaseViewModel
    {
        private IList<ProductModel> _dbProductList;


        
        private ObservableCollection<ProductCategoryModel> _categories;
        private ObservableCollection<ProductSubCategoryModel> _subCategories;

		public string DiscountValue { get; set; }

		// Multi receipt
		private ObservableCollection<PurchasedProductViewModel> _receiptList;
        public ObservableCollection<PurchasedProductViewModel> ReceiptList
        {
            get
            {
                return _receiptList;
            }
            set { SetProperty(ref _receiptList, value); }
        }

        private string _categoryFilter;
        public string CategoryFilter
        {
            get { return _categoryFilter; }
            set
            {
                SetProperty(ref _categoryFilter, value);
                ProductList.Refresh();
            }
        }

        private string _subCategoryFilter;
        public string SubCategoryFilter
        {
            get { return _subCategoryFilter; }
            set
            {
                SetProperty(ref _subCategoryFilter, value);
                ProductList.Refresh();
            }
        }

       





        private ProductSubCategoryModel _selectedSubCategory;
        public ProductSubCategoryModel SelectedSubCategory
        {
            get { return _selectedSubCategory; }
            set {
                if (value != null)
                {
                    _selectedSubCategory = value;
                    SubCategoryFilter = value.Name;
                  
                }
            }
        }

        private ProductCategoryModel _selectedCategory;
        public ProductCategoryModel SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                CategoryFilter = value.Name;
                SetSubCategories(value.Name);
               
            }
        }
     
        

        private ICollectionView _productList;
        public ICollectionView ProductList
        {
            get { return _productList; }
            set { SetProperty(ref _productList, value); }
        }

        private ObservableCollection<SelectedProductViewModel> _purchasedProducts;
        public ObservableCollection<SelectedProductViewModel> PurchasedProducts
        {
            get
            {
                return _purchasedProducts;
            }
            set { SetProperty(ref _purchasedProducts, value); }
        }

        public ObservableCollection<ProductCategoryModel> Categories
        {
            get { return _categories; }
            set
            {
                SetProperty(ref _categories, value);
            }
        }

        public ObservableCollection<ProductSubCategoryModel> SubCategories
        {
            get { return _subCategories; }
            set
            {
                SetProperty(ref _subCategories, value);
            }
        }

        private PurchasedProductViewModel _currentPurchase;
        public PurchasedProductViewModel CurrentPurchase
        {
            get { return _currentPurchase; }
            set {
                SetProperty(ref _currentPurchase, value);
                PurchasedProducts = value.Products;
            }
        }

        private int _receiptIndex;
        public int ReceiptIndex
        {
            get { return _receiptIndex; }
            set {
                if (value < 0) return;

                SetProperty(ref _receiptIndex, value);
                CurrentPurchase = _receiptList[value];
            }
        }

       

        private SelectedProductViewModel _selectedPurchasedProduct;
        public SelectedProductViewModel SelectedPurchasedProduct
        {
            get { return _selectedPurchasedProduct; }
            set {
                
                if (value !=null)
                {
                    _selectedPurchasedProduct = value;

                    QuantityPop = _selectedPurchasedProduct.Quantity;


                  


                    if (_selectedPurchasedProduct.DiscountOption == "Percent")
                    {
                        DiscountImagePath = DollarSignPath;
                        DiscountPercentText = Visibility.Visible;
                        DiscountDollarText = Visibility.Hidden;
                    }
                   
                    else if (_selectedPurchasedProduct.DiscountOption == "Dollar")
                    {
                        DiscountImagePath = DiscountImagePath;
                        DiscountPercentText = Visibility.Hidden;
                        DiscountDollarText = Visibility.Visible; 
                    }
                       


                    DiscountDollar = _selectedPurchasedProduct.Discount;
                    DiscountPercentage = _selectedPurchasedProduct.DiscountPercentage;


                    //decimal Calculation = (_selectedPurchasedProduct.Discount / _selectedPurchasedProduct.Price) * 100;

                    // DiscountPercentage = (int)Calculation;



                }
                
             
            }
        }

        #region Commands
        public ICommand OnPurchased { get; private set; }
        public ICommand OnIncrementPurchased { get; private set; }
        public ICommand OnDecrementPurchased { get; private set; }
        public ICommand OnSelectAllPurchased { get; private set; }
        public ICommand OnNewReceipt { get; private set; }
        public ICommand OnPreviousReceipt { get; private set; }
        public ICommand OnNextReceipt { get; private set; }
        public ICommand OnPay { get; private set; }
        public ICommand OnDiscountDollar { get; private set; }
		public ICommand OnDiscountPercent { get; private set; }
        public ICommand OnDeleteProducts { get; private set; }

        public ICommand OnPrintReceipt { get; private set; }



        //Quantity Numpad Commands
        public ICommand OnNumpadPurchased { get; private set; }
        public ICommand OnNumpadNumPressed { get; private set; }
        public ICommand OnNumpadCleared { get; private set; }
        public ICommand OnNumpadBackspace { get; private set; }
        public ICommand OnNumpadOpen { get; private set; }



        //Quantity Pop Commands
        public ICommand OnIncrementQuantityPop { get; private set; }
        public ICommand OnDecrementQuantityPop { get; private set; }
        public ICommand OnQuantityPopOpen { get; private set; }
        public ICommand OnApplyQuantityPop { get; private set; }
        public ICommand OnSelectQuantityOption { get; private set; }
        public ICommand OnSelectQuantityOptionLongPress { get; private set; }


        //Discount Pop Commands
        public ICommand OnIncrementDiscountPop { get; private set; }
        public ICommand OnDecrementDiscountPop { get; private set; }
        public ICommand OnDiscountPopOpen { get; private set; }
        public ICommand OnDiscountChangeButton { get; private set; }
        public ICommand OnApplyDiscountPop { get; private set; }

        public ICommand OnPurchaseSelectMultiple { get; private set; }


        //Delete Commands
        public ICommand OnDeleteOpen { get; private set; }
        public ICommand OnDeleteWholeOrder { get; private set; }
        public ICommand OnPurchaseSelectAll { get; private set; }
        public ICommand OnSetDollarDiscount { get; private set; }

        //PayScreen Commands
        public ICommand OnOpenPayScreen { get; private set; }
        public ICommand OnClosePayScreen { get; private set; }

        public ICommand OnPaymentOption { get; private set; }
     

        public ICommand OnPayScreenSideButtons { get; private set; }
        public ICommand OnEmailSubmit { get; private set; }
        public ICommand OnPaymentNumpad { get; private set; }
        public ICommand OnReceiptSideButtons { get; private set; }

        public ICommand OnPredictionPressed { get; private set; }
    
        #endregion




        public SalesViewModel() : base ("Sales", "#FDAC94","Icons/product.png")
		{



            //PurchasedItems = new ObservableCollection<PurchasedProductViewModel>();
            _categories = new ObservableCollection<ProductCategoryModel>();
            _purchasedProducts = new ObservableCollection<SelectedProductViewModel>();
            _receiptList = new ObservableCollection<PurchasedProductViewModel>();
           

            _customer = new ObservableCollection<CustomerModel>();


            _categoryFilter = string.Empty;
            _subCategoryFilter = string.Empty;

            // TODO: This is where to get data from DB
            GetProducts();
            ProductList = CollectionViewSource.GetDefaultView(_dbProductList);
            ProductList.Filter += ProductCategoryFilter;
            ProductList.Filter += ProductSubCategoryFilter;

           
            
            
            
            //Customers List
            GetCustomers();
            CustomerBinding = CollectionViewSource.GetDefaultView(_customer);
            CustomerBinding.Filter = CustomerFilter;
           

            // Set the first product as active category
            _categoryFilter = _categories.FirstOrDefault() == null ? string.Empty : _categories.FirstOrDefault().Name;
            SetSubCategories(_categoryFilter);
            SelectFirstCategory();

            

            CreateNewReceipt(null);

            // Commands
            OnPurchased = new BaseCommand(Puchase);
            OnIncrementPurchased = new BaseCommand(IncrementPurchase);
            OnDecrementPurchased = new BaseCommand(DecrementPurchase);
            OnSelectAllPurchased = new BaseCommand(SelectAllPurchased);
            OnNewReceipt = new BaseCommand(AddOrNextReceipt);
            OnPreviousReceipt = new BaseCommand(SelectPreviousReceipt);
            OnNextReceipt = new BaseCommand(SelectNextReceipt);
            OnPay = new BaseCommand(Pay);
            OnDiscountDollar = new BaseCommand(DiscountPurchaseByDollar);
			OnDiscountPercent = new BaseCommand (DiscountPurchaseByPercent);
            OnDeleteProducts = new BaseCommand(DeleteProducts);

            //Print
            OnPrintReceipt = new BaseCommand(PrintReceipt);

            //Numpad
            OnNumpadPurchased = new BaseCommand(NumpadPurchased);
            OnNumpadNumPressed = new BaseCommand(NumpadNumPressed);
            OnNumpadCleared = new BaseCommand(NumpadCleared);
            OnNumpadBackspace = new BaseCommand(NumpadBackspace);
            OnNumpadOpen = new BaseCommand(NumpadOpen);
            NumpadVisibility = Visibility.Hidden;


            //QuantityPop
            OnIncrementQuantityPop = new BaseCommand(IncrementQuantityPop);
            OnDecrementQuantityPop = new BaseCommand(DecrementQuantityPop);
            OnApplyQuantityPop = new BaseCommand(ApplyQuantityPop);
            OnQuantityPopOpen = new BaseCommand(QuantityPopOpen);
            OnSelectQuantityOption = new BaseCommand(SelectQuantityOption);
            OnSelectQuantityOptionLongPress = new BaseCommand(SelectQuantityOptionLongPress);
     
            QuantityPopVisibility = Visibility.Collapsed;
            QuantityPop = 1;
            QuantityPopRow = 0;


            //DiscountPop
            OnIncrementDiscountPop = new BaseCommand(IncrementDiscountPop);
            OnDecrementDiscountPop = new BaseCommand(DecrementDiscountPop);
            OnApplyDiscountPop = new BaseCommand(ApplyDiscountPop);
            OnDiscountPopOpen = new BaseCommand(DiscountPopOpen);
            OnDiscountChangeButton = new BaseCommand(DiscountChangeButton);
            DiscountPopVisibility = Visibility.Collapsed;
            DiscountPopRow = 1;
            DiscountPercentText = Visibility.Visible;
            DiscountDollarText = Visibility.Hidden;
            DiscountImagePath = DollarSignPath;
            DiscountDollar = 1;

            OnPurchaseSelectMultiple = new BaseCommand(PurchaseSelectMultiple);
            SelectionMode = SelectionMode.Extended;
            SelectionColor = (Brush)(new BrushConverter().ConvertFrom("#999999"));


            //DeleteOrder
            OnDeleteOpen = new BaseCommand(DeleteOpen);
            OnDeleteWholeOrder = new BaseCommand(DeleteWholeOrder);


            OnPurchaseSelectAll = new BaseCommand(SelectAllPurchased);



            //PayScreen Commands
            OnOpenPayScreen = new BaseCommand(OpenPayScreen);
            OnClosePayScreen = new BaseCommand(ClosePayScreen);
            OnPayScreenSideButtons = new BaseCommand(PayScreenSideButtons);
            OnPaymentOption = new BaseCommand(PaymentOption);
            OnEmailSubmit = new BaseCommand(EmailSubmit);

            OnPaymentNumpad = new BaseCommand(PaymentNumpad);

            OnReceiptSideButtons = new BaseCommand(ReceiptSideButtons);
            OnPredictionPressed = new BaseCommand(PredictionPressed);

            // OnSetDollarDiscount = new BaseCommand(SetDollarDiscount);

        }


        #region Customer List and methods


        //Bind client search text in the payscreen
        private string _customerSearchText = string.Empty;
        public string CustomerSearchText
        {
            get { return _customerSearchText; }
            set
            {

                string _value = value as string;



                SetProperty(ref _customerSearchText, value);
             



                if (string.IsNullOrWhiteSpace(_value))
                {
                    PayScreenClientBoxVisibility = Visibility.Collapsed;

                }
                else
                { 
                    PayScreenClientBoxVisibility = Visibility.Visible;
                    CustomerBinding.Refresh();
                }
                    



            }
        }

        public ICollectionView CustomerBinding { get; private set; }

        private ObservableCollection<CustomerModel> _customer;

        public ObservableCollection<CustomerModel> Customer
        {
            get { return _customer; }
            set
            {
                SetProperty(ref _customer, value);

            }
        }


        private void GetCustomers()
        {
            _customer.Add(new CustomerModel
            {
                Name = "Roger Vanz",
                Email = "R.Vanz@gmail.com"
            }
            );
            _customer.Add(new CustomerModel
            {
                Name = "Roger Rakker",
                Email = "RororoKer@gmail.com"
            }
           );
            _customer.Add(new CustomerModel
            {
                Name = "Roger van Hamels",
                Email="Roger.Van@hotmail.com"
            }
           );
            _customer.Add(new CustomerModel
            {
                Name = "Roger Van Zanz",
                Email="iamrogervanz@gmail.com"
            }
           );
            _customer.Add(new CustomerModel
            {
                Name = "Stephen Curry",
                Email = "curry.stephen@gmail.com"
            }
           );
            _customer.Add(new CustomerModel
            {
                Name = "Kyle Kuzma",
                Email = "kuzma.kyle@gmail.com"
            }
           );
            _customer.Add(new CustomerModel
            {
                Name = "Lonzo Ball",
                Email = "ballerslonzo@gmail.com"
            }
           );

        }

        private bool CustomerFilter(object item)
        {
            var cust = item as CustomerModel;
            return (cust.Name.ToLower().Contains(_customerSearchText.ToLower()));

        }

        private CustomerModel _selectedCustomer;
        public CustomerModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (value != null)
                {
                    _selectedCustomer = value;


                    CurrentPurchase.ClientName= _selectedCustomer.Name;
                    CustomerSearchText = string.Empty;
                    SelectedCustomer = null;

                    HidePayScreenComponents();
                    ClearSideButtonsSelection();
                    PayClientTextVisibility = Visibility.Visible;
                }

                   

            }
        }


        #endregion


        #region Payment


        #region Payments Prediction in cash

        private void SetPrediction()
        {
           
            if (BoolIsCash)
            {
                PredictionVisibility = Visibility.Visible;
                if (PayScreenPaymentsVisibility == Visibility.Collapsed)
                {
                
                    Prediction1 = Math.Round(CurrentPurchase.Due, 0, MidpointRounding.AwayFromZero);
                    Prediction2 = Math.Round(CurrentPurchase.Due+10, 0, MidpointRounding.AwayFromZero);
                    Prediction3 = Math.Round(CurrentPurchase.Due+20, 0, MidpointRounding.AwayFromZero);
                    Prediction4 = Math.Round(CurrentPurchase.Due+30, 0, MidpointRounding.AwayFromZero);
                    Prediction5 = Math.Round(CurrentPurchase.Due+40, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    Prediction1 = Math.Round(PaymentSecondValue, 0, MidpointRounding.AwayFromZero);
                    Prediction2 = Math.Round(PaymentSecondValue+10, 0, MidpointRounding.AwayFromZero);
                    Prediction3 = Math.Round(PaymentSecondValue+20, 0, MidpointRounding.AwayFromZero);
                    Prediction4 = Math.Round(PaymentSecondValue+30, 0, MidpointRounding.AwayFromZero);
                    Prediction5 = Math.Round(PaymentSecondValue+40, 0, MidpointRounding.AwayFromZero);
                }
            }
            else
               PredictionVisibility = Visibility.Hidden;


        }

        private Visibility _predictionVisibility = Visibility.Hidden;
        public Visibility PredictionVisibility
        {
            get { return _predictionVisibility; }
            set
            {
                SetProperty(ref _predictionVisibility, value);
            }
        }





        public decimal _prediction1;
        public decimal Prediction1
        {
            get { return _prediction1; }
            set
            {
                _prediction1 = value;
                OnPropertyChanged();
            }
        }

        public decimal _prediction2;
        public decimal Prediction2
        {
            get { return _prediction2; }
            set
            {
                _prediction2 = value;
                OnPropertyChanged();
            }
        }
        public decimal _prediction3;
        public decimal Prediction3
        {
            get { return _prediction3; }
            set
            {
                _prediction3 = value;
                OnPropertyChanged();
            }
        }
        public decimal _prediction4;
        public decimal Prediction4
        {
            get { return _prediction4; }
            set
            {
                _prediction4 = value;
                OnPropertyChanged();
            }
        }
        public decimal _prediction5;
        public decimal Prediction5
        {
            get { return _prediction5; }
            set
            {
                _prediction5 = value;
                OnPropertyChanged();
            }
        }
     



        #endregion

        #region PaymentScreen Numpad logics


        //OnPredictionPressed.
        private void PredictionPressed(object obj)
        {
            string _obj = obj as string;

            decimal _value = 0.0m;

            if (_obj.Equals("1"))
                _value = Prediction1;
            else if (_obj.Equals("2"))
                _value = Prediction2;
            else if (_obj.Equals("3"))
                _value = Prediction3;
            else if (_obj.Equals("4"))
                _value = Prediction4;
            else if (_obj.Equals("5"))
                _value = Prediction5;

       

            NumpadPrice = _value;

            if (PayScreenPaymentsVisibility == Visibility.Collapsed)
                PaymentNumpad(null);
            else
                PaymentNumpadSecond();

            NumpadPrice = 0;

        }


        private void PaymentNumpad(object obj)
        {
            

            //This will trigger first payment
            if (PayScreenPaymentsVisibility == Visibility.Collapsed)
                PaymentNumpadFirst();
            //This will trigger second payment
            else if (PayScreenPaymentsVisibility == Visibility.Visible && PaymentSecondValue!=0)
                PaymentNumpadSecond();


            //Hide components
            if (_paymentsGood)
            {
                PayScreenPaymentsVisibility = Visibility.Visible;
                NumpadCleared(null);
            
                ClearPaymentOptions();
            }
        
        }

        private decimal _NumpadCash = 0;
        public decimal NumpadCash
        {
            get { return _NumpadCash; }
            set
            {
                _NumpadCash = value;
                OnPropertyChanged();

            }
        }

        private decimal _NumpadPin = 0;
        public decimal NumpadPin
        {
            get { return _NumpadPin; }
            set
            {
                _NumpadPin = value;
                OnPropertyChanged();

            }
        }


        //If in the second payment , the amount pressed is not less than the DUE!
        bool _paymentsGood = false;
       
        //Method for first payment
        private void PaymentNumpadFirst()
        {
            if (NumpadPrice < CurrentPurchase.Due)
            {
                //Set the value as absolute(always positive)
                decimal _ChangeAfterFirstPay = Math.Abs(CurrentPurchase.Due - NumpadPrice);

                PaymentFirstValue = NumpadPrice;

                //Get the result of the _changeAfterFirstPay to see if it has due.
                PaymentSecondValue = _ChangeAfterFirstPay;
                PaymentSecondText = "Due";

                BoolAllowedToPay = false;

               


            }
            //If the Payment price is higher or equals to the current due
            else
            {
                decimal _Change = NumpadPrice-CurrentPurchase.Due;

                PaymentFirstValue = CurrentPurchase.Due;
                PaymentSecondValue = _Change ;
                PaymentSecondText = "Change";

                NumpadChange = _Change;

                BoolAllowedToPay = true;

              

            }

            //Set which payment option is selected at first point
            if (BoolIsCash)
            {
                PaymentFirstText = "Cash";
                NumpadCash = NumpadPrice;
            }
            else if (BoolIsPin)
            {
                PaymentFirstText = "Pin";
                NumpadPin = PaymentFirstValue;
            }
                


            PaymentNumpadVisibility = Visibility.Hidden;
            _paymentsGood = true;

        }

        private void PaymentNumpadSecond()
        {
            //In the second payment, lesser than the second value is not allowed
            if (NumpadPrice >= PaymentSecondValue)
            {
                //Set the value as absolute(always positive)
                decimal _ChangeAfterSecondPay = Math.Abs(PaymentSecondValue - NumpadPrice);

                PaymentSecondValue = PaymentSecondValue;

                //Set which payment option is selected at second point
                if (BoolIsCash)
                {
                    PaymentNumpadVisibility = Visibility.Visible;
                    NumpadChange = _ChangeAfterSecondPay;
                    PaymentSecondText = "Cash";
                    NumpadCash = NumpadPrice;
                }
                else if (BoolIsPin)
                {
                    PaymentNumpadVisibility = Visibility.Hidden;
                    PaymentSecondText = "Pin";
                    NumpadPin = PaymentSecondValue;
                }
                BoolAllowedToPay = true;

                _paymentsGood = true;
            }
            else
            {
                BoolAllowedToPay = false;
                _paymentsGood = false;
                sounds.Error();
            }

           

        }
        //_paymentValue is the value of the payment and _paymentText is the Type whether its cash or pin

        private decimal _paymentFirstValue;
        public decimal PaymentFirstValue
        {
            get { return _paymentFirstValue; }
            set
            {
                _paymentFirstValue = value;
                OnPropertyChanged();
            }
        }


      

        private Visibility _NumpadChangeVisibility = Visibility.Hidden;
        public Visibility NumpadChangeVisibility
        {
            get { return _NumpadChangeVisibility; }
            set
            {
                SetProperty(ref _NumpadChangeVisibility, value);
            }
        }

        private decimal _NumpadChange =0;
        public decimal NumpadChange
        {
            get { return _NumpadChange; }
            set
            {
                _NumpadChange = value;
                Console.WriteLine(value);
                if (value == 0)
                    NumpadChangeVisibility = Visibility.Hidden;
                else
                    NumpadChangeVisibility = Visibility.Visible;

                OnPropertyChanged();
              
            }
        }

        private decimal _paymentSecondValue;
        public decimal PaymentSecondValue
        {
            get { return _paymentSecondValue; }
            set
            {
                _paymentSecondValue = value;
                OnPropertyChanged();
            }
        }

        private string _paymentFirstText="";
        public string PaymentFirstText
        {
            get { return _paymentFirstText; }
            set
            {
                _paymentFirstText = value;
                OnPropertyChanged();
            }
        }

        private string _paymentSecondText="";
        public string PaymentSecondText
        {
            get { return _paymentSecondText; }
            set
            {
                _paymentSecondText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _payScreenPaymentsVisibility = Visibility.Collapsed;
        public Visibility PayScreenPaymentsVisibility
        {
            get { return _payScreenPaymentsVisibility; }
            set
            {
                SetProperty(ref _payScreenPaymentsVisibility, value);
            }
        }





        #endregion

        //Bool for if you can pay already.
        private bool _boolAllowedToPay = false;
        public bool BoolAllowedToPay
        {
            get { return _boolAllowedToPay; }
            set
            {
                SetProperty(ref _boolAllowedToPay, value);
                if(value==true)
                    PayColor = (Brush)(new BrushConverter().ConvertFrom("#2E4051"));
                else
                    PayColor = (Brush)(new BrushConverter().ConvertFrom("#D6D6D6"));



            }
        }

        private Brush _payColor = (Brush)(new BrushConverter().ConvertFrom("#D6D6D6"));
        public Brush PayColor
        {
            get { return _payColor; }
            set
            {
                SetProperty(ref _payColor, value);
            }
        }

        //On Opening of PayScreen
        private void OpenPayScreen(object item)
        {
            //Due must not 0 to Open the payscreen
            if (CurrentPurchase.Due > 0)
            {
                BoolAllowedToPay = false;

                NumpadVisibility = Visibility.Hidden;


                ClearPaymentOptions();
                HidePayScreenComponents();
                ClearSideButtonsSelection();


                //Need to refractor , hide payClientText if null
                if (!string.IsNullOrEmpty(CurrentPurchase.ClientName))
                    PayClientTextVisibility = Visibility.Visible;
                else
                    PayClientTextVisibility = Visibility.Collapsed;



                PayScreenVisibility = Visibility.Visible;
            }
            else
                sounds.Error();

           


        }
        public void ClosePayScreen(object obj)
        {
            PayScreenVisibility = Visibility.Hidden;
            PaymentNumpadVisibility = Visibility.Hidden;
        }

        private bool _boolPrediction1 = false;
        public bool BoolPrediction1
        {
            get { return _boolPrediction1; }
            set
            {
                SetProperty(ref _boolPrediction1, value);
            }
        }

    
        private bool _boolPrediction2 = false;
        public bool BoolPrediction2
        {
            get { return _boolPrediction2; }
            set
            {
                SetProperty(ref _boolPrediction2, value);
            }
        }

        private bool _boolPrediction3 = false;
        public bool BoolPrediction3
        {
            get { return _boolPrediction3; }
            set
            {
                SetProperty(ref _boolPrediction3, value);
            }
        }

        private bool _boolPrediction4 = false;
        public bool BoolPrediction4
        {
            get { return _boolPrediction4; }
            set
            {
                SetProperty(ref _boolPrediction4, value);
            }
        }
        private bool _boolPrediction5 = false;
        public bool BoolPrediction5
        {
            get { return _boolPrediction5; }
            set
            {
                SetProperty(ref _boolPrediction5, value);
            }
        }

        private void ClearPredictionSelection()
        {
            BoolPrediction1 = false;
            BoolPrediction2 = false;
            BoolPrediction3 = false;
            BoolPrediction4 = false;
            BoolPrediction5 = false;
        }



        public void PaymentOption(object item)
        {
            ClearPredictionSelection();


            SetPrediction();
            //Error means the payment is already done!
            if ((PaymentSecondText.Equals("Change") ||(PaymentSecondValue.Equals(0) && PaymentSecondText.Equals("Due")) ||(PaymentSecondText.Equals("Cash") || PaymentSecondText.Equals("Pin")))  && (PayScreenPaymentsVisibility.Equals(Visibility.Visible)))
            {
                BoolIsCash = false;
                BoolIsPin = false;
                sounds.Error();
            }
            else
            {
                PaymentNumpadVisibility = Visibility.Visible;
            }

            
            
          
        }
        
        private void ReceiptSideButtons(object item)
        {
            OpenPayScreen(null);
            PayScreenSideButtons(item);
        }

        private void PayScreenSideButtons(object item)
        {
            string _value = item as string;
            
            HidePayScreenComponents();

            

            if (_value.Equals("Print"))
            {
                PayScreenPrintVisibility = Visibility.Visible;
                BoolOptionPrint = true;
            }
            else if (_value.Equals("Email"))
            {
                PayScreenSelectionVisibility = Visibility.Visible;
                PayScreenEmailVisibility = Visibility.Visible;
                BoolSelectionEmail = true;
                BoolOptionEmail = true;

            }
            else if (_value.Equals("Client"))
            {
                PayScreenSelectionVisibility = Visibility.Visible;
                PayScreenClientVisibility = Visibility.Visible;
                BoolSelectionClient = true;
                BoolOptionClient = true;
            }
        }


        private void HidePayScreenComponents()
        {
            PayScreenPaymentsVisibility = Visibility.Collapsed;

            PayScreenPrintVisibility = Visibility.Collapsed;
            PayScreenSelectionVisibility = Visibility.Collapsed;
            PayScreenClientVisibility = Visibility.Collapsed;
            PayScreenEmailVisibility = Visibility.Collapsed;
        }
        private void ClearSideButtonsSelection()
        {
            BoolOptionEmail = false;
            BoolOptionPrint = false;
            BoolOptionClient = false;

        }

        private void ClearPaymentOptions()
        {
            BoolIsCash = false;
            BoolIsPin = false;
        }


        //Checkbox Bools SELECTION BOX clients/mail
        private bool _boolSelectionClient = true;
        public bool BoolSelectionClient
        {
            get { return _boolSelectionClient; }
            set
            {
                SetProperty(ref _boolSelectionClient, value);
            }
        }

        private bool _boolSelectionEmail = false;
        public bool BoolSelectionEmail
        {
            get { return _boolSelectionEmail; }
            set
            {
                SetProperty(ref _boolSelectionEmail, value);
            }
        }

        //isChecked bool for side Buttons on payscreen
        private bool _boolOptionPrint = false;
        public bool BoolOptionPrint
        {
            get { return _boolOptionPrint; }
            set
            {
                SetProperty(ref _boolOptionPrint, value);
            }
        }

        private bool _boolOptionEmail = false;
        public bool BoolOptionEmail
        {
            get { return _boolOptionEmail; }
            set
            {
                SetProperty(ref _boolOptionEmail, value);
            }
        }

        private bool _boolOptionClient = false;
        public bool BoolOptionClient
        {
            get { return _boolOptionClient; }
            set
            {
                SetProperty(ref _boolOptionClient, value);
            }
        }


        //Print bool ischecked 

        private bool _boolPrintBon = true;
        public bool BoolPrintBon
        {
            get { return _boolPrintBon; }
            set
            {
                SetProperty(ref _boolPrintBon, value);
            }
        }

        private bool _boolPrintA4 = false;
        public bool BoolPrintA4
        {
            get { return _boolPrintA4; }
            set
            {
                SetProperty(ref _boolPrintA4, value);
            }
        }


        //strings customer's email searchbox

        private string _emailSearchBox;
        public string EmailSearchBox
        {
            get { return _emailSearchBox; }
            set
            {
                SetProperty(ref _emailSearchBox, value);
 
            }
        }

        private void EmailSubmit(object ite)
        {
            if (!string.IsNullOrWhiteSpace(EmailSearchBox))
            {
                CurrentPurchase.ClientName = EmailSearchBox;
                HidePayScreenComponents();
                ClearSideButtonsSelection();
                PayClientTextVisibility = Visibility.Visible;
            }
            else
                sounds.Error();
          
            
        }
       



        //Visibility for the three main side buttons on Pay Sreen

        private Visibility _payScreenPrintVisibility = Visibility.Collapsed;
        public Visibility PayScreenPrintVisibility
        {
            get { return _payScreenPrintVisibility; }
            set
            {
                SetProperty(ref _payScreenPrintVisibility, value);
            }
        }



        private Visibility _payScreenSelectionVisibility = Visibility.Collapsed;
        public Visibility PayScreenSelectionVisibility
        {
            get { return _payScreenSelectionVisibility; }
            set
            {
                SetProperty(ref _payScreenSelectionVisibility, value);
            }
        }

        private Visibility _payScreenClientVisibility = Visibility.Collapsed;
        public Visibility PayScreenClientVisibility
        {
            get { return _payScreenClientVisibility; }
            set
            {
                SetProperty(ref _payScreenClientVisibility, value);
            }
        }

        private Visibility _payScreenEmailVisibility = Visibility.Collapsed;
        public Visibility PayScreenEmailVisibility
        {
            get { return _payScreenEmailVisibility; }
            set
            {
                SetProperty(ref _payScreenEmailVisibility, value);
            }
        }


        //SearchBox lists
        private Visibility _payScreenClientBoxVisibility = Visibility.Collapsed;
        public Visibility PayScreenClientBoxVisibility
        {
            get { return _payScreenClientBoxVisibility; }
            set
            {
                SetProperty(ref _payScreenClientBoxVisibility, value);
            }
        }



        private Visibility _PayClientText = Visibility.Collapsed;
        public Visibility PayClientTextVisibility
        {
            get { return _PayClientText; }
            set
            {
                SetProperty(ref _PayClientText, value);
            }
        }


    

    
        private Visibility _payScreenVisibility= Visibility.Hidden;
        public Visibility PayScreenVisibility
        {
            get { return _payScreenVisibility; }
            set
            {
                SetProperty(ref _payScreenVisibility, value);
            }
        }

       

        private Visibility _PaymentNumpadVisibility = Visibility.Hidden;
        public Visibility PaymentNumpadVisibility
        {
            get { return _PaymentNumpadVisibility; }
            set
            {
                SetProperty(ref _PaymentNumpadVisibility, value);
            }
        }

        //Payment Option Buttons
        private bool _boolisPin = false;
        public bool BoolIsPin
        {
            get { return _boolisPin; }
            set
            {
                SetProperty(ref _boolisPin, value);
            }
        }

        private bool _boolIsCash= false;
        public bool BoolIsCash
        {
            get { return _boolIsCash; }
            set
            {
                SetProperty(ref _boolIsCash, value);
            }
        }



        #endregion


        #region Delete Methods
        private void DeleteOpen(object item)
        {
            string objectvalue = item as string;


            if (objectvalue.Equals("Open"))
            {
                int purchaseItem = _purchasedProducts.Count();
                if (DeleteVisibility == Visibility.Hidden && purchaseItem >= 1)
                {
                    DeleteVisibility = Visibility.Visible;
                    sounds.Press();
                }
                else if (DeleteVisibility == Visibility.Hidden && purchaseItem < 1)
                {
                    sounds.Error();
                }
            }

            else if(objectvalue.Equals("Close"))
                DeleteVisibility = Visibility.Hidden;

        }
        private void DeleteWholeOrder(object item)
        {
            ReceiptList.Remove(CurrentPurchase);

            if (ReceiptList.Count == 0)
            {
                CreateNewReceipt(item);
            }
            else if (ReceiptIndex == 0)
            {

                SelectNextReceipt(item);
                ReceiptIndex = 0;
                ReceiptName = CurrentPurchase.Label;

            }
            else
            {
                SelectPreviousReceipt1(item);
            }

            DeleteOpen(item);
            sounds.Delete();

        }
        private Visibility _deleteVisibility = Visibility.Hidden;
        public Visibility DeleteVisibility
        {
            get { return _deleteVisibility; }
            set
            {
                SetProperty(ref _deleteVisibility, value);
            }
        }

        #endregion

        private void PurchaseSelectMultiple(object item)
        {
            if (SelectionMode == SelectionMode.Extended)
            {
                SelectionMode = SelectionMode.Multiple;
                SelectionColor=(Brush)(new BrushConverter().ConvertFrom("#0193C6"));
              
            }
            else
            {
                SelectionMode = SelectionMode.Extended;
                SelectionColor = (Brush)(new BrushConverter().ConvertFrom("#999999"));
          
            }
          
        }
        private SelectionMode _selectionMode;
        public SelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set
            {
                SetProperty(ref _selectionMode, value);
            }
        }
        private Brush _selectionColor;
        public Brush SelectionColor
        {
            get { return _selectionColor; }
            set
            {
                SetProperty(ref _selectionColor, value);
            }
        }

        


        #region Discount Pop Methods


        private void ApplyDiscountPop(object obj)
        {
            var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList(); ;
            foreach (var item in selectedItems)
            {
                ApplyDiscountedProduct(item,DiscountOption);
            }

            DiscountPopVisibility = Visibility.Collapsed;
            SelectionMode = SelectionMode.Extended;
            SelectionColor = (Brush)(new BrushConverter().ConvertFrom("#999999"));
            DeselectSelectAllPurchased();
            sounds.Save();
        }
        private void ApplyDiscountedProduct(SelectedProductViewModel product, string option)
        {
           

            if (option == "Dollar")
            {
                Console.WriteLine("Sales "+option);

               
                product.DiscountOption = "Dollar";
                product.Discount = DiscountDollar;
            }
            else if (option == "Percent")
            {
                Console.WriteLine("Sales " + option);

   
                product.DiscountOption = "Percent";
                product.DiscountPercentage = DiscountPercentage;

             


            }
            product.ComputeSubTotal();

            CurrentPurchase.ComputeTotal();
        }

        string DollarSignPath = "ViewIcons/dollarSign.png";
        string PercentSignPath = "ViewIcons/percentSign.png";
        string DiscountOption = "Percent";
        private void DiscountChangeButton(object obj)
        {
            //If Button Dollar is clicked
            if (DiscountImagePath.Equals(DollarSignPath))
            {
                DiscountDollarText = Visibility.Visible;
                DiscountPercentText = Visibility.Hidden;
                DiscountImagePath = PercentSignPath;
                DiscountOption = "Dollar";
            }
            //If Button Percentage is clicked
            else
            {
                DiscountPercentText = Visibility.Visible;
                DiscountDollarText = Visibility.Hidden;
                DiscountImagePath = DollarSignPath;
                DiscountOption = "Percent";
            }
               
        }

       

     

        private void IncrementDiscountPop(object item)
        {
            if (DiscountDollarText == Visibility.Visible)
            {
                DiscountDollar++;
            }
            else
            {
                if (DiscountPercentage != 100)
                    DiscountPercentage++;
            }


           
        }
        private void DecrementDiscountPop(object item)
        {
            if (DiscountDollarText == Visibility.Visible)
            {
                if (DiscountDollar != 0)
                    DiscountDollar--;
            }
            else
            {
                if (DiscountPercentage != 0)
                    DiscountPercentage--;
            }

        
        }

        private void DiscountPopOpen(object obj)
        {
            int selectedProducts = _purchasedProducts.Where(x => x.IsSelected).Count();



            if (DiscountPopVisibility == Visibility.Collapsed && selectedProducts>=1)
            {
                DiscountPopRow = 0;
                QuantityPopRow = 1;
                DiscountPopVisibility = Visibility.Visible;
                QuantityPopVisibility = Visibility.Collapsed;
                sounds.Numpad();
            }
            else if (DiscountPopVisibility == Visibility.Collapsed && selectedProducts < 1 )
            {
                sounds.Error();
            }
            else
                DiscountPopVisibility = Visibility.Collapsed;



        }

        private Visibility _DiscountPopVisibility;
        public Visibility DiscountPopVisibility
        {
            get { return _DiscountPopVisibility; }
            set
            {
                SetProperty(ref _DiscountPopVisibility, value);
            }
        }
        private Visibility _DiscountPercentText;
        public Visibility DiscountPercentText
        {
            get { return _DiscountPercentText; }
            set
            {
                SetProperty(ref _DiscountPercentText, value);
            }
        }

        private Visibility _DiscountDollarText;
        public Visibility DiscountDollarText
        {
            get { return _DiscountDollarText; }
            set
            {
                SetProperty(ref _DiscountDollarText, value);
            }
        }

       

        public int _discountPercentage;
        public int DiscountPercentage
        {
            get { return _discountPercentage; }
            set
            {
                _discountPercentage = value;
                OnPropertyChanged();
            }
        }

        public decimal _discountDollar;
        public decimal DiscountDollar
        {
            get { return _discountDollar; }
            set
            {
                _discountDollar = value;
                OnPropertyChanged();
            }
        }

        public int _discountPopRow;
        public int DiscountPopRow
        {
            get { return _discountPopRow; }
            set
            {
                _discountPopRow = value;
                OnPropertyChanged();
            }
        }

        public string _discountImagePath;
        public string DiscountImagePath
        {
            get { return _discountImagePath; }
            set
            {
                _discountImagePath = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Quantity Pop Methods


        private void SelectQuantityOptionLongPress(object obj)
        {
            string object_value = obj as string;

            if (object_value.Equals("Open"))
            {
                if (QuantitySelectionVisibility == Visibility.Collapsed)
                    QuantitySelectionVisibility = Visibility.Visible;
            }
            else
                QuantitySelectionVisibility = Visibility.Collapsed;



        }


        //Quantity hold selections eg. 6,10,12
        private void SelectQuantityOption(object obj)
        {

            int numberPressed = Convert.ToInt32(obj);
            QuantitySelectOption = numberPressed;
            QuantityPop = numberPressed;
            QuantitySelectionVisibility = Visibility.Collapsed;

        }

        private void SelectQuantityOption2(object obj)
        {
            
            QuantityPop = QuantitySelectOption;
            QuantitySelectionVisibility = Visibility.Collapsed;

        }

        private Visibility _QuantitySelectionVisibility=Visibility.Collapsed;
        public Visibility QuantitySelectionVisibility
        {
            get { return _QuantitySelectionVisibility; }
            set
            {
                SetProperty(ref _QuantitySelectionVisibility, value);
            }
        }

        public int _quantitySelectOption=6;
        public int QuantitySelectOption
        {
            get { return _quantitySelectOption; }
            set
            {
                _quantitySelectOption = value;
                OnPropertyChanged();
            }
        }



        private void ApplyQuantityPop(object obj)
        {
            var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList(); ;
            foreach (var item in selectedItems)
            {
                IncrementSelectedProduct(item,QuantityPop);
            }
            
            QuantityPopVisibility = Visibility.Collapsed;
            SelectionMode = SelectionMode.Extended;
            SelectionColor = (Brush)(new BrushConverter().ConvertFrom("#999999"));
            DeselectSelectAllPurchased();
            sounds.Save();
        }

        private void IncrementSelectedProduct(SelectedProductViewModel product,int quantity)
        {
            product.Quantity=quantity;
            CurrentPurchase.ComputeTotal();
        }

        private void IncrementQuantityPop(object item)
        {
            QuantityPop++;
        }
        private void DecrementQuantityPop(object item)
        {
            if(QuantityPop!=1)
            QuantityPop--;
        }
        SoundsComponent sounds = new SoundsComponent();
        private void QuantityPopOpen(object obj)
        {
            
            int selectedProducts = _purchasedProducts.Where(x => x.IsSelected).Count();

           
            if (QuantityPopVisibility == Visibility.Collapsed && selectedProducts>=1)
            {
                DiscountPopRow = 1;
                QuantityPopRow = 0;
                QuantityPopVisibility = Visibility.Visible;
                DiscountPopVisibility = Visibility.Collapsed;
                sounds.Press();
            }
            else if (QuantityPopVisibility == Visibility.Collapsed && selectedProducts < 1)
            {
                sounds.Error();
            }
            else
                QuantityPopVisibility = Visibility.Collapsed;

            QuantitySelectionVisibility = Visibility.Collapsed;

        }

        private Visibility _QuantityPopVisibility;
        public Visibility QuantityPopVisibility
        {
            get { return _QuantityPopVisibility; }
            set
            {
                SetProperty(ref _QuantityPopVisibility, value);
            }
        }

        public int _quantityPop;
        public int QuantityPop
        {
            get { return _quantityPop; }
            set
            {
                _quantityPop = value;
                OnPropertyChanged();
            }
        }

        public int _quantityPopRow;
        public int QuantityPopRow
        {
            get { return _quantityPopRow; }
            set
            {
                _quantityPopRow = value;
                OnPropertyChanged();
            }
        }





        #endregion

        #region Numpads Method
        public void NumpadOpen(object obj)
        {
          

            if (NumpadVisibility == Visibility.Hidden)
            {
                sounds.NumpadOpen();
                NumpadVisibility = Visibility.Visible;
                
            }
            
            else
                NumpadVisibility = Visibility.Hidden;


            NumpadCleared(null);

        }

        string stringNum = "00";
        private void NumpadNumPressed(object obj)
        {
            string numberPressed = obj as string;
            stringNum = stringNum + numberPressed;

            NumpadPrice= Convert.ToDecimal(stringNum) / 100;

        }

        private void NumpadCleared(object obj)
        {

            NumpadPrice = 0;
            stringNum = "00";
            NumpadText = string.Empty;
        }

        private void NumpadBackspace(object obj)
        {
            try
            {
                if (stringNum != "00")
                {
                    string num_price = NumpadPrice.ToString().Replace(".", "");
                    string num_price_remove = num_price.Remove(num_price.Length - 1);
                    stringNum = num_price_remove;

                    NumpadPrice = Convert.ToDecimal(stringNum) / 100;
                }
            }
            catch (Exception ee) {
            }
          

        }
        private void NumpadPurchased(object obj)
        {


            if (NumpadPrice != 0 && !string.IsNullOrWhiteSpace(NumpadText))
            {
                AddNumpadProduct();
                NumpadCleared(null);
                NumpadVisibility = Visibility.Hidden;
            }
            else
            {
                sounds.Error();
            }

        }

        private void AddNumpadProduct()
        {
            var item = new SelectedProductViewModel
            {
                Quantity = 1,
                ID = 0,
                Name = NumpadText,
                Price = NumpadPrice

            };

            item.ComputeSubTotal();

            PurchasedProducts.Add(item);

            CurrentPurchase.ComputeTotal();
        }


        private string _numpadText;
        public string NumpadText
        {
            get { return _numpadText; }
            set
            {
                _numpadText = value;
                OnPropertyChanged();
            }
        }

        private decimal _numpadPrice;
        public decimal NumpadPrice
        {
            get { return _numpadPrice; }
            set
            {
                _numpadPrice = value;
                OnPropertyChanged();
            }
        }


        private Visibility _NumpadVisibility;
        public Visibility NumpadVisibility
        {
            get { return _NumpadVisibility; }
            set
            {
                SetProperty(ref _NumpadVisibility, value);
            }
        }

        #endregion

        

        private bool ProductCategoryFilter(object item)
        {
            var product = item as ProductModel;
            return item == null ? true : product.Category.Contains(_categoryFilter);
        }


        private bool ProductSubCategoryFilter(object item)
        {
            var product = item as ProductModel;
            return (product.Category.Contains(_categoryFilter) &&
                product.SubCategory.Contains(_subCategoryFilter));
        }

        private void SetSubCategories (string category)
        {
            SubCategories = new ObservableCollection<ProductSubCategoryModel>(_categories.Where(x => x.Name == category).First().SubCategories);
            SubCategoryFilter = string.Empty;
        }

        private bool CustomerFiltering(object item)
        {
            return true;
            //var cus = item as CustomerModel;
            //return (product.Category.Contains(_categoryFilter) &&
            //    product.SubCategory.Contains(_subCategoryFilter));
        }




        // TODO: DATABASE - Get Categories  from DB
        private void GetCategories(IList<ProductModel> products)
        {
            //Dummy Colors of Categories
            String[] categories_colors= new string[] { "#CEBA5E","#D0A342", "#B422B9", "#6BB4FA", "#39985D", "#CEBA5E", "#962525" , "#7E8085" };

            // TODO: Categories can be obtained from DB especially the color
            var categories = products.Select(x => x.Category).Distinct();

            int z = 0;
            foreach (var category in categories)
            {

                var subCategories = products.Where(x => x.Category == category)
                                    .Select(x => x.SubCategory).Distinct();

                var productSubCategories = new List<ProductSubCategoryModel>();
                foreach (var subCategory in subCategories)
                {
                    productSubCategories.Add(new ProductSubCategoryModel
                    {
                        Name = subCategory,
                         Color = categories_colors[z]
                    });
                }

                _categories.Add(new ProductCategoryModel
                {
                    Name = category,
                    Color = categories_colors[z],
                    SubCategories = productSubCategories
                });

               // Console.WriteLine("11");

                 z++;
            }

         
        }   
        // TODO: DATABASE - Get Sub_Categories  from DB
        private void GetSubCategories(IList<ProductModel> products)
        {

        }

        // TODO: DATABASE - Get Products from DB

        private void GetProducts ()
        {
            _dbProductList = new List<ProductModel> {
                 new ProductModel
                {
                    ID = 90002,
                    Name = "Ink 1 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink1.JPG",
                    Price = 45.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Deluxe",
                    Tax=5.0m
                },
                  new ProductModel
                {
                    ID = 90003,
                    Name = "Ink 2",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink2.jpg",
                    Price = 35.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Deluxe",
                    Tax=5.0m
                },
                   new ProductModel
                {
                    ID = 90004,
                    Name = "Ink 3 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink3.jpg",
                    Price = 65.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Deluxe",
                    Tax=5.0m
                },
                    new ProductModel
                {
                    ID = 90005,
                    Name = "Ink 4 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink4.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Basic",
                    Tax=2.0m
                },
                    new ProductModel
                {
                    ID = 90006,
                    Name = "Ink 5 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink5.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Basic",
                    Tax=2.0m
                },
                    new ProductModel
                {
                    ID = 90007,
                    Name = "Ink 6 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink6.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Basic",
                    Tax=2.0m
                },
                    new ProductModel
                {
                    ID = 90008,
                    Name = "Ink 7 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink7.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=2.0m
                },
                    new ProductModel
                {
                    ID = 90009,
                    Name = "Ink 8 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink8.png",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=2.0m
                },

                     new ProductModel
                {
                    ID = 90010,
                    Name = "Ink 9 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink9.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=2.0m
                },
                      new ProductModel
                {
                    ID = 90011,
                    Name = "Ink 10",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink10.jpg",
                    Price = 25.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=2.0m
                },
                       new ProductModel
                {
                    ID = 90009,
                    Name = "Ink 11 ",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink11.jpg",
                    Price = 15.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=2.0m
                }, new ProductModel
                {
                    ID = 90009,
                    Name = "Ink 12",
                    Image ="/CompleetKassa.ViewModels;component/Images/inks/ink12.jpg",
                    Price = 55.0m,
                    // Description = "This is sample 1",
                    Category = "Ink",
                    SubCategory = "Premium",
                    Tax=4.0m
                },

                 new ProductModel
                {
                    ID = 1,
                    Name = "Cheyene Hawk pen Purle with 25mm grip including spacersd dasdas das d ds ds dsas",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 100.0m,
                    // Description = "This is sample 1",
                    Category = "Shoes",
                    SubCategory = "Running",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 2,
                    Name = "Shoes 2",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 20.0m,
                    //Description = "This is sample 2",
                    Category = "Shoes",
                    SubCategory = "Walking",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 3,
                    Name = "Bag 1",
                    Image ="/CompleetKassa.ViewModels;component/Images/sumi_black.jpg",
                    Price = 20.0m,
                    // Description = "This is sample 2",
                    Category = "Bag",
                    SubCategory = "Shoulder Bag",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 4,
                    Name = "Bag 2",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 20.0m,
                    // Description = "This is sample 2",
                    Category = "Bag",
                    SubCategory = "Shoulder Bag",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 5,
                    Name = "Belt 1",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Belt",
                    SubCategory = "Men's Belt",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 5,
                    Name = "Belt 11",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Sterfilters",
                    SubCategory = "Men's BeltXX",
                    Tax=12.0m
                },
                new ProductModel
                {
                    ID = 6,
                    Name = "Nike Shoes",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Shoes",
                    SubCategory = "Basketball",
                    Tax=12.0m
                }
                ,
                new ProductModel
                {
                    ID =7,
                    Name = "Nike Shoes2",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Shoes",
                    SubCategory = "Basketball",
                    Tax=12.0m
                }
                ,
                new ProductModel
                {
                    ID =7,
                    Name = "Nike Shoes2",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "SkylightFilters",
                    SubCategory = "Default",
                    Tax=12.0m
                }
                ,
                new ProductModel
                {
                    ID =7,
                    Name = "Nike Shoes2",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Filtersets",
                        SubCategory = "Default",
                    Tax=12.0m
                }
                ,
                new ProductModel
                {
                    ID =7,
                    Name = "Nike Shoes2",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "UV Filters",
                   SubCategory = "Default",
                    Tax=12.0m
                }

            };

            GetCategories(_dbProductList);
        }

      

        private void SelectFirstCategory ()
        {
            if (_categories != null && 0 < _categories.Count)
            {
                SelectedCategory = _categories[0];
            }
        }

		private void DiscountPurchaseByPercent (object obj)
		{
			var selectedItems = _purchasedProducts.Where (x => x.IsSelected).ToList (); ;
			foreach (var item in selectedItems) {
				DiscountedProduct (item, ProductDiscountOptions.Percent);
			}
		}

		private void DiscountPurchaseByDollar(object obj)
        {
            var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList(); ;
            foreach (var item in selectedItems)
            {
                DiscountedProduct(item, ProductDiscountOptions.Dollar);
            }
        }

        private void IncrementPurchase(object obj)
        {
            var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList(); ;
            foreach (var item in selectedItems)
            {
                IncrementPurchasedProduct(item);
            }
        }

      
        private void DeleteProducts(object obj)
        {
            int selectedCount = _purchasedProducts.Where(x => x.IsSelected).Count();


            if (selectedCount>0)
            {
                var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList();
                foreach (var item in selectedItems)
                {
                    PurchasedProducts.Remove(item);
                }

                CurrentPurchase.ComputeTotal();
                sounds.Delete();
            }
            else
                sounds.Error();

        }

        private void DecrementPurchase(object obj)
        {
            var selectedItems = _purchasedProducts.Where(x => x.IsSelected).ToList();
            foreach (var item in selectedItems)
            {
                DecrementPurchasedProduct(item);
            }
        }

        public void SelectAllPurchased(object obj) 
        {
            foreach (var item in _purchasedProducts)
            {
                item.IsSelected = true;
            }
        }

        public void DeselectSelectAllPurchased()
        {
            foreach (var item in _purchasedProducts)
            {
                item.IsSelected = false;
            }
        }

        

        private void Puchase(object obj)
        {
            // TODO: Fix bug when press the same product on receipt

            // Cannot add products on receipt when the PayScreen is active
            if (PayScreenVisibility == Visibility.Hidden)
            {
                var item = (SelectedProductViewModel)obj;

                var existItem = _purchasedProducts.FirstOrDefault(x => x.ID == item.ID);
                if (existItem == null)
                {
                    AddPurchasedProduct(item);
                }
                else
                {
                    IncrementPurchasedProduct(existItem);
                }

                sounds.Products();
                DeselectSelectAllPurchased();
            }
            else
                sounds.Error();
        
        }

        public void PrintReceipt(object obj)
        {
            foreach (var item in _purchasedProducts)
            {
                
                MessageBox.Show(item.Name.ToString());
            }
        }
        

    


        public string imgAddReceiptPath = "ViewIcons/order_add.png";
        public string imgNextReceiptPath = "ViewIcons/order_next.png";

        public string _receiptImagePath;
        public string ReceiptImagePath
        {
            get { return _receiptImagePath; }
            set
            {
                _receiptImagePath = value;
                OnPropertyChanged();
            }
        }

        public int _remainingOrder=0;
        public int RemainingOrder
        {
            get { return _remainingOrder; }
            set
            {
                _remainingOrder = value;
                OnPropertyChanged();
            }
        }

        private Visibility _prevVisibility=Visibility.Hidden;
        public Visibility PrevVisibility
        {
            get { return _prevVisibility; }
            set
            {
                SetProperty(ref _prevVisibility, value);
            }
        }


        private void AddOrNextReceipt(object obj)
        {
            if (ReceiptImagePath == imgAddReceiptPath)
            {
                CreateNewReceipt(null);
            }
            else if (ReceiptImagePath == imgNextReceiptPath)
                SelectNextReceipt(null);

            PrevVisibility = Visibility.Visible;
        }


        // TODO: Temporary Receipt counter
        private int _receiptCounter =0;


        private int OrderName=100000;


        public string _receiptName ;
        public string ReceiptName
        {
            get { return _receiptName; }
            set
            {
                _receiptName = value;
                OnPropertyChanged();
            }
        }

        // TODO: Need to refractor for SelectPrevious and SelectNext



        private void CreateNewReceipt(object obj)
        {

            CurrentPurchase = new PurchasedProductViewModel();
            CurrentPurchase.Name = (ReceiptList.Count()).ToString();
            CurrentPurchase.Label = $"{OrderName++}";
            ReceiptName = CurrentPurchase.Label;
          

            ReceiptList.Add(CurrentPurchase);
            ReceiptIndex = ReceiptList.Count() - 1;
            ReceiptImagePath = imgAddReceiptPath;

            RemainingOrder = ReceiptList.IndexOf(ReceiptList.Where(x => x.Label == ReceiptName.ToString()).FirstOrDefault());

        }

        private void SelectPreviousReceipt(object obj)
        {
            

            RemainingOrder = ReceiptList.IndexOf(ReceiptList.Where(x => x.Label == ReceiptName.ToString()).FirstOrDefault()) - 1;

            Console.WriteLine(RemainingOrder);

            if (RemainingOrder <= 0)
                PrevVisibility = Visibility.Hidden;
            
            else
                PrevVisibility = Visibility.Visible;

            ReceiptImagePath = imgNextReceiptPath;

          

            if (ReceiptIndex <= 0) return;
            ReceiptIndex--;
            ReceiptName = CurrentPurchase.Label;

        }

        private void SelectPreviousReceipt1(object obj)
        {

            RemainingOrder = RemainingOrder-1;

          //  RemainingOrder = ReceiptList.IndexOf(ReceiptList.Where(x => x.Label == ReceiptName.ToString()).FirstOrDefault()) - 1;

            if (RemainingOrder <= 0)
                PrevVisibility = Visibility.Hidden;

            else
                PrevVisibility = Visibility.Visible;


            if (RemainingOrder + 1 >= _receiptList.Count)
                ReceiptImagePath = imgAddReceiptPath;
            else
                ReceiptImagePath = imgNextReceiptPath;




            if (ReceiptIndex <= 0) return;
            ReceiptIndex--;
            ReceiptName = CurrentPurchase.Label;

        }


        private void SelectNextReceipt(object obj)
        {

         

            RemainingOrder = ReceiptList.IndexOf(ReceiptList.Where(x => x.Label == ReceiptName.ToString()).FirstOrDefault()) + 1;

        


            if (RemainingOrder + 1 >= _receiptList.Count)
                ReceiptImagePath = imgAddReceiptPath;
            else
                ReceiptImagePath = imgNextReceiptPath;



            if (ReceiptIndex == _receiptList.Count() - 1) return;
            ReceiptIndex++;
            ReceiptName = CurrentPurchase.Label;


        }

        public void Pay(object obj)
        {


            ReceiptList.Remove(CurrentPurchase);

            if (ReceiptList.Count == 0)
            {
                CreateNewReceipt(obj);
            }
            else if (ReceiptIndex == 0)
            {

                SelectNextReceipt(obj);
                ReceiptIndex = 0;
                ReceiptName = CurrentPurchase.Label;

            }
            else
            {
                SelectPreviousReceipt1(obj);
            }
        }

        private void DiscountedProduct (SelectedProductViewModel product, ProductDiscountOptions option)
		{
			decimal discount = 0.0m;
			if(Decimal.TryParse (DiscountValue, out discount) == false) {
				discount = 0.0m;
			}

			if(option == ProductDiscountOptions.Dollar) {
				product.Discount = discount;
			}
			else if (option == ProductDiscountOptions.Percent) {
				product.Discount = product.Price * (discount/100);
			}

            
			CurrentPurchase.ComputeTotal ();
		}

		private void DiscountedProduct(SelectedProductViewModel product)
        {
            product.Discount= 5.50m;
            CurrentPurchase.ComputeTotal();
        }

        private void AddPurchasedProduct(SelectedProductViewModel product)
        {
          

			var item = new SelectedProductViewModel {
				Quantity = 1,
				ID = product.ID,
				Name = product.Name,
				Price = product.Price,
                Tax=product.Tax,
				Discount = product.Discount
			};
            Console.WriteLine(product.Tax);

            item.ComputeSubTotal ();

			PurchasedProducts.Add(item);

            CurrentPurchase.ComputeTotal();
        }

        

        private void IncrementPurchasedProduct(SelectedProductViewModel product)
        {
            product.Quantity++;
            CurrentPurchase.ComputeTotal();
        }


        private void DecrementPurchasedProduct(SelectedProductViewModel product)
        {
            product.Quantity--;
            if(product.Quantity == 0)
            {
                PurchasedProducts.Remove(product);
            }

            CurrentPurchase.ComputeTotal();
        }


       

       


    }
}
