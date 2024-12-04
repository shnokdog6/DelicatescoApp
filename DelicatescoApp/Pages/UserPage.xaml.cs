using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelicatescoApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();


        public UserPage()
        {
            InitializeComponent();
        }

        private void Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = Products.SelectedItem as Product;
            if (selectedProduct == null) return;

            ProductName.Text = selectedProduct.Name;
            ProductPrice.Text = $"{selectedProduct.Price}₽";
        }

        private void AddToCartBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = Products.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберете продукт");
                return;
            }

            var cartItem = _entities.CartItem
                .Where(x => x.CartId == 2 && x.ProductId == selectedProduct.Id)
                .FirstOrDefault();

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                _entities.CartItem.AddOrUpdate(cartItem);
                _entities.SaveChanges();
                UpdateProductInCartListBox();
                return;
            }

            cartItem = new CartItem() 
            { 
                ProductId = selectedProduct.Id,
                Quantity = 1,
                CartId = 2
            };

            _entities.CartItem.Add(cartItem);
            _entities.SaveChanges();

            UpdateProductInCartListBox();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProductInCartListBox();
            Products.ItemsSource = _entities.Product.ToList();
            Products.SelectedIndex = 0;
        }

        private void UpdateProductInCartListBox()
        {
            ProductsInCart.ItemsSource = _entities.CartItem.ToList();
            ProductsInCart.SelectedIndex = 0;
        }

        private void ShowDetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = Products.SelectedItem as Product;
            if (selectedProduct == null) return;

            var window = new ProductDetailsWindow(selectedProduct);
            window.Show();
        }

        private void ProductsInCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = ProductsInCart.SelectedItem as CartItem;
            if (selectedProduct == null) return;

            ProductName2.Text = selectedProduct.Product.Name;
            ProductQuantity.Text = $"{selectedProduct.Quantity}";
        }
    }
}
