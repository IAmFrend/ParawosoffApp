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

namespace ParawosoffApp
{
    /// <summary>
    /// Логика взаимодействия для Selector.xaml
    /// </summary>
    public partial class Selector : Window
    {
        public Selector()
        {
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            if ((MessageBox.Show("Вы действительно хотите выйти?", "ParawosoffApp", MessageBoxButton.YesNo, MessageBoxImage.Question)) == MessageBoxResult.Yes)
            {
                Owner.Show();
                this.Hide();
            }
        }

        private void BtManager_Click(object sender, RoutedEventArgs e)
        {
            ManagerInterface Manager = new ManagerInterface();
            Manager.Owner = this;
            Manager.Show();
            this.Hide();
        }

        private void BtMarketer_Click(object sender, RoutedEventArgs e)
        {
            MarketerInterface Marketer = new MarketerInterface();
            Marketer.Owner = this;
            Marketer.Show();
            this.Hide();
        }

        private void BtStorager_Click(object sender, RoutedEventArgs e)
        {
            StoragerInterface Storager = new StoragerInterface();
            Storager.Owner = this;
            Storager.Show();
            this.Hide();
        }

        private void BtSaler_Click(object sender, RoutedEventArgs e)
        {
            SaleInterface sale = new SaleInterface();
            sale.Owner = this;
            sale.Show();
            this.Hide();
        }
    }
}
