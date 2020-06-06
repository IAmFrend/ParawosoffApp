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
    /// Логика взаимодействия для SellAdd.xaml
    /// </summary>
    public partial class SellAdd : Window
    {
        public SellAdd()
        {
            InitializeComponent();
            OrHeigth = this.Height;
            OrWidth = this.Width;
        }

        double OrHeigth = 0;
        double OrWidth = 0;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            switch (MessageBox.Show("Вы действительно хотите отменить добавление заказа?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case (MessageBoxResult.Yes):
                    this.Hide();
                    Owner.Show();
                    break;
            }
        }

        DataTable sell = new DataTable("Sell");
        DataColumn ID = new DataColumn();
        DataColumn PrName = new DataColumn();
        DataColumn Amount = new DataColumn();
        DataColumn Price = new DataColumn();
        DataColumn SumPrice = new DataColumn();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckNumber_Get();
            ProductFill();
            ID.DataType = System.Type.GetType("System.String");
            ID.AutoIncrement = false;
            ID.ColumnName = "Артикул";
            sell.Columns.Add(ID);
            PrName.DataType = System.Type.GetType("System.String");
            PrName.ColumnName = "Название";
            sell.Columns.Add(PrName);
            Amount.DataType = System.Type.GetType("System.Int32");
            Amount.ColumnName = "Количество";
            sell.Columns.Add(Amount);
            Price.DataType = System.Type.GetType("System.Int32");
            Price.ColumnName = "Цена";
            sell.Columns.Add(Price);
            SumPrice.DataType = System.Type.GetType("System.Int32");
            SumPrice.ColumnName = "Сумма";
            sell.Columns.Add(SumPrice);
            dgSell.ItemsSource = sell.DefaultView;
        }

        string CurCheck = "";

        private void CheckNumber_Get()
        {
            Function_class function = new Function_class("CheckGet", Function_class.Function_Result.scalar);
            CurCheck = function.Regtable.Rows[0][0].ToString();
            lblCheck.Content = "Номер чека: " + CurCheck;
        }

        private void ProductFill()
        {
            Action action = () =>
            {
                Table_Class product = new Table_Class("SELECT [ID_Product] as \'Артикул\',[Name] as \'Название\',[Storage] as \'Количество\',[Price] as \'Цена\' FROM [dbo].[Product] WHERE [ID_Product]>0 and [LogDelete] = 0 and [Storage]>0");
                dgProduct.ItemsSource = product.table.DefaultView;
            };
            action.Invoke();
        }

        private void TbNumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void DgProduct_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Артикул")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void TbProductID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbProductID.Text.Length > 0)
            {
                btAdd.IsEnabled = true;
            }
            else
            {
                btAdd.IsEnabled = false;
            }
            TbCurSellID_TextChanged(sender, e);
        }

        private void TbCurSellID_TextChanged(object sender, TextChangedEventArgs e)
        {
            Action action = () =>
            {
                if (tbReamount.Text.Length == 0)
                    tbReamount.Text = "1";
                else
                if (Convert.ToInt32(tbReamount.Text) <= 0)
                    tbReamount.Text = "1";
                if (tbCurSellID.Text.Length >0)
                {
                    btDelete.IsEnabled = true;
                    Table_Class table = new Table_Class(String.Format("select [Storage] from [dbo].[Product] where [ID_Product] = {0}", tbCurSellID.Text));
                    int Storage = Convert.ToInt32(table.table.Rows[0][0].ToString());
                    int Selected = Convert.ToInt32(tbReamount.Text);
                    if (Storage>=Selected)
                    {
                        btReamount.IsEnabled = true;
                    }
                    else
                    {
                        btReamount.IsEnabled = false;
                    }
                }
                else
                {
                    btReamount.IsEnabled = false;
                    btDelete.IsEnabled = false;
                }
            };
            action.Invoke();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                if (sell.Select(String.Format("Артикул = {0}", tbProductID.Text)).Length == 0)
                {
                    DataRow row = sell.NewRow();
                    row["Артикул"] = tbProductID.Text;
                    row["Название"] = tbProductName.Text;
                    row["Количество"] = 1;
                    row["Цена"] = Convert.ToInt32(tbProductPrice.Text);
                    row["Сумма"] = Convert.ToInt32(tbProductPrice.Text);
                    sell.Rows.Add(row);
                    dgSell.ItemsSource = sell.DefaultView;
                    tbProductID.Clear();
                    SumMath();
                    InsertUnlock();
                }
                else
                    MessageBox.Show("Этот товар уже добавлен!", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            action.Invoke();
        }

        private void BtReamount_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                DataRow addrow = sell.NewRow();
                addrow[0] = tbCurSellID.Text;
                addrow[1] = tbCurSellName.Text;
                addrow[2] = tbReamount.Text;
                addrow[3] = Convert.ToInt32(tbCurSellPrice.Text);
                addrow[4] = Convert.ToInt32(tbReamount.Text) *Convert.ToInt32(tbCurSellPrice.Text);
                DataRow row = sell.Select(String.Format("Артикул = {0}", tbCurSellID.Text))[0];
                sell.Rows.Remove(row);
                sell.Rows.Add(addrow);
                dgSell.ItemsSource = sell.DefaultView;
                tbCurSellID.Clear();
                tbProductID.Clear();
                InsertUnlock();
                SumMath();
            };
            action.Invoke();
        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                DataRow row = sell.Select(String.Format("Артикул = {0}", tbCurSellID.Text))[0];
                sell.Rows.Remove(row);
                dgSell.ItemsSource = sell.DefaultView;
                SumMath();
                tbCurSellID.Clear();
                tbProductID.Clear();
                InsertUnlock();
            };
            action.Invoke();
        }

        private void BtInsert_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                foreach (DataRow row in sell.Rows)
                {
                    ArrayList array = new ArrayList();
                    array.Add(CurCheck);
                    array.Add(row[2]);
                    array.Add(App.intID);
                    array.Add(row[0]);
                    Procedure_Class procedure = new Procedure_Class();
                    procedure.procedure_Execution("Sell_Insert", array);
                    array.Clear();
                }
                Owner.Show();
                (Owner as SaleInterface).TableRefresh();
                this.Hide();
            };
            action.Invoke();
        }

        private void SumMath()
        {
            Action action = () =>
            {
                int Sum = 0;
                foreach (DataRow row in sell.Rows)
                {
                    Sum += Convert.ToInt32(row[4]);
                }
                lblCurSum.Content = String.Format("{0} рублей",Sum.ToString());
            };
            action.Invoke();
        }

        private void InsertUnlock()
        {
            Action action = () =>
            {
                if (sell.Rows.Count>0)
                {
                    btInsert.IsEnabled = true;
                }
                else
                {
                    btInsert.IsEnabled = false;
                }
            };
            action.Invoke();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double Heigth = e.NewSize.Height;
            double Width = e.NewSize.Width;
            double HeigthInc = Heigth / OrHeigth;
            double WidthInc = Width / OrWidth;
            lblCheck.FontSize = 14 * WidthInc * HeigthInc;
            lblProduct.FontSize = 14 * WidthInc * HeigthInc;
            lblSell.FontSize = 14 * WidthInc * HeigthInc;
            lblSum.FontSize = 14 * WidthInc * HeigthInc;
            lblCurSum.FontSize = 16 * WidthInc * HeigthInc;
            dgProduct.FontSize = 14 * WidthInc * HeigthInc;
            dgSell.FontSize = 14 * WidthInc * HeigthInc;
            tbReamount.FontSize = 14 * WidthInc * HeigthInc;
            btAdd.FontSize = 14 * WidthInc * HeigthInc;
            btDelete.FontSize = 14 * WidthInc * HeigthInc;
            btReamount.FontSize = 14 * WidthInc * HeigthInc;
            btInsert.FontSize = 14 * WidthInc * HeigthInc;
        }
    }
}
