using DelicatescoApp.Windows;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DelicatescoApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();
        private Category _category;

        public AdminPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateProductsListBox();
            UpdateOrdersDataGrid();
            UpdateCategoriesDataGrid();
            UpdateSuplliesDataGrid();
            UpdateSuplliersDataGrid();

            CategoriesBox.ItemsSource = _entities.OrderStatus.ToList();
            Categories2.ItemsSource = _entities.Category.ToList();
        }

        private void UpdateProductsListBox()
        {
            if (_category != null)
            {
                Products.ItemsSource = _entities.Category.Where(c => c.Id == _category.Id).FirstOrDefault().Product.ToList();
            }
            else
            {
                Products.ItemsSource = _entities.Product.ToList();
            }

            Products.SelectedIndex = 0;
            Products.SelectedIndex = 0;
        }

        private void UpdateOrdersDataGrid()
        {
            Orders.ItemsSource = _entities.Order.ToList();
        }

        private void UpdateCategoriesDataGrid()
        {
            Categories.ItemsSource = _entities.Category.ToList();
        }

        private void UpdateSuplliesDataGrid()
        {
            Supplies.ItemsSource = _entities.Supply.ToList();
        }

        private void UpdateSuplliersDataGrid()
        {
            Suppliers.ItemsSource = _entities.Supplier.ToList();
        }

        private void Products_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProduct = Products.SelectedItem as Product;
            if (selectedProduct == null) return;

            ProductName.Text = selectedProduct.Name;
            ProductPrice.Text = $"{selectedProduct.Price}₽";
            ProductImage.Source = new BitmapImage(new Uri(@"/Images/" + selectedProduct.Image, UriKind.Relative));
        }

        private void EditProductBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedProduct = Products.SelectedItem as Product;
            if (selectedProduct == null) return;

            var window = new AddOrEditProductWindow(selectedProduct);
            window.Show();
        }

        private void AddProductBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddOrEditProductWindow();
            window.Show();
        }

        private void AddCategoryBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddCategoryWindow();
            window.Show();
        }

        private void Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = Orders.SelectedItem as Order;
            var selectedStatus = CategoriesBox.SelectedItem as OrderStatus;

            EditOrder.IsEnabled = selectedOrder != null && selectedStatus != null && selectedStatus.Id != selectedOrder.StatusId && selectedOrder.StatusId != 5;
        }

        private void EditOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedOrder = Orders.SelectedItem as Order;
            var selectedStatus = CategoriesBox.SelectedItem as OrderStatus;

            selectedOrder.StatusId = selectedStatus.Id;

            _entities.Order.AddOrUpdate(selectedOrder);
            _entities.SaveChanges();

            UpdateOrdersDataGrid();

            EditOrder.IsEnabled = false;
        }

        private void CategoriesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = Orders.SelectedItem as Order;
            var selectedStatus = CategoriesBox.SelectedItem as OrderStatus;

            EditOrder.IsEnabled = selectedOrder != null && selectedStatus != null && selectedStatus.Id != selectedOrder.StatusId && selectedOrder.StatusId != 5;
        }

        private void AddSupply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddSupplyWindow();

            window.Closing += Window_Closing;
            window.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateSuplliesDataGrid();
        }

        private void Categories2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _category = Categories2.SelectedItem as Category;
            UpdateProductsListBox();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _category = null;
            UpdateProductsListBox();
        }
    }
}
