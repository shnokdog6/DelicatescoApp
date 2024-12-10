using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddStoreWindow.xaml
    /// </summary>
    public partial class AddStoreWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();

        public AddStoreWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductName.Text))
            {
                MessageBox.Show("Заполните название");
                return;
            }

            _entities.Store.Add(new Store() { Name = ProductName.Text });
            _entities.SaveChanges();
            MessageBox.Show("Успешно");
            Close();
        }
    }
}
