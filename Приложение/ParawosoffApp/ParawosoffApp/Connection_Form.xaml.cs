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
using System.Windows.Navigation;
using System.Management;
using System.Runtime.InteropServices;


namespace ParawosoffApp
{
    /// <summary>
    /// Логика взаимодействия для Connection_Form.xaml
    /// </summary>
    public partial class Connection_Form : Window
    {
        [DllImport("wininet.dll")] private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKylobytes);
        public Connection_Form()
        {
            SystemCheck();
            if (Startup)
            {
                InitializeComponent();
            }
            else
            {
                Close();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void SystemCheck()
        {
            int Major = Environment.OSVersion.Version.Major;
            int Minor = Environment.OSVersion.Version.Minor;
            if ((Major >= 6) && (Minor >= 0))
            {
                RegistryKey registrySQL =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                RegistryKey registryNET =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework");
                RegistryKey registryExcel =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Excel");
                RegistryKey registryWord =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\16.0\Word");

                RegistryKey freckey = Registry.LocalMachine;
                freckey = freckey.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
                string str = freckey.GetValue("~MHz").ToString();
                if (Convert.ToInt32(str) <= 1400)
                {
                    MessageBox.Show(String.Format("Данное приложение не может запуститься с такой тактовой частотой: {0}", str));
                    Startup = false;
                }

                GetSystemInfo();

                if (registrySQL == null)
                {
                    MessageBox.Show("Запуск системы невозможен, в системе отсутствует Microsoft SQL Server", "ParawosoffApp");
                    Startup = false;
                }
                else if (registryNET == null)
                {
                    MessageBox.Show("Запуск системы невозможен, в системе отсутствует .NETFramework", "ParawosoffApp");
                    Startup = false;
                }
                else if (registryExcel == null)
                {
                    MessageBox.Show("Запуск системы невозможен, в системе отсутствует Microsoft Excel", "ParawosoffApp");
                    Startup = false;
                }

                else if (registryWord == null)
                {
                    MessageBox.Show("Запуск системы невозможен, в системе отсутствует Microsoft Word", "ParawosoffApp");
                    Startup = false;
                }
            }
            else
            {
                MessageBox.Show("Данная операционная система не предназначена для запуска приложения!", "ParawosoffApp");
            }
        }
        Boolean Startup = true;
        void GetSystemInfo()
        {
            long memKb;
            GetPhysicallyInstalledSystemMemory(out memKb);
            memKb = memKb / 1024 / 1024;
            if (memKb < 4)
            {
                MessageBox.Show(String.Format("Данное приложение не запустится с таким количеством памяти ОЗУ: {0} Гб", memKb.ToString()));
                Startup = false;
            }
            if (IsConnectedToInternet() == false)
            {
                MessageBox.Show(String.Format("Отсутствует подключение к интернету"));
                Startup = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //bool Create_App;
            //App.instanse = new Mutex(true, App.app_Name, out Create_App);
            //if (Create_App)
            //{
                try
                {
                    //Проверка подключения
                    Configuration_class configuration = new Configuration_class();
                    configuration.SQL_Server_Configuration_Get();
                    Configuration_class.connection.Open();
                    App.connect = true;
                }
                catch
                {
                    //Загрузка резервной формы
                    Configuration_class configuration = new Configuration_class();
                    configuration.Server_Collection += Configuration_Server_Collection;
                //Thread threadServers = new Thread(configuration.SQL_Server_Enumenator);
                //threadServers.Start();
                configuration.SQL_Server_Enumenator();
                }
                finally
                {
                    Configuration_class.connection.Close();
                    //Проверка подключения
                    switch (App.connect)
                    {
                        //Подключение не установлено
                        case false:
                            MessageBox.Show("Ошибка подключения к источнику данных", "PaladinApp", MessageBoxButton.OK, MessageBoxImage.Error);
                            //Environment.Exit(0);
                            break;
                        //Подключение установлено
                        case true:
                            try
                            {
                                Autorization_Form autorization_Form = new Autorization_Form();
                                autorization_Form.Show();
                                this.Hide();
                            }
                            catch
                            {

                            }
                            break;
                    }
                }
            //}
            //else
            //{
            //    Application.Current.Shutdown();
            //}
        }

        private void Configuration_Server_Collection(DataTable obj)
        {
            Action action = () =>
            {
                foreach (DataRow r in obj.Rows)
                {
                    cbServer.Items.Add(string.Format(@"{0}\{1}", r[0], r[1]));
                }
                cbServer.IsEnabled = true;
                btChecked.IsEnabled = true;
            };
            action.Invoke();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CbServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Configuration_class configuration = new Configuration_class();
            configuration.ds = cbServer.SelectedItem.ToString();
            configuration.Conection_Checked += Configuration_Conection_Checked;
            //Thread thread = new Thread(configuration.SQL_Data_Base_Checking);
            //thread.Start();
            configuration.SQL_Data_Base_Checking();
        }

        private void Configuration_Conection_Checked(bool obj)
        {
            switch (obj)
            {
                case true:
                    MessageBox.Show("Sukcess");
                    Action action = () =>
                    {
                        //Заполнение списка БД
                        Configuration_class configuration_call = new Configuration_class();
                        configuration_call.Data_Base_Collection += Configuration_Data_Base_Collection;
                        //Thread threadBases = new Thread(configuration_call.SQL_Data_Base_Collection);
                        //threadBases.Start();
                        configuration_call.ds = cbServer.SelectedItem.ToString();
                        configuration_call.SQL_Data_Base_Collection();
                        btConnect.IsEnabled = true;
                    };
                    action.Invoke();
                    break;
                case false:
                    //ПОвторное сканирование
                    Configuration_class configuration = new Configuration_class();
                    configuration.Server_Collection += Configuration_Server_Collection;
                    //Thread threadServers = new Thread(configuration.SQL_Server_Enumenator);
                    //threadServers.Start();
                    configuration.SQL_Server_Enumenator();
                    btConnect.IsEnabled = false;
                    break;
            }
        }

        private void Configuration_Data_Base_Collection(DataTable obj)
        {
            Action action = () =>
            {
                foreach (DataRow r in obj.Rows)
                {
                    cbDatabase.Items.Add(r[0]);
                }
                cbDatabase.IsEnabled = true;
            };
            action.Invoke();
        }

        private void BtChecked_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sql = new SqlConnection(string.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security = true;", cbServer.Text, cbDatabase.Text));
            try
            {
                sql.Open();
                btConnect.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("Подключение не установлено!", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Error);
                btConnect.IsEnabled = false;
            }
            finally
            {
                sql.Close();
            }
        }

        private void BtConnect_Click(object sender, RoutedEventArgs e)
        {
            switch (cbDatabase.Text == "")
            {
                case true:
                    MessageBox.Show("Выберите базу данных!", "ParawosoffApp", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbDatabase.Focus();
                    break;
                case false:
                    Configuration_class configuration = new Configuration_class();
                    configuration.SQL_Server_Configuration_Set(cbServer.Text, cbDatabase.Text);
                    App.connect = true;
                    Autorization_Form autorization_Form = new Autorization_Form();
                    autorization_Form.Show();
                    this.Hide();
                    break;
            }
        }
    }
}
