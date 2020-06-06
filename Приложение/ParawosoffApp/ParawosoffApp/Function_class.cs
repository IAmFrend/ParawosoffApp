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
    class Function_class
    {
        public DataTable Regtable = new DataTable();
        internal enum Function_Result
        {
            table, scalar
        }
        public Function_class()
        {

        }
        ///<summary>
        ///Перегрузка для процедур без формльных параметров.
        ///</summary>
        ///<param name = "Function_Name">Название процедуры</param>
        ///<param name = "Type_function">Тип функции</param>
        public Function_class(string Function_Name, Function_Result result)
        {
            string querry = "";
            switch (result)
            {
                case Function_Result.scalar:
                    //Вывод данныхиз скалаярной функции
                    querry = string.Format(@"select [dbo].[{0}]()", Function_Name);
                    break;
                case Function_Result.table:
                    querry = string.Format(@"select * from [dbo].[{0}]()", Function_Name);
                    break;
            }
            try
            {
                //Вывод результирующей таблицы и запись её в переменную
                Table_Class table = new Table_Class(querry);
                Regtable = table.table;
            }
            catch
            {

            }
        }
        ///<summary>
        ///Перегрузка для процедур с параметрами.
        ///</summary>
        ///<param name = "Function_Name">Название процедуры</param>
        ///<param name = "Type_function">Тип функции</param>
        ///<param name = "Parameters">Параметры</param>
        public Function_class(string Function_Name, Function_Result Type_function, ArrayList Parameters)
        {
            string querry = "";
            try
            {
                switch (Type_function)
                {
                    case Function_Result.scalar:
                        //Вывод данныхиз скалаярной функции
                        querry = string.Format(@"select [dbo].[{0}](", Function_Name);
                        break;
                    case Function_Result.table:
                        querry = string.Format(@"select * from [dbo].[{0}](", Function_Name);
                        break;
                }
                //Переменная списка параметров
                string list_parameters = "";

                //switch (Parameters.Count)
                //{
                //    case 1:
                //        //Вывод данныхиз скалаярной функции
                //        list_parameters += Parameters[0].ToString() + ")";
                //        break;
                //    default:
                //        foreach (object parameter in Parameters)
                //        {
                //            list_parameters += parameter + ",";
                //        }
                //        list_parameters = list_parameters.Remove(list_parameters.Length-1, 1);
                //        list_parameters += ")";
                //        break;
                //}

                //Заполнение списка из листа параметров
                foreach (object parameter in Parameters)
                {
                    list_parameters += "\'"+parameter.ToString() + "\',";
                }
                list_parameters = list_parameters.Remove(list_parameters.Length - 1, 1);
                list_parameters += ")";
                //Оформление запроса
                querry += list_parameters;
                //Заполнение таблицы
                Table_Class table = new Table_Class(querry);
                Regtable = table.table;
            }
            catch
            {

            }
        }
    }
}
