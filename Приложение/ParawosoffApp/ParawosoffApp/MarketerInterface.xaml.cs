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
    /// Логика взаимодействия для MarketerInterface.xaml
    /// </summary>
    public partial class MarketerInterface : Window
    {
        public MarketerInterface()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NamingSelect();
            CurSchemeQuerry = SchemeQuerry;
            SchemeFill();
            CurProductQuerry = ProductQuerry;
            ProductFill();
        }

        private void NamingSelect()
        {
            Action action = () =>
            {
                Table_Class name = new Table_Class(String.Format("select [Surname],[Name],[Firstname] from [dbo].[Staff] where [ID_Staff] = {0}", App.intID));
                this.Title = String.Format("Интерфейс менеджера отдела продаж: {0} {1} {2}", name.table.Rows[0][0].ToString(), name.table.Rows[0][1].ToString(), name.table.Rows[0][2].ToString());
            };
            action.Invoke();
        }

        string SchemeQuerry = "select [ID_Scheme] as \'Номер\', [SchemeName] as \'Название\', [Basis] as \'Основание\', [Reality] as \'Реализация\' from [dbo].[MarkScheme] WHERE [LogDelete] = 0";
        string CurSchemeQuerry = "";
        string ProductQuerry = "select [ID_Product] as \'Артикул\',[Name] as \'Название\', [Price] as \'Цена\', [Storage] as \'Остаток на складе\', (select Sum([Amount]) from [dbo].[Sell] where [Product_ID] = [ID_Product]) as \'Всего продаж\', (select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID]) as \'Текущая схема\' from [dbo].[Product] WHERE [ID_Product]>0 and [LogDelete] = 0";
        string CurProductQuerry = "";
        private void SchemeFill()
        {
            Action action = () =>
            {
                Table_Class scheme = new Table_Class(CurSchemeQuerry);
                scheme.Dependency.OnChange += SchemeDependency_OnChange;
                dgScheme.ItemsSource = scheme.table.DefaultView;
            };
            action.Invoke();
        }

        private void SchemeDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != System.Data.SqlClient.SqlNotificationInfo.Invalid)
                SchemeFill();
        }

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

        private void TbID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbProductID.Text.Length > 0) & (tbSchemeID.Text.Length > 0))
            {
                btApply.IsEnabled = true;
            }
            else
            {
                btApply.IsEnabled = false;
            }
            Tb_TextChanged(sender, e);
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool Selected = false;
            if ((tbSchemeID.Text.Length>0)&(tbSchemeID.Text != "0"))
            {
                btDelete.IsEnabled = true;
                Selected = true;
            }
            else
            {
                btDelete.IsEnabled = false;
                Selected = false;
            }
            if ((tbName.Text.Length>0)&(tbBasis.Text.Length > 0)&(tbReality.Text.Length>0))
            {
                btInsert.IsEnabled = true;
                if (Selected)
                    btUpdate.IsEnabled = true;
                else
                    btUpdate.IsEnabled = false;
            }
            else
            {
                btInsert.IsEnabled = false;
                btUpdate.IsEnabled = false;
            }

        }

        private void BtApply_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbProductID.Text); 
                array.Add(tbSchemeID.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Product_SchemeSet", array);
                ProductFill();
                tbProductID.Clear();
                tbSchemeID.Clear();
            };
            action.Invoke();
        }

        private void BtInsert_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbName.Text);
                array.Add(tbBasis.Text);
                array.Add(tbReality.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Scheme_Insert", array);
                SchemeFill();
                tbName.Clear();
                tbBasis.Clear();
                tbReality.Clear();
            };
            action.Invoke();
        }

        private void BtUpdate_Click(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                ArrayList array = new ArrayList();
                array.Add(tbSchemeID.Text);
                array.Add(tbName.Text);
                array.Add(tbBasis.Text);
                array.Add(tbReality.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Scheme_Update", array);
                SchemeFill();
                tbSchemeID.Clear();
                tbName.Clear();
                tbBasis.Clear();
                tbReality.Clear();
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
                    array.Add(tbSchemeID.Text);
                    if (MessageBox.Show("Удалить все связанные записи?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        array.Add("1");
                    }
                    else
                    {
                        array.Add("0");
                    }
                    Procedure_Class procedure = new Procedure_Class();
                    procedure.procedure_Execution("Scheme_LogDelete", array);
                    SchemeFill();
                    ProductFill();
                    tbSchemeID.Clear();
                }
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

        private void DgProduct_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Артикул")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void DgScheme_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Номер")
                e.Column.Visibility = Visibility.Hidden;
        }

        private void BtSSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSSearch.Text.Length >0)
            {
                CurSchemeQuerry = SchemeQuerry + String.Format("AND ((Convert(varchar,[ID_Scheme]) like \'%{0}%\')or(Convert(varchar,[SchemeName]) like \'%{0}%\')or(Convert(varchar,[Basis]) like \'%{0}%\')or(Convert(varchar,[Reality]) like \'%{0}%\'))", tbSSearch.Text);
            }
            else
            {
                CurSchemeQuerry = SchemeQuerry;
            }
            SchemeFill();
        }

        private void BtPSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbPSearch.Text.Length > 0)
            {
                CurProductQuerry = ProductQuerry + String.Format("AND ((Convert(varchar,[ID_Product]) like \'%{0}%\')or(Convert(varchar,[Name]) like \'%{0}%\')or(Convert(varchar,[Price]) like \'%{0}%\')or(Convert(varchar,[Storage]) like \'%{0}%\')or(Convert(varchar,(select Sum([Amount]) from [dbo].[Sell] where [Product_ID] = [ID_Product])) like \'%{0}%\')or(Convert(varchar,(select [SchemeName] from [dbo].[MarkScheme] where [ID_Scheme] = [Scheme_ID])) like \'%{0}%\'))", tbPSearch.Text);
            }
            else
            {
                CurProductQuerry = ProductQuerry;
            }
            ProductFill();
        }

        private void BtSelect_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
            Table_Class table = new Table_Class("select [SchemeName] as \'Название\', [Basis] as \'Основание\', [Reality] as \'Реализация\' from [dbo].[MarkScheme] WHERE [LogDelete] = 0");
            Document_class document = new Document_class();
            document.Document_Create(Document_class.Document_Type.MarkSchemes, save.FileName, table.table);
        }
    }
}
