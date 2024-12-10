using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditProductWindow.xaml
    /// </summary>
    public partial class AddOrEditProductWindow : Window
    {
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();
        private readonly ObservableCollection<Category> categories;
        private readonly Product currentProduct;

        public AddOrEditProductWindow(Product product = null)
        {
            InitializeComponent();

            var packages = _entities.Package.ToList();
            Package.ItemsSource = packages;

            categories = new ObservableCollection<Category>();

            if (product != null)
            {
                ProductName.Text = product.Name;
                ProductPrice.Text = $"{product.Price}";
                Preview.Source = new BitmapImage(new Uri(@"/Images/" + product.Image, UriKind.Relative));
                Weight.Text = $"{product.ProductDetails.Weight}";
                Package.SelectedIndex = packages.FindIndex(p => p.Id == product.ProductDetails.Package.Id);
                StorageConditions.Text = product.ProductDetails.StorageConditions;
                EnergyValue.Text = $"{product.ProductDetails.EnergyValue}";
                Compound.Text = product.ProductDetails.Сompound;
                Description.Text = product.ProductDetails.Description;

                Proteins.Text = $"{product.ProductDetails.NutritionalValue.Proteins}";
                Fats.Text = $"{product.ProductDetails.NutritionalValue.Fats}";
                Carbohydrates.Text = $"{product.ProductDetails.NutritionalValue.Carbohydrates}";


                categories = new ObservableCollection<Category>(product.Category);

            }

            currentProduct = product ?? new Product();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new EditCategoriesWindow(currentProduct);
            window.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Categories.ItemsSource = categories;
            cbox.ItemsSource = _entities.Category.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                currentProduct.Image = openFileDialog.FileName.Split('\\').Last();
                Preview.Source = bitmap;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductName.Text))
            {
                MessageBox.Show("Заполните имя");
                return;
            }

            if (!double.TryParse(ProductPrice.Text, out var price))
            {
                MessageBox.Show("Заполните цену");
                return;
            }

            if (!double.TryParse(Weight.Text, out var weight))
            {
                MessageBox.Show("Укажите вес");
                return;
            }

            var package = Package.SelectedItem as Package;
            if (package == null)
            {
                MessageBox.Show("Выберете упаковку");
                return;
            }


            if (string.IsNullOrEmpty(StorageConditions.Text))
            {
                MessageBox.Show("Заполните условия хранения");
                return;
            }

            if (!int.TryParse(EnergyValue.Text, out var energyValue))
            {
                MessageBox.Show("Укажите энергетическую ценность");
                return;
            }

            if (string.IsNullOrEmpty(Compound.Text))
            {
                MessageBox.Show("Заполните состав");
                return;
            }

            if (!double.TryParse(Proteins.Text, out var proteins))
            {
                MessageBox.Show("Укажите кол-во белков");
                return;
            }

            if (!double.TryParse(Fats.Text, out var fats))
            {
                MessageBox.Show("Укажите кол-во жиров");
                return;
            }

            if (!double.TryParse(Carbohydrates.Text, out var carbohydrates))
            {
                MessageBox.Show("Укажите кол-во углеводов");
                return;
            }

            if (string.IsNullOrEmpty(Description.Text))
            {
                MessageBox.Show("Заполните описание");
                return;
            }

            currentProduct.Name = ProductName.Text;
            currentProduct.Price = price;

            if (currentProduct.ProductDetails == null)
            {
                currentProduct.ProductDetails = new ProductDetails
                {
                    NutritionalValue = new NutritionalValue()
                };
            }


            if (currentProduct.Category != null)
            {
                //var deleted = new List<Category>();
                //var added = new List<Category>();



                //foreach (var category in categories)
                //{
                //    if (currentProduct.Category.FirstOrDefault(a => a.Id == category.Id) == null)
                //    {
                //        added.Add(category);
                //    }
                //}

                //foreach (var category in currentProduct.Category)
                //{
                //    if (categories.FirstOrDefault(a => a.Id == category.Id) == null)
                //    {
                //        deleted.Add(category);
                //    }
                //}


              

            }


            currentProduct.Category = categories;
            currentProduct.ProductDetails.Weight = weight;
            currentProduct.ProductDetails.PackageId = package.Id;
            currentProduct.ProductDetails.StorageConditions = StorageConditions.Text;
            currentProduct.ProductDetails.EnergyValue = energyValue;
            currentProduct.ProductDetails.Сompound = Compound.Text;
            currentProduct.ProductDetails.Description = Description.Text;

            currentProduct.ProductDetails.NutritionalValue.Proteins = proteins;
            currentProduct.ProductDetails.NutritionalValue.Fats = fats;
            currentProduct.ProductDetails.NutritionalValue.Carbohydrates = carbohydrates;

            _entities.Product.AddOrUpdate(currentProduct);
            _entities.ProductDetails.AddOrUpdate(currentProduct.ProductDetails);
            _entities.NutritionalValue.AddOrUpdate(currentProduct.ProductDetails.NutritionalValue);


            _entities.SaveChanges();
            MessageBox.Show("Успешно");
            Close();
        }

        private void Categories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedCategory = Categories.SelectedItem as Category;
            DeleteCategoryBtn.IsEnabled = selectedCategory != null;

        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = cbox.SelectedItem as Category;
            categories.Add(selectedCategory);
        }

        private void DeleteCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = Categories.SelectedItem as Category;
            categories.Remove(selectedCategory);
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedCategory = cbox.SelectedItem as Category;
            AddCategoryBtn.IsEnabled = selectedCategory != null;
        }
    }
}
