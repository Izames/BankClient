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
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        BankAccountTableAdapter adapter = new BankAccountTableAdapter();
        ClientTableAdapter clientTableAdapter = new ClientTableAdapter();
        FinanceOperationsTableAdapter financeOperationsTableAdapter = new FinanceOperationsTableAdapter();
        string Id = "";
        public ClientPage(string id)
        {
            InitializeComponent();
            ID.Text = "ID " + id;
            Id = id;
            bankAccounts.ItemsSource = adapter.GetData();
            bankAccounts.DisplayMemberPath = "AccountNumber";
            bankAccounts.SelectedValuePath = "id_BankAccount";
            bankAccounts.SelectedIndex = 0;
            List<string> operations = new List<string>();
            foreach (DataRow row in adapter.GetData())
            {
                if (row["id_BankAccount"].ToString() == bankAccounts.SelectedValue.ToString())
                {
                    Balance.Text = row["Amount"].ToString();
                }
            }
            DataTable dataTable = clientTableAdapter.GetData();
            FIO.Text = dataTable.Rows.Find(Id)["UserSurname"].ToString() + " " +
                dataTable.Rows.Find(Id)["UserName"].ToString() + " " +
                dataTable.Rows.Find(Id)["UserPatronymic"].ToString();
            foreach(DataRow row in financeOperationsTableAdapter.GetData())
            {
                if (row["id_BankAccount"].ToString() == bankAccounts.SelectedValue.ToString())
                {
                    operations.Add(row["OperationType"].ToString() + row["Balance"].ToString());
                }
            }
            Finances.ItemsSource = operations;
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

        private void means_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new TransactionPage();
        }

        private void AddBankAccount_Click(object sender, RoutedEventArgs e)
        {
            string bankaccount = "";
            Random random = new Random();
            for( int i = 0; i<16;  i++ )
            {
                bankaccount += random.Next(0, 10).ToString();
            }
            adapter.InsertQuery(bankaccount, 30000.0, DateTime.Today, Convert.ToInt32(Id));
            bankAccounts.ItemsSource = adapter.GetData();
            bankAccounts.SelectedIndex = 0;
        }

        private void bankAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> operations = new List<string>();
            foreach (DataRow row in adapter.GetData())
            {
                if (row["id_BankAccount"].ToString() == bankAccounts.SelectedValue.ToString())
                {
                    Balance.Text = row["Amount"].ToString();
                }
            }
            foreach (DataRow row in financeOperationsTableAdapter.GetData())
            {
                if (row["id_BankAccount"].ToString() == bankAccounts.SelectedValue.ToString())
                {
                    operations.Add(row["OperationType"].ToString() + row["Balance"].ToString());
                }
            }
            Finances.ItemsSource = operations;
        }
        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new DebitCardPage(Id, bankAccounts.SelectedValue.ToString());
        }
    }
}
