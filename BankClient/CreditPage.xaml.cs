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
    /// Логика взаимодействия для CreditPage.xaml
    /// </summary>
    public partial class CreditPage : Page
    {
        CreditDataTableAdapter adapter = new CreditDataTableAdapter();
        BankAccountTableAdapter bankAccountTableAdapter = new BankAccountTableAdapter();
        string Id = "";
        public CreditPage(string id)
        {
            InitializeComponent();
            Id = id;
            string pass = "";
            foreach(DataRow row in adapter.GetData())
            {
                if (row["Status"].ToString() == "принят" && row["id_client"].ToString() == Id)
                {
                    document.Visibility = Visibility.Visible;
                    BankAccount.Visibility = Visibility.Visible;
                    Summ.Visibility = Visibility.Visible;
                    Procent.Visibility = Visibility.Visible;
                    PayPerMonth.Visibility = Visibility.Visible;
                    remains.Visibility = Visibility.Visible;
                    string number = row["id_BankAccount"].ToString();
                    foreach(DataRow row2 in bankAccountTableAdapter.GetData())
                    {
                        if (row2["id_BankAccount"].ToString() == number)
                        {
                            BankAccount.Text = row2["AccountNumber"].ToString();
                        }
                    }
                    Summ.Text = row["AmountCredit"].ToString();
                    Procent.Text = row["CreditInterest"].ToString();
                    PayPerMonth.Text = row["LoanRepaymentByMonth"].ToString();
                    remains.Text = row["DebtBalance"].ToString();
                    pass = "принят";
                    break;
                }
                else if(row["Status"].ToString() == "заявка" && row["id_client"].ToString() == Id)
                {
                    InfoCredit.Visibility = Visibility.Visible;
                    pass = "заявка";
                    break;
                }
            }
            if (pass == "")
            {
                CreateCredit.Visibility = Visibility.Visible;
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
            //(Application.Current.MainWindow as MainWindow).MainFrame.Content = new CreditPage();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AuthorizationPage();
        }

        private void CreateCredit_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new CreateCreditPage(Id);
        }
    }
}
