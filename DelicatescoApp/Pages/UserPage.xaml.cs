using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            var cart = Session.CurrentUser.Cart.FirstOrDefault();
            if (cart == null)
            {
                var userId = Session.CurrentUser.Id;
                cart = _entities.Cart.Add(new Cart() { UserId = userId });
            }

            var cartItem = _entities.CartItem
                .Where(x => x.CartId == cart.Id && x.ProductId == selectedProduct.Id)
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
                CartId = cart.Id,
            };

            _entities.CartItem.Add(cartItem);
            _entities.SaveChanges();

            UpdateProductInCartListBox();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProductInCartListBox();
            UpdateProductsListBox();
            UpdateOrdersDataGrid();
        }

        private void UpdateProductsListBox()
        {
            Products.ItemsSource = _entities.Product.ToList();
            Products.SelectedIndex = 0;
        }

        private void UpdateProductInCartListBox()
        {
            ProductsInCart.ItemsSource = _entities.CartItem.ToList();
            ProductsInCart.SelectedIndex = 0;
        }

        private void UpdateOrdersDataGrid()
        {
            Orders.ItemsSource = _entities.Order.ToList();
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

        private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsInCart.SelectedItem as CartItem;
            if (selectedProduct == null) return;

            selectedProduct.Quantity += 1;
            _entities.CartItem.AddOrUpdate(selectedProduct);
            _entities.SaveChanges();

            ProductQuantity.Text = $"{selectedProduct.Quantity}";
        }

        private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsInCart.SelectedItem as CartItem;
            if (selectedProduct == null) return;

            if (selectedProduct.Quantity > 1)
            {
                selectedProduct.Quantity -= 1;
                _entities.CartItem.AddOrUpdate(selectedProduct);
                _entities.SaveChanges();
                ProductQuantity.Text = $"{selectedProduct.Quantity}";
                return;
            }

            DeleleFromCart();
        }

        private void DeleteFromCartBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleleFromCart();
        }

        private void DeleleFromCart()
        {
            var selectedProduct = ProductsInCart.SelectedItem as CartItem;
            if (selectedProduct == null) return;

            var result = MessageBox.Show("Удалить товар из корзины?", string.Empty, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                _entities.CartItem.Remove(selectedProduct);
                _entities.SaveChanges();
                UpdateProductInCartListBox();
            }

            if (ProductsInCart.SelectedItem == null)
            {
                ProductName2.Text = string.Empty;
                ProductQuantity.Text = "0";
                return;
            }
        }

        private void CreateOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsInCart.SelectedItem as CartItem;

            if (selectedProduct == null) return;

            var totalPrice = selectedProduct.Product.Price * selectedProduct.Quantity;

            var result = MessageBox.Show(
                $"Заказать {selectedProduct.Product.Name}\n" +
                $"Кол-во: {selectedProduct.Quantity}\n" +
                $"На сумму: {totalPrice}₽",
                "Заказ", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return;

            var userId = Session.CurrentUser.Id;

            var orderDto = new Order()
            {
                UserId = userId,
                Sum = (float)totalPrice,
                StatusId = 1,
                Date = DateTime.Now
            };

            var order = _entities.Order.Add(orderDto);

            var orderItem = new OrderItem()
            {
                OrderId = order.Id,
                ProductId = selectedProduct.ProductId,
                Quantity = selectedProduct.Quantity
            };

            _entities.OrderItem.Add(orderItem);
            _entities.SaveChanges();
            UpdateOrdersDataGrid();


            MessageBox.Show("Заказ создан");
        }

        private void CreateOrderAllBtn_Click(object sender, RoutedEventArgs e)
        {
            double totalPrice = 0;
            foreach (CartItem cartItem in ProductsInCart.ItemsSource)
            {
                totalPrice += cartItem.Product.Price + cartItem.Quantity;
            }

            var result = MessageBox.Show(
                $"Заказать всё, что в корзине\n" +
                $"На сумму: {totalPrice}₽",
                "Заказ", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No) return;

            var userId = Session.CurrentUser.Id;

            var orderDto = new Order()
            {
                UserId = userId,
                Sum = (float)totalPrice,
                StatusId = 1,
                Date = DateTime.Now
            };

            var order = _entities.Order.Add(orderDto);

            var orderItems = new List<OrderItem>();
            foreach (CartItem cartItem in ProductsInCart.ItemsSource)
            {
                var orderItem = new OrderItem()
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };
                orderItems.Add(orderItem);
            }

            _entities.OrderItem.AddRange(orderItems);
            _entities.SaveChanges();
            UpdateOrdersDataGrid();

            MessageBox.Show("Заказ создан");
        }

        private void Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var order = Orders.SelectedItem as Order;
            CancelOrder.IsEnabled = order != null && order.StatusId == 1;
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            var order = Orders.SelectedItem as Order;
            if (order == null) return;

            order.StatusId = 5;
            _entities.Order.AddOrUpdate(order);
            _entities.SaveChanges();

            CancelOrder.IsEnabled = false;
            UpdateOrdersDataGrid();

        }
    }
}
