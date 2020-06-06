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
    /// Логика взаимодействия для PosForm.xaml
    /// </summary>
    public partial class PosForm : Window
    {
        public PosForm()
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
                Table_Class pos = new Table_Class("SELECT [ID_Position] as \'Номер\',[PositionName] as \'Название\',[Salary] as \'Оклад\' FROM [dbo].[Position]");
                dgPosition.ItemsSource = pos.table.DefaultView;
            };
            action.Invoke();
        }

        private void TbStaffID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((tbStaffID.Text.Length>0)&(tbStaffID.Text!="0"))
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
                ArrayList array = new ArrayList();
                array.Add(tbStaffID.Text);
                array.Add(tbName.Text);
                array.Add(tbSalary.Text);
                Procedure_Class procedure = new Procedure_Class();
                procedure.procedure_Execution("Pos_Update", array);
                tbStaffID.Clear();
            };
            action.Invoke();
            PosFill();
        }

        private void BtDrawback_Click(object sender, RoutedEventArgs e)
        {
            PosFill();
        }

        private void DgPosition_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
            lblName.FontSize = 14 * WidthInc * HeigthInc;
            lblPosition.FontSize = 14 * WidthInc * HeigthInc;
            lblSalary.FontSize = 14 * WidthInc * HeigthInc;
            dgPosition.FontSize = 14 * WidthInc * HeigthInc;
            tbName.FontSize = 14 * WidthInc * HeigthInc;
            tbSalary.FontSize = 14 * WidthInc * HeigthInc;
            btDrawback.FontSize = 14 * WidthInc * HeigthInc;
            btEnter.FontSize = 14 * WidthInc * HeigthInc;
        }
    }
}
