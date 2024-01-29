using BankClient.BANKDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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

    public partial class RegistrationPage : Page
    {
        Regex regex = new Regex(@"^[А-Яа-я]+$");
        Regex regex2 = new Regex(@"^[0-9]+$");
        ClientTableAdapter adapter = new ClientTableAdapter();
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private void Athorization_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AuthorizationPage();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string surname = SurName.Text;
            string name = Name.Text;
            string thirdname = ThirdName.Text;
            string pincode = PinCode.Text;
            string passport = Passport.Text;
            bool pass = true;
            if (surname.Length < 5 || surname.Length > 30 || !regex.IsMatch(surname))
            {
                MessageBox.Show("фамилия должна быть от 5 до 30 символов и содержать только русские буквы");
            } else if (name.Length < 2 || name.Length > 30 || !regex.IsMatch(name))
            {
                MessageBox.Show("Имя должно быть от 2 до 30 русских букв");
            } else if (pincode.Length != 4 || !regex2.IsMatch(pincode))
            {
                MessageBox.Show("пин код должен содержать 4 цифры");
            } else if (passport.Length != 10 || !regex2.IsMatch(passport))
            {
                MessageBox.Show("паспорт должен содержать 10 цифр");
            } else if (!BirthDate.SelectedDate.HasValue)
            {
                MessageBox.Show("нужно выбрать дату рождения");
            } else
            {
                DataTable clientTable = adapter.GetData();
                foreach (DataRow row in clientTable.Rows)
                {
                    if (row["PassportData"].ToString() == passport)
                    {
                        MessageBox.Show("номер паспорта уже используется");
                        pass = false;
                        break;
                    }
                }
                if (pass == true)
                {
                    DateTime dateTime = BirthDate.SelectedDate.Value;
                    adapter.InsertQuery(pincode, name, surname, thirdname, passport, dateTime);
                    (Application.Current.MainWindow as MainWindow).MainFrame.Content = new AuthorizationPage();
                }
            }
        }
    }
}