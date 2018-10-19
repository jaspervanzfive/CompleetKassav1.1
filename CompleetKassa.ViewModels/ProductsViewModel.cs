using CompleetKassa.Models;
using CompleetKassa.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CompleetKassa.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private IList<ProductModel> _dbProductList;

        private ObservableCollection<ProductCategoryModel> _categories;
        private ObservableCollection<ProductSubCategoryModel> _subCategories;
        

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
            set
            {
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
                //SetSubCategories(value.Name);
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

        public ICommand OnNumpadOpen { get; private set; }

        public ProductsViewModel() : base ("Products", "#FDAC94", "Icons/product.png")
        {
            _categories = new ObservableCollection<ProductCategoryModel>();

            _categoryFilter = string.Empty;
            _subCategoryFilter = string.Empty;

            // TODO: This is where to get data from DB
            GetProducts();
            ProductList = CollectionViewSource.GetDefaultView(_dbProductList);
            ProductList.Filter += ProductCategoryFilter;
            ProductList.Filter += ProductSubCategoryFilter;


            // Set the first product as active category
            _categoryFilter = _categories.FirstOrDefault() == null ? string.Empty : _categories.FirstOrDefault().Name;
            SelectFirstCategory();

            OnNumpadOpen = new BaseCommand(NumpadOpen);

        }

        SalesViewModel ds = new SalesViewModel();

        private void NumpadOpen(object item)
        {
            ds.NumpadOpen(item);
            
        }
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
        // TODO: DATABASE - Get Sub_Categories && Categories from DB
        private void GetCategories(IList<ProductModel> products)
        {
            //Dummy Colors of Categories
            String[] categories_colors = new string[] { "#D0A342", "#B422B9", "#6BB4FA", "#39985D", "#CEBA5E", "#962525", "#7E8085" };

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
        // TODO: DATABASE - Get Products fro DB
        private void GetProducts()
        {
            _dbProductList = new List<ProductModel> {
                 new ProductModel
                {
                    ID = 1,
                    Name = "Cheyene Hawk pen Purle with 25mm grip including spacersd dasdas das d ds ds dsas",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 100.0m,
                    // Description = "This is sample 1",
                    Category = "Shoes",
                    SubCategory = "Running"
                },
                new ProductModel
                {
                    ID = 2,
                    Name = "Shoes 2",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 20.0m,
                    // Description = "This is sample 2",
                    Category = "Shoes",
                    SubCategory = "Walking"
                },
                new ProductModel
                {
                    ID = 3,
                    Name = "Bag 1",
                    Image ="/CompleetKassa.ViewModels;component/Images/sumi_black.jpg",
                    Price = 20.0m,
                    // Description = "This is sample 2",
                    Category = "Bag",
                    SubCategory = "Shoulder Bag"
                },
                new ProductModel
                {
                    ID = 4,
                    Name = "Bag 2",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 20.0m,
                    // Description = "This is sample 2",
                    Category = "Bag",
                    SubCategory = "Shoulder Bag"
                },
                new ProductModel
                {
                    ID = 5,
                    Name = "Belt 1",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Belt",
                    SubCategory = "Men's Belt"
                },
                new ProductModel
                {
                    ID = 5,
                    Name = "Belt 11",
                    Image ="/CompleetKassa.ViewModels;component/Images/sample.png",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Sterfilters",
                    SubCategory = "Men's BeltXX"
                },
                new ProductModel
                {
                    ID = 6,
                    Name = "Nike Shoes",
                    Image ="/CompleetKassa.ViewModels;component/Images/nike.jpg",
                    Price = 10.0m,
                    // Description = "This is Belt 1",
                    Category = "Shoes",
                    SubCategory = "Basketball"
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
                    SubCategory = "Basketball"
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
                    SubCategory = "Default"
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
                        SubCategory = "Default"
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
                   SubCategory = "Default"
                }

            };

            GetCategories(_dbProductList);
        }

        private void SelectFirstCategory()
        {
            if (_categories != null && 0 < _categories.Count)
            {
                SelectedCategory = _categories[0];
            }
        }


        //private ObservableCollection<ProductModel> _productList;
        //public ObservableCollection<ProductModel> ProductList
        //{
        //    get { return _productList; }
        //    set { SetProperty(ref _productList, value); }
        //}


        private ProductModel m_selectedroduct;
        public ProductModel SelectedProduct
        {
            get { return m_selectedroduct; }
            set { SetProperty(ref m_selectedroduct, value); }
        }

        private string m_sampleString;
        public string SampleString
        {
            get { return m_sampleString; }
            set { SetProperty(ref m_sampleString, value); }
        }
        private bool CanDelete
        {
            get { return SelectedProduct != null; }
        }

        //private ICommand m_deleteCommand;
        //public ICommand DeleteCommand
        //{
        //    get
        //    {
        //        if (m_deleteCommand == null)
        //        {
        //            m_deleteCommand = new BaseCommand(param => Delete(), param => CanDelete);
        //        }
        //        return m_deleteCommand;
        //    }
        //}

        //private void Delete()
        //{
        //    ProductList.Remove(SelectedProduct);
        //}
    }
}
