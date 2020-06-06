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
    /// Логика взаимодействия для SaleInterface.xaml
    /// </summary>
    public partial class SaleInterface : Window
    {
        public SaleInterface()
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
            CurProductQuerry = ProductQuerry;
            ProductFill();
            CurSellQuerry = SellQuerry;
            SellFill();
        }

        public void TableRefresh()
        {
            NamingSelect();
            CurProductQuerry = ProductQuerry;
            ProductFill();
            CurSellQuerry = SellQuerry;
            SellFill();
            tbPSearch.Clear();
            tbSSearch.Clear();
        }

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", App.intID));
                this.Title = String.Format("Интерфейс продавца-кассира: {0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
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

        private void BtOwnerSettings_Click(object sender, RoutedEventArgs e)
        {
            OwnerSettings ownSettings = new OwnerSettings();
            ownSettings.Owner = this;
            ownSettings.Show();
            this.Hide();
        }

        string ProductQuerry = "SELECT [Name] as \'Название\',[Storage] as \'Количество\',[Price] as \'Цена\',[FirstDec] as \'Дата первой поставки\',[Timestamp] as \'Дата последнего изменения\',(select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID]) as \'Текущая маркетинговая схема\',(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = [Type_ID]) as \'Текущий тип\' FROM [dbo].[Product] WHERE [LogDelete] = 0 and [ID_Product]>0";
        string CurProductQuerry = "";
        string SellQuerry = "SELECT [Check] as \'Чек\',[Amount] as \'Количество товара\',[SellDate] as \'Дата продажи\',[Price] as \'Сумма позиции\',(select [Name] from [dbo].[Product] where [ID_Product] = [Product_ID]) as \'Товар\',(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = (select [Type_ID] from [dbo].[Product] where [ID_Product] = [Product_ID])) as \'Тип товара\' FROM [dbo].[Sell] WHERE [LogDelete] = 0 AND [ID_Sell] > 0";
        string CurSellQuerry = "";

        private void ProductFill()
        {
            Action action = () =>
            {
                Table_Class product = new Table_Class(CurProductQuerry);
                dgProduct.ItemsSource = product.table.DefaultView;
            };
            action.Invoke();
        }

        private void SellFill()
        {
            Action action = () =>
            {
                Table_Class sell = new Table_Class(CurSellQuerry + String.Format("AND [Staff_ID] = 0", App.intID));
                dgSell.ItemsSource = sell.table.DefaultView;
            };
            action.Invoke();
        }

        private void BtPSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbPSearch.Text.Length > 0)
            {
                CurProductQuerry = ProductQuerry + String.Format("AND ((Convert(varchar,[Name])like \'%{0}%\')or(Convert(varchar,[Storage])like \'%{0}%\')or(Convert(varchar,[Price])like \'%{0}%\')or(Convert(varchar,[FirstDec])like \'%{0}%\')or(Convert(varchar,[Timestamp])like \'%{0}%\')or(Convert(varchar,(select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID]))like \'%{0}%\')or(Convert(varchar,(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = [Type_ID]))like \'%{0}%\'))", tbPSearch.Text);
            }
            else
            {
                CurProductQuerry = ProductQuerry;
            }
            ProductFill();
        }

        private void BtSSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSSearch.Text.Length > 0)
            {
                CurSellQuerry = SellQuerry + String.Format("AND ((Convert(varchar,[Check]) like \'%{0}%\')or(Convert(varchar,[Amount])like \'%{0}%\')or(Convert(varchar,[SellDate])like \'%{0}%\')or(Convert(varchar,[Price])like \'%{0}%\')or(Convert(varchar,(select SUM([Price]) from [dbo].[Sell] where [dbo].[Sell].[Check] = [Check]))like \'%{0}%\')or(Convert(varchar,(select [Name] from [dbo].[Product] where [ID_Product] = [Product_ID]))like \'%{0}%\')or(Convert(varchar,(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = (select [Type_ID] from [dbo].[Product] where [ID_Product] = [Product_ID])))like \'%{0}%\'))", tbSSearch.Text);
            }
            else
            {
                CurSellQuerry = SellQuerry;
            }
            SellFill();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            SellAdd sellAdd = new SellAdd();
            sellAdd.Owner = this;
            sellAdd.Show();
            this.Hide();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double Heigth = e.NewSize.Height;
            double Width = e.NewSize.Width;
            double HeigthInc = Heigth / OrHeigth;
            double WidthInc = Width / OrWidth;
            lblProduct.FontSize = 14 * WidthInc * HeigthInc;
            lblSell.FontSize = 14 * WidthInc * HeigthInc;
            dgProduct.FontSize = 14 * WidthInc * HeigthInc;
            dgSell.FontSize = 14 * WidthInc * HeigthInc;
            btAdd.FontSize = 14 * WidthInc * HeigthInc;
            btPSearch.FontSize = 14 * WidthInc * HeigthInc;
            btSSearch.FontSize = 14 * WidthInc * HeigthInc;
            btOwnerSettings.FontSize = 14 * WidthInc * HeigthInc;
            btSelect.FontSize = 14 * WidthInc * HeigthInc;
            gdPSearch.FontSize = 12 * WidthInc * HeigthInc;
            gdSSearch.FontSize = 12 * WidthInc * HeigthInc;
        }

        private void TbCheck_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCheck.Text.Length>0)
            {
                btSelect.IsEnabled = true;
            }
            else
            {
                btSelect.IsEnabled = false;
            }
        }

        private void BtSelect_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
            Table_Class table = new Table_Class(String.Format("SELECT [Check] as \'Чек\',(select [Surname] from [dbo].[Staff] where [ID_Staff] = [Staff_ID]) as \'Фамилия\',(select [Name] from [dbo].[Staff] where [ID_Staff] = [Staff_ID]) as \'Имя\',(select [FirstName] from [dbo].[Staff] where [ID_Staff] = [Staff_ID]) as \'Отчество\', (select [Name] from [dbo].[Product] where [ID_Product] = [Product_ID]) as \'Товар\', (select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = (select [Scheme_ID] from [dbo].[Product] where [ID_Product] = [Product_ID])) as \'Схема\',[Amount] as \'Количество\',[SellDate] as \'Дата\',[Price] as \'Сумма\' FROM [dbo].[Sell] WHERE [Check] = \'{0}\'", tbCheck.Text));
            Document_class document = new Document_class();
            document.Document_Create(Document_class.Document_Type.Check, save.FileName, table.table);
            ArrayList array = new ArrayList();
            array.Add(tbCheck.Text);
            array.Add(save.FileName);
            Procedure_Class procedure = new Procedure_Class();
            procedure.procedure_Execution("Sell_Doc_Create", array);
            tbCheck.Clear();
        }
    }
}
