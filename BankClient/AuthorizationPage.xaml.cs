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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        ClientTableAdapter adapter = new ClientTableAdapter();
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string id = "0";
            DataTable clientTable = adapter.GetData();
            string passport = Passport.Text;
            string pincode = PinCode.Text;
            bool pass = false;
            foreach (DataRow row in clientTable.Rows)
            {
                if (row["PassportData"].ToString() == passport && row["Pincode"].ToString() == pincode)
                {
                    pass = true;
                    id = row["id_client"].ToString();
                }
            }
            if (pass)
            {
                (Application.Current.MainWindow as MainWindow).MainFrame.Content = new ClientPage(id);
            }
            else
            {
                MessageBox.Show("неверно введен паспорт или пинкод");
            }
        }
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content =new RegistrationPage();
        }
    }
}
