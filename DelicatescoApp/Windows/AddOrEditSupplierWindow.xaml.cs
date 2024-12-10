using System.Data.Entity.Migrations;
using System.Windows;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditSupplierWindow.xaml
    /// </summary>
    public partial class AddOrEditSupplierWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();
        private Supplier currentSupplier;


        public AddOrEditSupplierWindow(Supplier supplier = null)
        {
            InitializeComponent();

            if (supplier != null)
            {
                SupplierName.Text = supplier.Name;
                Email.Text = supplier.Email;
                INN.Text = supplier.INN;
            }

            currentSupplier = supplier ?? new Supplier();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SupplierName.Text))
            {
                MessageBox.Show("Заполните имя");
                return;
            }

            if (string.IsNullOrEmpty(Email.Text))
            {
                MessageBox.Show("Заполните почту");
                return;
            }

            if (string.IsNullOrEmpty(INN.Text))
            {
                MessageBox.Show("Заполните ИНН");
                return;
            }

            currentSupplier.Name = SupplierName.Text;
            currentSupplier.Email = Email.Text;
            currentSupplier.INN = INN.Text;

            _entities.Supplier.AddOrUpdate(currentSupplier);
            _entities.SaveChanges();
            MessageBox.Show("Успешно");
            Close();
        }
    }
}
