using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditSupplyWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();
        private Order currentOrder;

        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            currentOrder = order;

            var statuses = _entities.OrderStatus.ToList();
            Status.ItemsSource = statuses;
            Status.SelectedIndex = statuses.FindIndex(a => a.Id == order.StatusId);

            Products.ItemsSource = order.OrderItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var status = Status.SelectedItem as OrderStatus;

            currentOrder.StatusId = status.Id;

            _entities.Order.AddOrUpdate(currentOrder);
            _entities.SaveChanges();

            MessageBox.Show("Успешно");
            Close();
        }
    }
}
