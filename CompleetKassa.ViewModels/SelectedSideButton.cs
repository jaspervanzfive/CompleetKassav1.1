using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompleetKassa.ViewModels
{
    public class SelectedSideButton : BaseViewModel
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public decimal Price { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public SelectedSideButton() : base(string.Empty, string.Empty, string.Empty)
        {
            ID = 0;
            Label = string.Empty;
            Price = 0.0m;
            IsSelected = false;
        }

    }
}
