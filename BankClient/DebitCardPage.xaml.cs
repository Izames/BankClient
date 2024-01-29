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
    /// Логика взаимодействия для DebitCardPage.xaml
    /// </summary>
    public partial class DebitCardPage : Page
    {
        DebitCardTableAdapter adapter = new DebitCardTableAdapter();
        string Id = "";
        string BankID = "";
        List<string> Cards = new List<string>();
        public DebitCardPage(string id, string bankid)
        {
            InitializeComponent();
            Id = id;
            BankID = bankid;
            foreach (DataRow row in adapter.GetData())
            {
                if (row["id_bankAccount"].ToString() == BankID && row["State"].ToString() != "заблокирована")
                {
                    Cards.Add(row["CardNumber"].ToString());
                }
            }
            cards.ItemsSource = Cards;
            cards.SelectedIndex = 0;
            
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
        private void CreateCard_Click(object sender, RoutedEventArgs e)
        {
            string cardnumber = "";
            string cvv = "";
            string pincode = "";
            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                cardnumber += random.Next(0, 10).ToString();
            }
            for (int i = 0; i < 3; i++)
            {
                cvv += random.Next(0, 10).ToString();
            }
            for (int i = 0; i < 4; i++)
            {
                pincode += random.Next(0, 10).ToString();
            }
            adapter.InsertQuery(cardnumber, cvv, DateTime.Today.AddYears(10), pincode, "активна", Convert.ToInt32(BankID));
            DataUpdate();
        }
        private void DataUpdate()
        {
            Cards.Clear();
            foreach (DataRow row in adapter.GetData())
            {
                if (row["id_bankAccount"].ToString() == BankID && row["State"].ToString() != "заблокирована")
                {
                    Cards.Add(row["CardNumber"].ToString());
                }
            }
            cards.ItemsSource = Cards;
            foreach (DataRow row in adapter.GetData())
            {
                if (row["CardNumber"].ToString() == cards.SelectedValue.ToString())
                {
                    PinCode.Text = row["PinCode"].ToString();
                    CardNumber.Text = row["CardNumber"].ToString();
                    string date = row["CardValidityPeriod"].ToString();
                    EndDate.Text = date[3] + "" + date[4] + "/" + date[8] + date[9];
                    CVV.Text = row["CVV"].ToString();
                    if (row["State"].ToString() == "заморожена")
                    {
                        Freeze.Content = "разморозить";
                    }
                    else
                    {
                        Freeze.Content = "Заморозить";
                    }
                }
            }
            cards.ItemsSource = Cards;
        }

        private void cards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataUpdate();
        }

        private void Freeze_Click(object sender, RoutedEventArgs e)
        {
            if (Freeze.Content.ToString() == "Заморозить")
            {
                adapter.UpdateQuery("заморожена", cards.SelectedValue.ToString());
            }
            else
            {
                adapter.UpdateQuery("активна", cards.SelectedValue.ToString());
            }
            DataUpdate();
        }

        private void Block_Click(object sender, RoutedEventArgs e)
        {
            adapter.UpdateQuery("заблокирована", cards.SelectedValue.ToString());
            DataUpdate();
        }
    }
}
