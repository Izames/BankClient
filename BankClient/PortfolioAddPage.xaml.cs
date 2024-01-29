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
    /// Логика взаимодействия для PortfolioAddPage.xaml
    /// </summary>
    public partial class PortfolioAddPage : Page
    {
        string Id;
        SharesTableAdapter adapter = new SharesTableAdapter();
        BankAccountTableAdapter bankAccountTableAdapter = new BankAccountTableAdapter();
        InvestPortfolioTableAdapter portfolioTableAdapter = new InvestPortfolioTableAdapter();
        SharesXPortfolioTableAdapter sharesXPortfolioTableAdapter = new SharesXPortfolioTableAdapter();
        public PortfolioAddPage(string id)
        {
            InitializeComponent();
            List<string> banks = new List<string>();
            List<string> idbanks = new List<string>();
            Id = id;
            Shares.ItemsSource = adapter.GetData();
            Shares.DisplayMemberPath = "Name";
            Shares.SelectedValuePath = "id_shares";

            foreach(DataRow row in bankAccountTableAdapter.GetData())
            {
                if (row["id_client"].ToString() == Id)
                {
                    banks.Add(row["AccountNumber"].ToString());
                    idbanks.Add(row["id_BankAccount"].ToString());
                }
            }
            BankAccount.ItemsSource = banks;
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

        private void Shares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool pass = true;
            foreach(string item in AddedShares.Items)
            {
                if (item == Shares.SelectedValue.ToString())
                {
                    pass = false;
                }
            }
            if (pass)
            {
                AddedShares.Items.Add(Shares.SelectedValue.ToString());
                foreach(DataRow row in adapter.GetData())
                {
                    if (row["id_shares"].ToString() == Shares.SelectedValue.ToString())
                    {
                        Balance.Text = (Convert.ToDouble(row["SharesPrice"]) + Convert.ToDouble(Balance.Text)).ToString();
                    }
                }
            }
            
        }



        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Length > 3 && Name.Text.Length < 30 && BankAccount.SelectedIndex != -1)
            {
                foreach(DataRow row in bankAccountTableAdapter.GetData())
                {
                    if (row["AccountNumber"].ToString() == BankAccount.SelectedValue.ToString())
                    {
                        if (Convert.ToDouble(row["Amount"]) > Convert.ToDouble(Balance.Text))
                        {
                            bankAccountTableAdapter.UpdateQuery(row["AccountNumber"].ToString(), 
                                Convert.ToDouble(row["Amount"]) - Convert.ToDouble(Balance.Text), 
                                Convert.ToDateTime( row["OpeningDate"]), Convert.ToInt32(Id), 
                                Convert.ToInt32(row["id_BankAccount"]));
                            portfolioTableAdapter.InsertQuery(Name.Text, "активен", Convert.ToDouble(Balance.Text), DateTime.Today, null, Convert.ToInt32(Id));
                            foreach(var item in AddedShares.Items)
                            {
                                int ID = 0;
                                foreach(DataRow row2 in portfolioTableAdapter.GetData())
                                {
                                    ID = Convert.ToInt32(row2["id_portfolio"]);
                                }
                                sharesXPortfolioTableAdapter.InsertQuery(Convert.ToInt32(item), ID);
                            }
                        }
                    }
                }
            }
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new PortfolioPage(Id);

        }
    }
}
