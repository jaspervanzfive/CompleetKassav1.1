using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using CompleetKassa.ViewModels.Commands;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using CompleetKassa.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using CompleetKassa.DataTypes.Enumerations;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;


namespace CompleetKassa.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public ObservableCollection<BaseViewModel> PageViewModels
		{
			get;
			private set;
		}

        public BaseViewModel DefaultViewModel { get; set; }

        BaseViewModel _currentPageViewModel;
		public BaseViewModel CurrentPageViewModel
		{
			get { return _currentPageViewModel; }
			private set
			{
				if (Equals (value, _currentPageViewModel)) return;
				_currentPageViewModel = value;
				OnPropertyChanged ();
			}
		}

     
        private SelectedSideButton _selectedItem;
        public SelectedSideButton SelectedItem
        {

            get { return _selectedItem; }
            set
            {
                
                if (value != null)
                {
                    _selectedItem = value;
                 
                }


            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public ICommand OnChangePageCommand { get; private set; }
        public ICommand OnLockCommand { get; private set; }

    


        public MainViewModel () : base ("Main","#fff", "Icons/product.png")
		{
            DefaultViewModel = new SalesViewModel();

            _currentPageViewModel = DefaultViewModel;

            _mainPageViewModel = null;






            ViewModelProduct = new ProductsViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };
            ViewModelCustomer = new CustomersViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            }; 
            ViewModelTransaction = new TransactionsViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };
            ViewModelTotal = new TotalsViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };
            ViewModelUser = new UsersViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };
            ViewModelSetting = new SettingsViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };
            ViewModelSupport = new SupportViewModel()
            {
                OnClosePageCommand = new BaseCommand(ClosePage)
            };


            //Login and Lock
            ViewModelLogin= new LoginViewModel()
            {
               OnClose = new BaseCommand(Close)
            };
            ViewModelLock= new LockViewModel()
            {
              OnClose  = new BaseCommand(Close)
            };




            // this.CreateContentViewModels ();





            OnChangePageCommand = new BaseCommand (ChangePage);
            OnLockCommand = new BaseCommand(LockCommand);


        




        }
     
        void ChangePageCommand (object obj)
		{
			var page = (BaseViewModel)obj;

			if(page != CurrentPageViewModel) {
				CurrentPageViewModel = page;

            }
		}

        public BaseViewModel ViewModelProduct { get; set; }
        public BaseViewModel ViewModelCustomer { get; set; }
        public BaseViewModel ViewModelTransaction { get; set; }
        public BaseViewModel ViewModelTotal { get; set; }
        public BaseViewModel ViewModelUser { get; set; }
        public BaseViewModel ViewModelSetting { get; set; }
        public BaseViewModel ViewModelSupport { get; set; }

        private void ChangePage(object obj)
        {
            string page_name = obj as string;

            ClearHighlights();

            if (page_name.Equals("Product"))
            {

                ProductVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelProduct;
            }
            else if (page_name.Equals("Customer"))
            {
                CustomerVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelCustomer;
            }
            else if (page_name.Equals("Transaction"))
            {
                TransactionVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelTransaction;
            }
            else if (page_name.Equals("Total"))
            {
                TotalVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelTotal;
            }
            else if (page_name.Equals("User"))
            {
                UserVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelUser ;
            }
            else if (page_name.Equals("Setting"))
            {
               SettingVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelSetting;
            }
            else if (page_name.Equals("Support"))
            {
                SupportVisibility = Visibility.Visible;
                CurrentPageViewModel = ViewModelSupport;
            }



        }

        private void ClearHighlights()
        {
            ProductVisibility = Visibility.Hidden;
            CustomerVisibility = Visibility.Hidden;
            TransactionVisibility = Visibility.Hidden;
            TotalVisibility = Visibility.Hidden;
            UserVisibility = Visibility.Hidden;
            SettingVisibility = Visibility.Hidden;
            SupportVisibility = Visibility.Hidden;
        }

        #region Visibility of Side buttons
        private Visibility _productVisibility=Visibility.Hidden;
        public Visibility ProductVisibility
        {
            get { return _productVisibility; }
            set
            {
                SetProperty(ref _productVisibility, value);
            }
        }

        private Visibility _customerVisibility = Visibility.Hidden;
        public Visibility CustomerVisibility
        {
            get { return _customerVisibility; }
            set
            {
                SetProperty(ref _customerVisibility, value);
            }
        }

        private Visibility _transactionVisibility = Visibility.Hidden;
        public Visibility TransactionVisibility
        {
            get { return _transactionVisibility; }
            set
            {
                SetProperty(ref _transactionVisibility, value);
            }
        }
        private Visibility _totalVisibility = Visibility.Hidden;
        public Visibility TotalVisibility
        {
            get { return _totalVisibility; }
            set
            {
                SetProperty(ref _totalVisibility, value);
            }
        }
        private Visibility _userVisibility = Visibility.Hidden;
        public Visibility UserVisibility
        {
            get { return _userVisibility; }
            set
            {
                SetProperty(ref _userVisibility, value);
            }
        }
        private Visibility _settingVisibility = Visibility.Hidden;
        public Visibility SettingVisibility
        {
            get { return _settingVisibility; }
            set
            {
                SetProperty(ref _settingVisibility, value);
            }
        }
        private Visibility _supportVisibility = Visibility.Hidden;
        public Visibility SupportVisibility
        {
            get { return _supportVisibility; }
            set
            {
                SetProperty(ref _supportVisibility, value);
            }
        }
     

        #endregion


        /// <summary>
        /// Add OnclosePageCommand on every Pages
        /// </summary>

        public void CreateContentViewModels ()
		{
			PageViewModels = new ObservableCollection<BaseViewModel>
			{
                new ProductsViewModel() {
                    OnClosePageCommand = new BaseCommand (ClosePage)
                },
                new CustomersViewModel {
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
                   new TransactionsViewModel{
                    OnClosePageCommand = new BaseCommand (ClosePage)
                },

                         new TotalsViewModel{
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
						 new UsersViewModel{
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
						 new SettingsViewModel{
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
						 new SupportViewModel{
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
						 new LockViewModel{
					OnClosePageCommand = new BaseCommand (ClosePage)
				},
            };
		}
		public void ClosePage (object obj)
		{
        
            ClearHighlights();
            CurrentPageViewModel = DefaultViewModel;
		}


        ///<summary>
        /// For Lock and Login Page
        ///</summary>
        ///

        BaseViewModel _mainPageViewModel;
        public BaseViewModel ViewModelLock { get; set; }
        public BaseViewModel ViewModelLogin { get; set; }
        public BaseViewModel MainPageViewModel
        {
            get { return _mainPageViewModel; }
            private set
            {
                if (Equals(value, _mainPageViewModel)) return;
                _mainPageViewModel = value;
                OnPropertyChanged();
            }
        }

        private void LockCommand(object obj)
        {
            MainPageViewModel = ViewModelLock;
        }

        public void Close(object obj)
        {
            MainPageViewModel = null;
        }

    }
}
