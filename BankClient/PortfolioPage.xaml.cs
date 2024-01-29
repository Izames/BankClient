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
    /// Логика взаимодействия для PortfolioPage.xaml
    /// </summary>
    public partial class PortfolioPage : Page
    {
        string Id;
        InvestPortfolioTableAdapter investPortfolioTableAdapter = new InvestPortfolioTableAdapter();
        BankAccountTableAdapter bank = new BankAccountTableAdapter();
        public PortfolioPage(string id)
        {
            InitializeComponent();
            Id = id;
            foreach(DataRow row in investPortfolioTableAdapter.GetData())
            {
                if (row["Status"].ToString() != "продан")
                {
                    Portfolios.Items.Add(row);
                }
            }
            Portfolios.DisplayMemberPath = "Name";
            Portfolios.SelectedValuePath = "id_portfolio";
            Portfolios.SelectedIndex = 0;
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

        private void Portfolios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(DataRow row in investPortfolioTableAdapter.GetData())
            {
                if(Portfolios.SelectedValue.ToString() == row["id_portfolio"].ToString())
                {
                    PortfolioBalance.Text = row["Balance"].ToString(); 
                    PortfolioName.Text = row["Name"].ToString();
                }
            }
        }

        private void sell_Click(object sender, RoutedEventArgs e)
        {
            
            foreach(DataRow row in bank.GetData())
            {
                if (row["id_client"].ToString() == Id)
                {
                    bank.UpdateQuery(row["AccountNumber"].ToString(), Convert.ToDouble(row["Amount"]) + Convert.ToDouble(PortfolioBalance.Text), Convert.ToDateTime(row["OpeningDate"].ToString() ), Convert.ToInt32(Id), Convert.ToInt32(row["id_bankAccount"]));
                    investPortfolioTableAdapter.UpdateQuery("продан", DateTime.Today, Convert.ToInt32(Portfolios.SelectedValue));
                    Portfolios.SelectedIndex = 0;
                    break;
                }
            }
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new ClientPage(Id);
        }
    }
}
