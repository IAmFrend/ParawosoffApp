using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ParawosoffApp;


namespace ParawosoffApp
{
    class Configuration_class
    {
        public event Action<DataTable> Server_Collection;
        //Коллекция серверов
        public event Action<DataTable> Data_Base_Collection;
        //Коллекция баз
        public event Action<bool> Conection_Checked;
        //Определяет статус подключения
        public string DS = "Empty", IC = "Empty"; //значения Data Source и Initial Catalog 
        public string ds = "";//Проверка подключения Data Source
        public static SqlConnection connection = new SqlConnection();
        public static string Organization_Name, Save_Files_Path, Machine_Name;
        public static Int32 doc_Left_Merge, doc_Right_Merge, doc_Top_Merge, doc_Bottom_Merge;
        /// <summary>
        /// Метод получения информации и строке подключения к БД
        /// </summary>

        public void SQL_Server_Configuration_Get()
        {
            //Каталог в одном из корней реестра
            RegistryKey registry = Registry.CurrentUser;
            //Создание ключа в корне
            RegistryKey key = registry.CreateSubKey("ParawosoffApp");
            try
            {
                //Получение данных из реестра
                DS = key.GetValue("DS").ToString();
                IC = key.GetValue("IC").ToString();
            }
            catch
            {
                DS = "Empty";
                IC = "Empty";
            }
            finally
            {
                //Обновление строки подключения
                connection.ConnectionString = "Data Source = " + DS + "; Initial Catalog = " + IC + "; Integrated Security = true;";
            }
        }
        /// <summary>
        /// Метод обновления информации о подключении к источнику данных
        /// </summary>
        /// <param name="ds"> Запись значения Data Source</param>
        /// <param name="ic"> Запись значения Initial Catalog</param>
        public void SQL_Server_Configuration_Set(string ds, string ic)
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey key = registry.CreateSubKey("ParawosoffApp");
            key.SetValue("DS", ds);//Запись значений в реестр
            key.SetValue("IC", ic);
            SQL_Server_Configuration_Get();
        }
        /// <summary>
        /// Метод обновления информации о подключении к источнику данных
        /// </summary>
        public void SQL_Server_Enumenator()
        {
            //Получение сведений о доступных серверах
            SqlDataSourceEnumerator sourceEnumerator = SqlDataSourceEnumerator.Instance;
            //Вывод серверов в виде таблицы
            Server_Collection(sourceEnumerator.GetDataSources());
        }
        /// <summary>
        /// Метод проверки подключения к источнику данных
        /// </summary>
        public void SQL_Data_Base_Checking()
        {
            try
            {
                connection.ConnectionString = "Data Source = " + ds + "; Initial Catalog = master; Integrated Security = true;";
                //Проверка и поддтвердение возможности открытия подключения
                connection.Open();
                Conection_Checked(true);
            }
            catch
            {
                Conection_Checked(true);
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// Метод получения списка Баз данных
        /// </summary>
        public void SQL_Data_Base_Collection()
        {
            //Вывод списка доступных баз данных
            SqlCommand command = new SqlCommand("select name from sys.databases where name not in ('master','tempdb','model','msdb') and name like '%ParawosoffDB%'", connection);
            //connection.ConnectionString = "Data Source = " + ds + "; Initial Catalog = master; Integrated Security = true;";
            try
            {
                //connection.Open();
                DataTable table = new DataTable();
                table.Load(command.ExecuteReader());
                Data_Base_Collection(table);
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Метод получения конфигурации документа
        /// </summary>
        public void Document_Configuration_Get()
        {
            RegistryKey registry = Registry.CurrentUser;
            RegistryKey key = registry.CreateSubKey("Doc_Configuration_Parawosoff");
            try
            {
                Organization_Name = key.GetValue("Organization_Name").ToString();
                doc_Left_Merge = Convert.ToInt32(key.GetValue("doc_Left_Merge").ToString());
                doc_Right_Merge = Convert.ToInt32(key.GetValue("doc_Right_Merge").ToString());
                doc_Top_Merge = Convert.ToInt32(key.GetValue("doc_Top_Merge").ToString());
                doc_Bottom_Merge = Convert.ToInt32(key.GetValue("doc_Bottom_Merge").ToString());
            }
            catch
            {
                Organization_Name = "Parawosoff";
                doc_Left_Merge = 0;
                doc_Right_Merge = 0;
                doc_Top_Merge = 0;
                doc_Bottom_Merge = 0;
            }
        }
    }
}

