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
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();

        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductName.Text))
            {
                MessageBox.Show("Заполните название");
                return;
            }

            _entities.Category.Add(new Category() { Name = ProductName.Text });
            _entities.SaveChanges();
            MessageBox.Show("Успешно");
        }
    }
}
