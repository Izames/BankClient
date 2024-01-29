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
    /// Логика взаимодействия для TransactionPage.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        string Id;
        BankAccountTableAdapter bankAccountTableAdapter = new BankAccountTableAdapter();
        FinanceOperationsTableAdapter financeOperationsTableAdapter = new FinanceOperationsTableAdapter();
        public TransactionPage(string id)
        {
            InitializeComponent();
            foreach(DataRow row in bankAccountTableAdapter.GetData())
            {
                if (row["id_client"].ToString() == id)
                {
                    BankAccount.Items.Add(row);
                }
            }
            BankAccount.DisplayMemberPath = "AccountNumber";
            BankAccount.SelectedValuePath = "id_BankAccount";
            Id = id;
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
        private void AddPortfolio_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new PortfolioAddPage(Id);
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            int fromBankId;
            bool pass = false;
            if(BankAccount.SelectedIndex != 0 && ToBankAccount.Text.Length == 16)
            {
                foreach (DataRow row in bankAccountTableAdapter.GetData())
                {
                    if (row["id_BankAccount"].ToString() == BankAccount.SelectedValue.ToString())
                    {
                        if (Convert.ToDouble(row["Amount"]) > Convert.ToDouble(Balance.Text))
                        {
                            pass = true;
                        }
                    }
                }
            }
            if (pass)
            {
                foreach(DataRow row in bankAccountTableAdapter.GetData())
                {
                    if (row["AccountNumber"].ToString() == ToBankAccount.Text)
                    {
                        financeOperationsTableAdapter.InsertQuery(Convert.ToInt32(row["id_BankAccount"]), 
                            "+", DateTime.Now, Convert.ToInt32(BankAccount.SelectedValue), "успешно",
                            Convert.ToDouble(Balance.Text));
                        financeOperationsTableAdapter.InsertQuery(Convert.ToInt32(BankAccount.SelectedValue),
                            "-", DateTime.Now, Convert.ToInt32(row["id_BankAccount"]), "успешно",
                            Convert.ToDouble(Balance.Text));
                        bankAccountTableAdapter.UpdateQuery(row["AccountNumber"].ToString(),
                            Convert.ToDouble(row["Amount"]) + Convert.ToDouble(Balance.Text), 
                            Convert.ToDateTime(row["OpeningDate"].ToString()),
                            Convert.ToInt32(row["id_client"]), Convert.ToInt32(row["id_BankAccount"]));
                        foreach(DataRow row2 in bankAccountTableAdapter.GetData())
                        {
                            if (row2["id_BankAccount"].ToString() == BankAccount.SelectedValue.ToString()) 
                            {
                                bankAccountTableAdapter.UpdateQuery(row2["AccountNumber"].ToString(),
                                    Convert.ToDouble(row2["Amount"]) - Convert.ToDouble(Balance.Text),
                                    Convert.ToDateTime(row2["OpeningDate"]), Convert.ToInt32(Id), Convert.ToInt32(BankAccount.SelectedValue));
                            }
                        }

                    }
                }
            }
        }
    }
}
