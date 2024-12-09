using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSupplyWindow.xaml
    /// </summary>
    public partial class AddSupplyWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();
        private ObservableCollection<SupplyItem> products = new ObservableCollection<SupplyItem>();

        public AddSupplyWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProductsBox.ItemsSource = _entities.Product.ToList();
            Suppliers.ItemsSource = _entities.Supplier.ToList();
            Stores.ItemsSource = _entities.Store.ToList();
            Products.ItemsSource = products;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsBox.SelectedItem as Product;
            if (selectedProduct == null) return;

            if (!int.TryParse(Quantity.Text, out int quantity))
            {
                MessageBox.Show("Кол-во некорректно");
                return;
            }

            products.Add(new SupplyItem()
            {
                ProductId = selectedProduct.Id,
                Product = selectedProduct,
                Quantity = quantity

            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Date.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату");
                return;
            }

            var supplier = Suppliers.SelectedItem as Supplier;
            if (supplier == null)
            {
                MessageBox.Show("Укажите поставщика");
                return;
            }

            var store = Stores.SelectedItem as Store;
            if (store == null)
            {
                MessageBox.Show("Укажите склад");
                return;
            }

            if (products.Count < 1)
            {
                MessageBox.Show("Добавьите продукты в поставку");
                return;
            }

            Supply supplyDto = new Supply()
            {
                Date = (DateTime)Date.SelectedDate,
                SupplierId = supplier.Id,
                StoreId = store.Id
            };

            var supply = _entities.Supply.Add(supplyDto);

            foreach (var item in products)
            {
                item.SupplyId = supplyDto.Id;
            }

            _entities.SupplyItem.AddRange(products);
            _entities.SaveChanges();

            MessageBox.Show("Успешно");
            Close();

        }
    }
}
