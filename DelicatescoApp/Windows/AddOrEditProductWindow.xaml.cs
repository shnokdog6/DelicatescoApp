using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditProductWindow.xaml
    /// </summary>
    public partial class AddOrEditProductWindow : Window
    {
        private readonly Product currentProduct;

        public AddOrEditProductWindow(Product product = null)
        {
            InitializeComponent();

            currentProduct = product ?? new Product();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new EditCategoriesWindow(currentProduct);
            window.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentProduct == null) return;
            Preview.Source = new BitmapImage(new Uri(@"/Images/" + currentProduct.Image, UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
