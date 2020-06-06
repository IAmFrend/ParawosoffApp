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
    /// Логика взаимодействия для ManagerInterface.xaml
    /// </summary>
    public partial class ManagerInterface : Window
    {
        public ManagerInterface()
        {
            InitializeComponent();
            OrHeigth = this.Height;
            OrWidth = this.Width;
        }

        double OrHeigth = 0;
        double OrWidth = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NamingSelect();
            CurOrderQuerry = OrderQuerry;
            OrderFill();
            CurStaffQuerry = StaffQuerry;
            StaffFill();
        }

        public void TableRefresh()
        {
            NamingSelect();
            CurOrderQuerry = OrderQuerry;
            OrderFill();
            CurStaffQuerry = StaffQuerry;
            StaffFill();
        }

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", App.intID));
                this.Title = String.Format("Интерфейс менеджера по работе с персоналом: {0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
            };
            action.Invoke();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            switch (MessageBox.Show("Вы действительно хотите закрыть приложение?", "ParawosoffApp", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case (MessageBoxResult.Yes):
                    Application.Current.Shutdown();
                    break;
                case (MessageBoxResult.No):
                    this.Hide();
                    Owner.Show();
                    break;
            }
        }

        string StaffQuerry = "SELECT [ID_Staff] as \'Номер\',[Surname] as \'Фамилия\',[Name] as \'Имя\',[Firstname] as \'Отчество\',[Recruitment] as \'Дата приёма на работу\',[Exp] as \'Стаж (на момент приёма)\',[Exp] + DATEDIFF(YEAR,DATEFROMPARTS(SUBSTRING([Recruitment],7,4),SUBSTRING([Recruitment],4,2),SUBSTRING([Recruitment],1,2)),GETDATE()) as \'Стаж (текущий)\',(select [PositionName] from [dbo].[Position] where [ID_Position] = [Position_ID]) as \'Должность\',[LogDelete] as \'Уволен\',[TImestamp] as \'Последнее изменение\' FROM [dbo].[Staff]";
        string CurStaffQuerry = "";
        string OrderQuerry = "SELECT [ID_Order] as \'Номер\',[OrderName] as \'Название\',[LogDelete] as \'Сотрудник уволен\' FROM [dbo].[Order] WHERE";
        string CurOrderQuerry = "";

        private void StaffFill()
        {
            Action action = () =>
            {
                Table_Class staff = new Table_Class(CurStaffQuerry);
                dgEmployee.ItemsSource = staff.table.DefaultView;
            };
            action.Invoke();
        }

        private void OrderFill()
        {
            Action action = () =>
            {
                string SelectedStaff = "";
                if (tbStaffID.Text.Length >0)
                {
                    SelectedStaff = tbStaffID.Text;
                }
                else
                {
                    SelectedStaff = App.intID;
                }
                Table_Class order = new Table_Class(CurOrderQuerry + String.Format(" [Staff_ID] = {0}", SelectedStaff));
                dgOrder.ItemsSource = order.table.DefaultView;
            };
            action.Invoke();
        }

        private void BtESearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbESearch.Text.Length > 0)
            {
                CurStaffQuerry = StaffQuerry + String.Format("WHERE ((Convert(varchar,[Surname]) like \'%{0}%\')or(Convert(varchar,[Name]) like \'%{0}%\')or(Convert(varchar,[Firstname]) like \'%{0}%\')or(Convert(varchar,[Recruitment]) like \'%{0}%\')or(Convert(varchar,[Exp]) like \'%{0}%\')or(Convert(varchar,([Exp] + DATEDIFF(YEAR,DATEFROMPARTS(SUBSTRING([Recruitment],7,4),SUBSTRING([Recruitment],4,2),SUBSTRING([Recruitment],1,2)),GETDATE()))) like \'%{0}%\')or(Convert(varchar,(select [PositionName] from [dbo].[Position] where [ID_Position] = [Position_ID])) like \'%{0}%\')or(Convert(varchar,[LogDelete]) like \'%{0}%\')or(Convert(varchar,[TImestamp]) like \'%{0}%\'))", tbESearch.Text);
            }
            else
            {
                CurStaffQuerry = StaffQuerry;
            }
            StaffFill();
        }

        private void BtOSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbOSearch.Text.Length > 0)
            {
                CurOrderQuerry = OrderQuerry + String.Format("((Convert(varchar,[ID_Order]) like \'%{0}%\')or(Convert(varchar,[OrderName]) like \'%{0}%\')) AND", tbOSearch.Text);
            }
            else
            {
                CurOrderQuerry = OrderQuerry;
            }
            OrderFill();
        }

        private void BtForNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            NewEmpOrderForm newEmp = new NewEmpOrderForm();
            newEmp.Owner = this;
            newEmp.Show();
            this.Hide();
        }

        private void BtManagePositions_Click(object sender, RoutedEventArgs e)
        {
            PosForm pos = new PosForm();
            pos.Owner = this;
            pos.Show();
            this.Hide();
        }

        private void BtOwnerSettings_Click(object sender, RoutedEventArgs e)
        {
            OwnerSettings ownSettings = new OwnerSettings();
            ownSettings.Owner = this;
            ownSettings.Show();
            this.Hide();
        }

        private void BtForCurEmployee_Click(object sender, RoutedEventArgs e)
        {
            OrderForm order = new OrderForm();
            order.SelectedID = tbStaffID.Text;
            order.Owner = this;
            order.Show();
            this.Hide();
        }

        private void TbStaffID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbStaffID.Text.Length>0)
            {
                Table_Class table = new Table_Class(String.Format("select [LogDelete] from [dbo].[Staff] where [ID_Staff] = {0}", tbStaffID.Text));
                if (table.table.Rows[0][0].ToString() == "False")
                {
                    btForCurEmployee.IsEnabled = true;
                }
                else
                {
                    btForCurEmployee.IsEnabled = false;
                }
            }
            else
            {
                btForCurEmployee.IsEnabled = false;
            }
            OrderFill();
        }

        private void TbOrderID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbOrderID.Text.Length > 0)
            {
                //Table_Class table = new Table_Class(String.Format("select [LogDelete] from [dbo].[Order] where [ID_Order] = {0}", tbOrderID.Text));
                //if (table.table.Rows[0][0].ToString() == "False")
                //{
                //    btSelect.IsEnabled = true;
                //}
                //else
                //{
                //    btSelect.IsEnabled = false;
                //}
                btSelect.IsEnabled = true;
            }
            else
            {
                btSelect.IsEnabled = false;
            }
        }

        private void Dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Номер")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double Heigth = e.NewSize.Height;
            double Width = e.NewSize.Width;
            double HeigthInc = Heigth / OrHeigth;
            double WidthInc = Width / OrWidth;
            lblEmployee.FontSize = 14 * WidthInc* HeigthInc;
            dgEmployee.FontSize = 14 * WidthInc * HeigthInc;
            gdESearch.FontSize = 12 * WidthInc * HeigthInc;
            tbESearch.FontSize = 14 * WidthInc * HeigthInc;
            btESearch.FontSize = 14 * WidthInc * HeigthInc;
            lblOrder.FontSize = 14 * WidthInc * HeigthInc;
            dgOrder.FontSize = 14 * WidthInc * HeigthInc;
            gdOSearch.FontSize = 12 * WidthInc * HeigthInc;
            tbOSearch.FontSize = 14 * WidthInc * HeigthInc;
            btOSearch.FontSize = 14 * WidthInc * HeigthInc;
            btForNewEmployee.FontSize = 14 * WidthInc * HeigthInc;
            btForCurEmployee.FontSize = 14 * WidthInc * HeigthInc;
            btManagePositions.FontSize = 14 * WidthInc * HeigthInc;
            btSelect.FontSize = 14 * WidthInc * HeigthInc;
            btOwnerSettings.FontSize = 14 * WidthInc * HeigthInc;
        }

        private void BtSelect_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                SaveFileDialog save = new SaveFileDialog();
                save.ShowDialog();
                Table_Class table = new Table_Class(String.Format("select [OrderName],[OrderText] from [dbo].[Order] WHERE [ID_Order] = {0}", tbOrderID.Text));
                Document_class document = new Document_class();
                document.Document_Create(Document_class.Document_Type.Order, save.FileName,table.table);
                ArrayList array = new ArrayList();
                array.Add(tbOrderID.Text);
                array.Add(save.FileName);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Order_Doc_Create", array);
                tbOrderID.Clear();
            };
            action.Invoke();
        }
    }
}
