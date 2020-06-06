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

namespace ParawosoffApp
{
    /// <summary>
    /// Логика взаимодействия для OrderForm.xaml
    /// </summary>
    public partial class OrderForm : Window
    {
        public string SelectedID = "";
        public OrderForm()
        {
            InitializeComponent();
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
            NamingSelect();
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

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", SelectedID));
                lblCurStaff.Content = String.Format("{0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
            };
            action.Invoke();
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case (0):
                    lblText.Content = "Причина увольнения";
                    cbPosition.SelectedIndex = -1;
                    cbPosition.IsEnabled = false;
                    tbText.IsEnabled = true;
                    break;
                case (1):
                    lblText.Content = "Причина перевода";
                    cbPosition.IsEnabled = true;
                    tbText.IsEnabled = true;
                    break;
                case (2):
                    lblText.Content = "Текст приказа (другое)";
                    cbPosition.SelectedIndex = -1;
                    cbPosition.IsEnabled = false;
                    tbText.IsEnabled = true;
                    break;
                default:
                    lblText.Content = "Выберите тип приказа";
                    cbPosition.SelectedIndex = -1;
                    cbPosition.IsEnabled = false;
                    tbText.IsEnabled = false;
                    break;
            }
            EnableButton();
        }

        private void TbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void CbPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            if (tbText.IsEnabled)
            {
                if (tbText.Text.Length > 0)
                {
                    if (cbPosition.IsEnabled)
                    {
                        if (cbPosition.SelectedIndex != -1)
                        {
                            btEnter.IsEnabled = true;
                        }
                        else
                        {
                            btEnter.IsEnabled = false;
                        }
                    }
                    else
                    {
                        btEnter.IsEnabled = true;
                    }
                }
                else
                {
                    btEnter.IsEnabled = false;
                }
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
                Procedure_Class procedure = new Procedure_Class();
                ArrayList array = new ArrayList();
                Table_Class number = new Table_Class(String.Format("select count (*) +1 from [dbo].[Order] where [Staff_ID] = {0}", SelectedID));
                switch (cbType.SelectedIndex)
                {
                    case (0):
                        array.Add(String.Format("Увольнение сотрудника {0}", SelectedID));
                        array.Add(String.Format("Уволить сотрудника {0} по причине: {1}. Дата (местная): {2}", lblCurStaff.Content, tbText.Text, DateTime.Now.ToString()));
                        array.Add(SelectedID);
                        procedure.procedure_Execution("Order_Insert", array);
                        array.Clear();
                        array.Add(SelectedID);
                        array.Add("1");
                        procedure.procedure_Execution("Staff_LogDelete", array);
                        array.Clear();
                        break;
                    case (1):
                        Table_Class pos = new Table_Class(String.Format("select [PositionName] from [dbo].[Position] where [ID_Position] = \'{0}\'", cbPosition.SelectedValue.ToString()));
                        array.Add(String.Format("Перевод сотрудника {0} (№ {1})", SelectedID, number.table.Rows[0][0].ToString()));
                        array.Add(String.Format("Перевести сотрудника {0} на должность {1} по причине: {2}. Дата (местная): {3}", lblCurStaff.Content, pos.table.Rows[0][0].ToString(), tbText.Text, DateTime.Now.ToString()));
                        array.Add(SelectedID);
                        procedure.procedure_Execution("Order_Insert", array);
                        array.Clear();
                        array.Add(SelectedID);
                        array.Add(cbPosition.SelectedValue.ToString());
                        procedure.procedure_Execution("Staff_Reposition", array);
                        array.Clear();
                        break;
                    case (2):
                        array.Add(String.Format("Особый приказ для сотрудника {0} (№ {1})", SelectedID, number.table.Rows[0][0].ToString()));
                        array.Add(String.Format("{1}. Сотрудник: {0}. Дата (местная): {2}", lblCurStaff.Content, tbText.Text, DateTime.Now.ToString()));
                        array.Add(SelectedID);
                        procedure.procedure_Execution("Order_Insert", array);
                        break;
                }
                cbType.SelectedIndex = -1;
                cbPosition.SelectedIndex = -1;
                tbText.Clear();
            };
            action.Invoke();
            this.Hide();
            (Owner as ManagerInterface).TableRefresh();
            Owner.Show();
        }
    }
}