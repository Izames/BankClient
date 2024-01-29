using BankClient.BANKDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankClient
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        string Id = "";
        ClientTableAdapter adapter = new ClientTableAdapter();
        public AccountPage(string id)
        {
            InitializeComponent();
            Id = id;
            foreach(DataRow row in adapter.GetData())
            {
                if (row["id_client"].ToString() == Id)
                {
                    SurName.Text = row["UserSurname"].ToString();
                    Name.Text = row["UserName"].ToString();
                    ThirdName.Text = row["UserPatronymic"].ToString();
                    Pincode.Text = row["Pincode"].ToString();
                    Passport.Text = row["PassportData"].ToString();
                    BirthDate.SelectedDate = DateTime.Parse(row["DateOfBirth"].ToString());
                }
            }
        }
        private void AvatarButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AccountPage(Id);
        }

        private void PortfolioButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new PortfolioPage(Id);
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            //(Application.Current.MainWindow as MainWindow).MainFrame.Content = new ClientPage();
        }

        private void CreditButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new CreditPage(Id);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AuthorizationPage();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            if (Redact.Content == "редактировать")
            {
                Redact.Content = "сохранить изменения";
                SurName.IsEnabled = true;
                Name.IsEnabled = true;
                ThirdName.IsEnabled = true;
                Pincode.IsEnabled = true;
                Passport.IsEnabled = true;
                BirthDate.IsEnabled = true;
            }
            else
            {
                Redact.Content = "редактировать";
                adapter.UpdateQuery(Pincode.Text, Name.Text, SurName.Text, ThirdName.Text, 
                    Passport.Text, BirthDate.SelectedDate.Value, Convert.ToInt32(Id));
                SurName.IsEnabled = false;
                Name.IsEnabled = false;
                ThirdName.IsEnabled = false;
                Pincode.IsEnabled = false;
                Passport.IsEnabled = false;
                BirthDate.IsEnabled = false;
            }
        }
    }
}
