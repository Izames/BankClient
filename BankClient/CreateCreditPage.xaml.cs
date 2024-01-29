using BankClient.BANKDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для CreateCreditPage.xaml
    /// </summary>
    public partial class CreateCreditPage : Page
    {
        BankAccountTableAdapter bankAccountTableAdapter = new BankAccountTableAdapter();
        CreditDataTableAdapter credit = new CreditDataTableAdapter();
        string Id = "";
        public CreateCreditPage(string id)
        {
            InitializeComponent();
            BankAccounts.ItemsSource = bankAccountTableAdapter.GetData();
            BankAccounts.DisplayMemberPath = "AccountNumber";
            BankAccounts.SelectedValuePath = "id_BankAccount";
            Id = id;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(Balance.Text))
            {
                int balance = Convert.ToInt32(Balance.Text);
                if (balance < 2000000 && balance > 0)
                {
                    Random random = new Random();
                    int procent = random.Next(0, 5);
                    decimal precent = (decimal)(procent * 0.01);
                    double ppm = (balance + balance * procent * 0.01) / 12;
                    credit.InsertQuery(Convert.ToInt32(balance), Convert.ToInt32(Id), precent, (double)((balance + balance * precent) / 12), balance, DateTime.Today, null, "заявка", Convert.ToInt32(BankAccounts.SelectedValue.ToString()));
                    (Application.Current.MainWindow as MainWindow).MainFrame.Content = new CreditPage(Id);
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
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new ClientPage(Id);
        }

        private void CreditButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new CreditPage(Id);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AuthorizationPage();
        }
    }
}
