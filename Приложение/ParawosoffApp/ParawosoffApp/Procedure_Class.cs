using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ParawosoffApp;
using System.Collections;

namespace ParawosoffApp
{
    class Procedure_Class
    {
        SqlCommand command = new SqlCommand("", Configuration_class.connection);
        ///<summary>
        ///Метод обращеия к хранимым процедурам
        ///</summary>
        ///<param name = "Procedure_Name">Название процедуры</param>
        ///<param name = "field_value">Лист значений</param>
        public void procedure_Execution(string Procedure_Name, ArrayList field_value)
        {
            //Запрос на вывод списка параметров
            Table_Class table = new Table_Class(string.Format("select name from sys.parameters where object_id = (select object_id from sys.procedures where name = '{0}')", Procedure_Name));
            try
            {
                //Установка команды в режим хранимой процедуры
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = string.Format("[dbo].{0}", Procedure_Name);
                command.Parameters.Clear();
                if (table.table.Rows.Count != 0)
                    for (int i = 0; i < table.table.Rows.Count; i++)
                    {
                        //Доавление параметров в процедуру
                        command.Parameters.AddWithValue(table.table.Rows[i][0].ToString(), field_value[i]);
                    }
                Configuration_class.connection.Open();
                Configuration_class.connection.InfoMessage += Connection_InfoMessage;
                command.ExecuteNonQuery();
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
        ///<summary>
        ///Метод чтения сообщений БД
        ///</summary>
        ///<param name = "sender">Объект</param>
        ///<param name = "e">Значение ошики</param>
        private void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Message);
            Configuration_class.connection.InfoMessage += Connection_InfoMessage;
        }
    }
}
