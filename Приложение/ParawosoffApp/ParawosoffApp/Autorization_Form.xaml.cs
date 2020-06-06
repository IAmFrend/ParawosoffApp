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
    /// Логика взаимодействия для Autorization_Form.xaml
    /// </summary>
    public partial class Autorization_Form : Window
    {
        public Autorization_Form()
        {
            InitializeComponent();
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbLogin.Text.Length>0)&(tbPassword.Password.Length>0))
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
            if ((MessageBox.Show("Вы действительно хотите выйти?","ParawosoffApp",MessageBoxButton.YesNo,MessageBoxImage.Question)) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void BtEnter_Click(object sender, RoutedEventArgs e)
        {
            Autorization();
        }

        private void Autorization()
        {
            Action action = () =>
            {
                DES des = DES.Create();
                byte[] key = Convert.FromBase64String("WdbWuvWCHPc=");
                byte[] IV = Convert.FromBase64String("RuBs2bQBW58=");
                des.Key = key;
                des.IV = IV;
                byte[] login = Crypt_Class.SymmetricEncrypt(tbLogin.Text, des);
                byte[] passwd = Crypt_Class.SymmetricEncrypt(tbPassword.Password, des);
                ArrayList array = new ArrayList();
                array.Add(Convert.ToBase64String(login));
                array.Add(Convert.ToBase64String(passwd));
                tbLogin.Clear();
                tbPassword.Clear();
                Function_class autoriz = new Function_class("Autorization", Function_class.Function_Result.scalar, array);
                if (autoriz.Regtable.Rows[0][0].ToString() == "-1")
                {
                    MessageBox.Show("Учётная запись с такими параметрами не существует или является заблокированной!", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    App.intID = autoriz.Regtable.Rows[0][0].ToString();
                    Table_Class tablePos = new Table_Class(String.Format("select [Position_ID] from [dbo].[Staff] where [ID_Staff] = {0}", autoriz.Regtable.Rows[0][0].ToString()));
                    switch (tablePos.table.Rows[0][0].ToString())
                    {
                        case ("0"):
                            Selector selector = new Selector();
                            selector.Owner = this;
                            selector.Show();
                            this.Hide();
                            break;
                        case ("1"):
                            SaleInterface sale = new SaleInterface();
                            sale.Owner = this;
                            sale.Show();
                            this.Hide();
                            break;
                        case ("2"):
                            ManagerInterface manager = new ManagerInterface();
                            manager.Owner = this;
                            manager.Show();
                            this.Hide();
                            break;
                        case ("3"):
                            StoragerInterface storager = new StoragerInterface();
                            storager.Owner = this;
                            storager.Show();
                            this.Hide();
                            break;
                        case ("4"):
                            MarketerInterface marketer = new MarketerInterface();
                            marketer.Owner = this;
                            marketer.Show();
                            this.Hide();
                            break;
                    }
                }
            };
            action.Invoke();
        }

        private void TbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if ((tbLogin.Text.Length > 0) & (tbPassword.Password.Length > 0))
            {
                btEnter.IsEnabled = true;
            }
            else
            {
                btEnter.IsEnabled = false;
            }
        }
    }
}
