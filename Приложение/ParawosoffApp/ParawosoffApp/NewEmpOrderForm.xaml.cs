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
    /// Логика взаимодействия для NewEmpOrderForm.xaml
    /// </summary>
    public partial class NewEmpOrderForm : Window
    {
        public NewEmpOrderForm()
        {
            InitializeComponent();
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbName.Text.Length>0)& (tbFirstName.Text.Length > 0) & (tbPatronymic.Text.Length > 0) & (tbExp.Text.Length > 0) & (tbLogin.Text.Length > 0) & (tbPassword.Text.Length > 5) & (cbPosition.SelectedIndex != -1))
            {
                btEnter.IsEnabled = true;
            }
            else
            {
                btEnter.IsEnabled = false;
            }
        }

        private void CbPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((tbName.Text.Length > 0) & (tbFirstName.Text.Length > 0) & (tbPatronymic.Text.Length > 0) & (tbExp.Text.Length > 0) & (tbLogin.Text.Length > 0) & (tbPassword.Text.Length > 0) & (cbPosition.SelectedIndex != 0))
            {
                btEnter.IsEnabled = true;
            }
            else
            {
                btEnter.IsEnabled = false;
            }
        }

        private void BtEnter_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                Table_Class table = new Table_Class(String.Format("select count (*) from [dbo].[Staff] where [Login] = \'{0}\'",tbLogin.Text));
                if (table.table.Rows[0][0].ToString()!="0")
                {
                    MessageBox.Show("Этот логин использовать нельзя!", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ArrayList staff = new ArrayList();
                    staff.Add(tbFirstName.Text);
                    staff.Add(tbName.Text);
                    staff.Add(tbPatronymic.Text);
                    staff.Add(tbExp.Text);
                    DES des = DES.Create();
                    byte[] key = Convert.FromBase64String("WdbWuvWCHPc=");
                    byte[] IV = Convert.FromBase64String("RuBs2bQBW58=");
                    des.Key = key;
                    des.IV = IV;
                    byte[] login = Crypt_Class.SymmetricEncrypt(tbLogin.Text, des);
                    byte[] passwd = Crypt_Class.SymmetricEncrypt(tbPassword.Text, des);
                    staff.Add(Convert.ToBase64String(login));
                    staff.Add(Convert.ToBase64String(passwd));
                    staff.Add(cbPosition.SelectedValue.ToString());
                    Procedure_Class procedure = new Procedure_Class();
                    procedure.procedure_Execution("Staff_Insert", staff);
                    Table_Class ID = new Table_Class("select [ID_Staff],[Timestamp] from [dbo].[Staff] WHERE [Timestamp] = (select MAX([Timestamp]) from [dbo].[Staff])");
                    ArrayList array = new ArrayList();
                    array.Add(String.Format("Приём на работу ({0})",ID.table.Rows[0][0].ToString()));
                    Table_Class pos = new Table_Class(String.Format("select [PositionName] from [dbo].[Position] where [ID_Position] = \'{0}\'", cbPosition.SelectedValue.ToString()));
                    array.Add(String.Format("Принять сотрудника {0} {1} {2} на должность {3}. Дата: {4}", tbFirstName.Text, tbName.Text, tbPatronymic.Text, pos.table.Rows[0][0].ToString() ,ID.table.Rows[0][1].ToString()));
                    array.Add(ID.table.Rows[0][0].ToString());
                    procedure.procedure_Execution("Order_Insert", array);
                    this.Hide();
                    (Owner as ManagerInterface).TableRefresh();
                    Owner.Show();
                }
            };
            action.Invoke();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            switch (MessageBox.Show("Вы действительно хотите вернуться к основной странице?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case (MessageBoxResult.Yes):
                    this.Hide();
                    (Owner as ManagerInterface).TableRefresh();
                    Owner.Show();
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PosFill();
        }

        private void PosFill()
        {
            Action action = () =>
            {
                Table_Class pos = new Table_Class("SELECT [ID_Position],[PositionName] FROM [dbo].[Position] WHERE [ID_Position]>0 and [LogDelete] = 0");
                cbPosition.ItemsSource = pos.table.DefaultView;
                cbPosition.DisplayMemberPath = "PositionName";
                cbPosition.SelectedValuePath = "ID_Position";
            };
            action.Invoke();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TbExp_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
    }
}
