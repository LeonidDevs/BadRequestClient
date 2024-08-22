using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VittaClient.Models;
using VittaClient.Tools;

namespace VittaClient.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        public ProductDTO Product { get; private set; }
        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }

        public ProductViewModel(ProductDTO product)
        {
            Product = product;
            Quantity = 1;

            IncreaseQuantityCommand = new RelayCommand(_ => Quantity++);
            DecreaseQuantityCommand = new RelayCommand(_ => { if (Quantity > 0) Quantity--; });
        }
    }
}