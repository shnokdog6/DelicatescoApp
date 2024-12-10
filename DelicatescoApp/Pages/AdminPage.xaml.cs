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
            UpdateStoresDataGrid();

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

        private void UpdateStoresDataGrid()
        {
            Stores.ItemsSource = _entities.Store.ToList();
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
            

            EditOrder.IsEnabled = selectedOrder != null;
        }

        private void EditOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedOrder = Orders.SelectedItem as Order;

            var window = new EditOrderWindow(selectedOrder);

            window.Closing += Window_Closing3;
            window.Show();
        }

        private void Window_Closing3(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateOrdersDataGrid();
        }

        private void CategoriesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = Orders.SelectedItem as Order;

            EditOrder.IsEnabled = selectedOrder != null;
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

        private void AddStore_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddStoreWindow();

            window.Closing += Window_Closing1; ;
            window.Show();
        }

        private void Window_Closing1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateStoresDataGrid();
        }

        private void AddSupplier_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new AddOrEditSupplierWindow();

            window.Closing += Window_Closing2;
            window.Show();
        }

        private void Window_Closing2(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateSuplliersDataGrid();
        }

        private void EditSupplier_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selected = Suppliers.SelectedItem as Supplier;
            var window = new AddOrEditSupplierWindow(selected);

            window.Closing += Window_Closing2;
            window.Show();
        }

        private void Suppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Suppliers.SelectedItem as Supplier;
            EditSupplier.IsEnabled = selected != null;
        }

        private void EditSupply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //var selected = Supplies.SelectedItem as Supply;
            //var window = new EditS(selected);

            //window.Closing += Window_Closing2;
            //window.Show();
        }
    }
}
