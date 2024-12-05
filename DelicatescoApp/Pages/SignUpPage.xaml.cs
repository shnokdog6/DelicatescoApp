using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bcypt = BCrypt.Net.BCrypt;

namespace DelicatescoApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private readonly DelicatescoDBEntities m_db = new DelicatescoDBEntities();

        public SignUpPage()
        {
            InitializeComponent();
        }

        private void GoToSignInPageBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSignInPage();
        }

        private void NavigateToSignInPage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new SignInPage());

        }

        private void NavigateToUserPage()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new UserPage());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Login.Text))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (string.IsNullOrEmpty(Password.Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            var user = m_db.User
                .Where(temp => temp.Login.Equals(Login.Text))
                .FirstOrDefault();

            if (user != null)
            {
                MessageBox.Show("Логин занят");
                return;
            }

            var userDto = new User() 
            { 
                Login = Login.Text, 
                PasswordHash = Bcypt.HashPassword(Password.Password), 
                RoleId = 2 
            };

            user = m_db.User.Add(userDto);
            m_db.SaveChanges();

            Session.CurrentUser = user;
            NavigateToUserPage();
        }
    }
}
