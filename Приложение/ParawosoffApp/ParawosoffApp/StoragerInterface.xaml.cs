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
    /// Логика взаимодействия для StoragerInterface.xaml
    /// </summary>
    public partial class StoragerInterface : Window
    {
        public StoragerInterface()
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
            CurTypeQuerry = TypeQuerry;
            TypeProductFill();
        }

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", App.intID));
                this.Title = String.Format("Интерфейс завскладом: {0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
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

        private void TbNumbersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
        string ProductQuerry = "SELECT [ID_Product] as \'Артикул\',[Name] as \'Название\',[Storage] as \'Количество\',[Price] as \'Цена\',[FirstDec] as \'Дата первой поставки\',[Timestamp] as \'Дата последнего изменения\',(select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID]) as \'Текущая маркетинговая схема\',(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = [Type_ID]) as \'Текущий тип\' FROM [dbo].[Product] WHERE [LogDelete] = 0 and [ID_Product]>0";
        string CurProductQuerry = "";
        string TypeQuerry = "SELECT [ID_ProductType] as \'Номер\',[TypeName] as \'Название\',[Terms] as \'Условия\' FROM [dbo].[ProductType] WHERE [LogDelete]=0";
        string CurTypeQuerry = "";
        private void ProductFill()
        {
            Action action = () =>
            {
                Table_Class product = new Table_Class(CurProductQuerry);
                product.Dependency.OnChange += ProductDependency_OnChange;
                dgProduct.ItemsSource = product.table.DefaultView;
            };
            action.Invoke();
        }

        private void ProductDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != System.Data.SqlClient.SqlNotificationInfo.Invalid)
                ProductFill();
        }

        private void TbProductID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbAmount.Text.Length == 0)
                tbAmount.Text = "0";
            if ((tbProductID.Text.Length>0)&(tbProductCurAmount.Text.Length > 0) & (Convert.ToInt32(tbAmount.Text)>0))
            {
                btAdd.IsEnabled = true;
                int CurAmount = Convert.ToInt32(tbProductCurAmount.Text);
                if (CurAmount >= Convert.ToInt32(tbAmount.Text))
                {
                    btRemove.IsEnabled = true;
                }
                else
                {
                    btRemove.IsEnabled = false;
                }
            }
            else
            {
                btAdd.IsEnabled = false;
                btRemove.IsEnabled = false;
            }
            if (tbProductID.Text.Length > 0)
            {
                Table_Class table = new Table_Class(String.Format("SELECT [ID_ProductType],[TypeName],[Terms] FROM [dbo].[ProductType] WHERE [LogDelete]=0 and [ID_ProductType] = (select [Type_ID] from [dbo].[Product] where [ID_Product] = {0})",tbProductID.Text));
                tbTypeID.Text = table.table.Rows[0][0].ToString();
                tbTypeName.Text = table.table.Rows[0][1].ToString();
                tbTerms.Text = table.table.Rows[0][2].ToString();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbProductID.Text);
                array.Add(tbAmount.Text);
                array.Add("1");
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Product_Reamount", array);
                tbProductID.Clear();
                tbProductCurAmount.Clear();
                ProductFill();
            };
            action.Invoke();
        }

        private void BtRemove_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbProductID.Text);
                array.Add(tbAmount.Text);
                array.Add("0");
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Product_Reamount", array);
                tbProductID.Clear();
                tbProductCurAmount.Clear();
                ProductFill();
            };
            action.Invoke();
        }

        private void BtOwnerSettings_Click(object sender, RoutedEventArgs e)
        {
            OwnerSettings ownSettings = new OwnerSettings();
            ownSettings.Owner = this;
            ownSettings.Show();
            this.Hide();
        }

        private void TypeProductFill()
        {
            Action action = () =>
            {
                Table_Class type = new Table_Class(CurTypeQuerry);
                type.Dependency.OnChange += TypeProductDependency_OnChange;
                dgType.ItemsSource = type.table.DefaultView;
            };
            action.Invoke();
        }

        private void TypeProductDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != System.Data.SqlClient.SqlNotificationInfo.Invalid)
                TypeProductFill();
        }

        private void TbTypeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbTypeName.Text.Length>0)& (tbTerms.Text.Length > 0))
            {
                btTypeInsert.IsEnabled = true;
            }
            else
            {
                btTypeInsert.IsEnabled = false;
            }
            if ((tbTypeID.Text.Length>0)&(tbTypeID.Text!="0"))
            {
                btTypeDelete.IsEnabled = true;
                if (btTypeInsert.IsEnabled)
                {
                    btTypeUpdate.IsEnabled = true;
                }
                else
                {
                    btTypeUpdate.IsEnabled = false;
                }
            }
            else
            {
                btTypeDelete.IsEnabled = false;
                btTypeUpdate.IsEnabled = false;
            }
            TbName_TextChanged(sender, e);
        }

        private void BtTypeInsert_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbTypeName.Text);
                array.Add(tbTerms.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Type_Insert", array);
                tbTypeName.Clear();
                tbTerms.Clear();
                TypeProductFill();
            };
            action.Invoke();
        }

        private void BtTypeUpdate_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbTypeID.Text);
                array.Add(tbTypeName.Text);
                array.Add(tbTerms.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Type_Update", array);
                tbTypeID.Clear();
                tbTypeName.Clear();
                tbTerms.Clear();
                TypeProductFill();
            };
            action.Invoke();
        }

        private void BtTypeDelete_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ArrayList array = new ArrayList();
                    array.Add(tbTypeID.Text);
                    if (MessageBox.Show("Удалить все связанные записи?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        array.Add("1");
                    }
                    else
                    {
                        array.Add("0");
                    }
                    Procedure_Class procedure = new Procedure_Class();
                    procedure.procedure_Execution("Type_LogDelete", array);
                    TypeProductFill();
                    tbTypeID.Clear();
                }
            };
            action.Invoke();
        }

        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPrice.Text.Length == 0)
                tbPrice.Text = "0";
            if ((tbName.Text.Length>0)&(Convert.ToInt32(tbPrice.Text) > 0)& (tbTypeID.Text.Length > 0))
            {
                btInsert.IsEnabled = true;
            }
            else
            {
                btInsert.IsEnabled = false;
            }
            if (tbProductID.Text.Length>0)
            {
                btDelete.IsEnabled = true;
                if (btInsert.IsEnabled)
                {
                    btUpdate.IsEnabled = true;
                }
                else
                {
                    btUpdate.IsEnabled = false;
                }
            }
            else
            {
                btDelete.IsEnabled = false;
                btUpdate.IsEnabled = false;
            }
        }

        private void BtInsert_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbName.Text);
                array.Add(tbPrice.Text);
                array.Add(tbTypeID.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Product_Insert", array);
                tbName.Clear();
                tbPrice.Clear();
                tbTypeID.Clear();
                tbTypeName.Clear();
                tbTerms.Clear();
                ProductFill();
            };
            action.Invoke();
        }

        private void BtUpdate_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbProductID.Text);
                array.Add(tbName.Text);
                array.Add(tbPrice.Text);
                Table_Class table = new Table_Class(String.Format("SELECT [FirstDec],[Scheme_ID]FROM [dbo].[Product] WHERE [ID_Product] = {0}",tbProductID.Text));
                if (MessageBox.Show("Изменить дату первого добавления?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string Day = DateTime.Now.Day.ToString();
                    if (Day.Length < 2)
                        Day = "0" + Day;
                    string Month = DateTime.Now.Month.ToString();
                    if (Month.Length < 2)
                        Month = "0" + Month;
                    array.Add(Day + "." + Month + "." + DateTime.Now.Year.ToString());
                }
                else
                {
                    array.Add(table.table.Rows[0][0].ToString());
                }
                array.Add(table.table.Rows[0][1].ToString());
                array.Add(tbTypeID.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Product_Update", array);
                tbProductID.Clear();
                tbName.Clear();
                tbPrice.Clear();
                tbTypeID.Clear();
                tbTypeName.Clear();
                tbTerms.Clear();
                ProductFill();
            };
            action.Invoke();
        }

        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ArrayList array = new ArrayList();
                    array.Add(tbProductID.Text);
                    if (MessageBox.Show("Удалить все связанные записи?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        array.Add("1");
                    }
                    else
                    {
                        array.Add("0");
                    }
                    Procedure_Class procedure = new Procedure_Class();
                    procedure.procedure_Execution("Product_LogDelete", array);
                    ProductFill();
                    tbProductID.Clear();
                }
            };
            action.Invoke();
        }

        private void DgProduct_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Артикул")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void DgType_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Номер")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void BtPSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbPSearch.Text.Length > 0)
            {
                CurProductQuerry = ProductQuerry + String.Format("AND ((Convert(varchar,[ID_Product]) like \'{0}\')or(Convert(varchar,[Name])like \'{0}\')or(Convert(varchar,[Storage])like \'{0}\')or(Convert(varchar,[Price])like \'{0}\')or(Convert(varchar,[FirstDec])like \'{0}\')or(Convert(varchar,[Timestamp])like \'{0}\')or(Convert(varchar,(select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID]))like \'{0}\')or(Convert(varchar,(select [TypeName] from [dbo].[ProductType] where [ID_ProductType] = [Type_ID]))like \'{0}\'))", tbPSearch.Text);
            }
            else
            {
                CurProductQuerry = ProductQuerry;
            }
            ProductFill();
        }

        private void BtTSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbTSearch.Text.Length > 0)
            {
                CurTypeQuerry = TypeQuerry + String.Format("AND ((Convert(varchar,[ID_ProductType]) like \'{0}\')or(Convert(varchar,[TypeName]) like \'{0}\')or(Convert(varchar,[Terms]) like \'{0}\'))", tbTSearch.Text);
            }
            else
            {
                CurTypeQuerry = TypeQuerry;
            }
            TypeProductFill();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double Heigth = e.NewSize.Height;
            double Width = e.NewSize.Width;
            double HeigthInc = Heigth / OrHeigth;
            double WidthInc = Width / OrWidth;
            lblName.FontSize = 14 * WidthInc * HeigthInc;
            lblPrice.FontSize = 14 * WidthInc * HeigthInc;
            lblProduct.FontSize = 14 * WidthInc * HeigthInc;
            lblTerms.FontSize = 14 * WidthInc * HeigthInc;
            lblType.FontSize = 14 * WidthInc * HeigthInc;
            lblTypeName.FontSize = 14 * WidthInc * HeigthInc;
            dgProduct.FontSize = 14 * WidthInc * HeigthInc;
            dgType.FontSize = 14 * WidthInc * HeigthInc;
            gdPSearch.FontSize = 12 * WidthInc * HeigthInc;
            gdTSearch.FontSize = 12 * WidthInc * HeigthInc;
            btAdd.FontSize = 14 * WidthInc * HeigthInc;
            btDelete.FontSize = 14 * WidthInc * HeigthInc;
            btInsert.FontSize = 14 * WidthInc * HeigthInc;
            btOwnerSettings.FontSize = 14 * WidthInc * HeigthInc;
            btPSearch.FontSize = 14 * WidthInc * HeigthInc;
            btRemove.FontSize = 14 * WidthInc * HeigthInc;
            btSelect.FontSize = 14 * WidthInc * HeigthInc;
            btTSearch.FontSize = 14 * WidthInc * HeigthInc;
            btTypeDelete.FontSize = 14 * WidthInc * HeigthInc;
            btTypeInsert.FontSize = 14 * WidthInc * HeigthInc;
            btTypeUpdate.FontSize = 14 * WidthInc * HeigthInc;
            btUpdate.FontSize = 14 * WidthInc * HeigthInc;
            tbPSearch.FontSize = 14 * WidthInc * HeigthInc;
            tbTSearch.FontSize = 14 * WidthInc * HeigthInc;
            tbName.FontSize = 14 * WidthInc * HeigthInc;
            tbPrice.FontSize = 14 * WidthInc * HeigthInc;
            tbTypeName.FontSize = 14 * WidthInc * HeigthInc;
            tbTerms.FontSize = 14 * WidthInc * HeigthInc;
            tbAmount.FontSize = 14 * WidthInc * HeigthInc;
        }

        private void BtSelect_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
            Table_Class table = new Table_Class("SELECT [Name],(select [TypeNAme] from [dbo].[ProductType] WHERE [Type_ID] = [ID_ProductType]),[Storage],[FirstDec],[Price] FROM [dbo].[Product] WHERE [ID_Product] >0 and [LogDelete] = 0");
            Document_class document = new Document_class();
            document.Document_Create(Document_class.Document_Type.Storage, save.FileName, table.table);
        }
    }
}
