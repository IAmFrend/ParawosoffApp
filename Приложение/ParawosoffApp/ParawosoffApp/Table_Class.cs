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
    class Table_Class
    {
        //Виртуальная таблица
        public DataTable table = new DataTable();
        //Переменная команды SQL
        private SqlCommand command = new SqlCommand("", Configuration_class.connection);
        //Прослушивание БД
        public SqlDependency Dependency = new SqlDependency();
        ///<summary>
        ///Заполнение таблицы в зависимости от запроса
        ///</summary>
        ///<param_name="SQL_Select_Querry">Переменная запроса</param_name>
        public Table_Class(string SQL_Select_Querry)
        {
            command.Notification = null;//Отключение оповещений для прослушивания
            command.CommandText = SQL_Select_Querry;//Перевод строки запроса в комманду
            Dependency.AddCommandDependency(command);//Прослушивание БД по команде
            try
            {
                //Запуск прослушивания
                SqlDependency.Start(Configuration_class.connection.ConnectionString);
                //Открытие подключения
                Configuration_class.connection.Open();
                //Запись данных в таблицу
                table.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                //Вывод сообщения об ошибке
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                Configuration_class.connection.Close();
            }
        }
    }
}
