using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.Win32;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using ParawosoffApp;
using System.Collections;
using ParawosoffCrypt;
using System.Security.Cryptography;

namespace ParawosoffApp
{
    /// <summary>
    /// Логика взаимодействия для OwnerSettings.xaml
    /// </summary>
    public partial class OwnerSettings : Window
    {
        public OwnerSettings()
        {
            InitializeComponent();
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbLogin.Text.Length > 0) & (tbPassword.Text.Length > 5))
            {
                btEnter.IsEnabled = true;
            }
            else
            {
                btEnter.IsEnabled = false;
            }
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            if ((MessageBox.Show("Вы действительно хотите выйти без изменения данных?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question)) == MessageBoxResult.Yes)
            {
                this.Hide();
                Owner.Show();
            }
        }

        private void BtEnter_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(App.intID);
                array.Add(Surname);
                array.Add(Name);
                array.Add(Firstname);
                array.Add(Recruitment);
                array.Add(Exp);
                DES des = DES.Create();
                byte[] key = Convert.FromBase64String("WdbWuvWCHPc=");
                byte[] IV = Convert.FromBase64String("RuBs2bQBW58=");
                des.Key = key;
                des.IV = IV;
                byte[] login = Crypt_Class.SymmetricEncrypt(tbLogin.Text, des);
                byte[] passwd = Crypt_Class.SymmetricEncrypt(tbPassword.Text, des);
                array.Add(Convert.ToBase64String(login));
                array.Add(Convert.ToBase64String(passwd));
                array.Add(Pos);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Staff_Update", array);
                MessageBox.Show("Данные успешно изменены", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Information);
                BasicsSelect();
                this.Hide();
                Owner.Show();
            };
            action.Invoke();
        }

        private void BtDrawback_Click(object sender, RoutedEventArgs e)
        {
            BasicsSelect();
        }

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", App.intID));
                lblTitle.Content = String.Format("Интерфейс управления личными данными: {0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
            };
            action.Invoke();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NamingSelect();
            BasicsSelect();
        }

        string Surname = "";
        string Name = "";
        string Firstname = "";
        string Recruitment = "";
        string Exp = "";
        string Pos = "";

        private void BasicsSelect()
        {
            Action action = () =>
            {
                Table_Class basics = new Table_Class(String.Format("SELECT [Surname],[Name],[Firstname],[Recruitment],[Exp],[Login],[Position_ID] FROM [dbo].[Staff] WHERE [ID_Staff] = {0}", App.intID));
                Surname = basics.table.Rows[0][0].ToString();
                Name = basics.table.Rows[0][1].ToString();
                Firstname = basics.table.Rows[0][2].ToString();
                Recruitment = basics.table.Rows[0][3].ToString();
                Exp = basics.table.Rows[0][4].ToString();
                Pos = basics.table.Rows[0][6].ToString();
                DES des = DES.Create();
                byte[] key = Convert.FromBase64String("WdbWuvWCHPc=");
                byte[] IV = Convert.FromBase64String("RuBs2bQBW58=");
                des.Key = key;
                des.IV = IV;
                string login = Crypt_Class.SymmetricDecrypt(Convert.FromBase64String(basics.table.Rows[0][5].ToString()), des);
                tbLogin.Text = login;
                tbPassword.Clear();
            };
            action.Invoke();
        }
    }
}
