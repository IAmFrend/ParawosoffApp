using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace ParawosoffApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //переменная учётной записи
        public static string intID;
        //переменная адреса отправки
        public static string strMail = "michaellvovthefrend@gmail.com";
        //Наличие подключения к БД
        public static bool connect = false;
        //Класс виртуального интерфейса приложения
        public static Mutex instanse;
        //Название приложения
        public const string app_Name = "ArizonaDatabase";
    }
}
