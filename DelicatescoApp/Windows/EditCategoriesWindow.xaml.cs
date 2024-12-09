using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DelicatescoApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditCategoriesWindow.xaml
    /// </summary>
    public partial class EditCategoriesWindow : Window
    {
        private readonly Product product;
        private readonly ObservableCollection<Category> categories;
        private DelicatescoDBEntities _entities = new DelicatescoDBEntities();

        public EditCategoriesWindow(Product product)
        {
            InitializeComponent();

            this.product = product;
            categories = new ObservableCollection<Category>(product.Category);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Categories.ItemsSource = categories;
            cbox.ItemsSource = _entities.Category.ToList();
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
