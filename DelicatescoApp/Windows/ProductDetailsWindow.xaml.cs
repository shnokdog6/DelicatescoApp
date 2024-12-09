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

namespace DelicatescoApp
{
    /// <summary>
    /// Логика взаимодействия для ProductDetailsWindow.xaml
    /// </summary>
    public partial class ProductDetailsWindow : Window
    {
        private readonly Product source;

        public ProductDetailsWindow(Product product)
        {
            InitializeComponent();

            source = product;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Weight.Text = source.Name;
            Package.Text = source.ProductDetails.Package.Name;
            StorageConditions.Text = source.ProductDetails.StorageConditions;
            NutritionalValue.Text = $"белки - {source.ProductDetails.NutritionalValue.Proteins} г, жиры - {source.ProductDetails.NutritionalValue.Fats} г, углеводы - {source.ProductDetails.NutritionalValue.Carbohydrates} г";
            EnergyValue.Text = $"{source.ProductDetails.EnergyValue} ккал";
            Compound.Text = $"{source.ProductDetails.Сompound}";
        }
    }
}
