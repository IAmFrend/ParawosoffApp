using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ParawosoffApp;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;
//Библиотеки под псевдонимом пространства имён
using word = Microsoft.Office.Interop.Word;
using excel = Microsoft.Office.Interop.Excel;

namespace ParawosoffApp
{
    class Document_class
    {
        ///<summary>
        ///Список доступных видов отчётов.
        ///</summary>
        internal enum Document_Type
        {
            Check, MarkSchemes, Order, Storage
        }
        ///<summary>
        ///Процедура формирования документа
        ///</summary>
        ///<param name="type">Тип документа</param>
        ///<param name="name">Название документа</param>
        ///<param name="table">результирующая таблица</param>
        public void Document_Create(Document_Type type, string name, DataTable table)
        {
            //Получение данных о документе
            Configuration_class configuration_Class = new Configuration_class();
            configuration_Class.Document_Configuration_Get();
            switch (name != "" || name != null)
            {
                case (true):
                    switch (type)
                    {
                        case (Document_Type.Check):
                            //Открытие приложения и документа в нём
                            word.Application application = new word.Application();
                            word.Document document = application.Documents.Add(Visible: true);
                            //Формирование документа
                            try
                            {
                                //Начало
                                word.Range range = document.Range(0, 0);
                                //Поля
                                document.Sections.PageSetup.LeftMargin = application.CentimetersToPoints((float)Configuration_class.doc_Left_Merge);
                                document.Sections.PageSetup.RightMargin = application.CentimetersToPoints((float)Configuration_class.doc_Right_Merge);
                                document.Sections.PageSetup.TopMargin = application.CentimetersToPoints((float)Configuration_class.doc_Top_Merge);
                                document.Sections.PageSetup.BottomMargin = application.CentimetersToPoints((float)Configuration_class.doc_Bottom_Merge);
                                //Присвоение текстового значения в абзац
                                range.Text = Configuration_class.Organization_Name;
                                //Выравнивание
                                range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                //Интервалы в абзаце
                                range.ParagraphFormat.SpaceAfter = 1;
                                range.ParagraphFormat.SpaceBefore = 1;
                                range.ParagraphFormat.LineSpacingRule = word.WdLineSpacing.wdLineSpaceSingle;
                                //Шрифт
                                range.Font.Name = "Times New Roman";
                                range.Font.Size = 12;
                                //Параграф (один Enter)
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                //Название документа
                                word.Paragraph Document_Name = document.Paragraphs.Add();
                                //Настройка параграфа
                                Document_Name.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                Document_Name.Range.Font.Name = "Times New Roman";
                                Document_Name.Range.Font.Size = 16;
                                Document_Name.Range.Text = "Выгрузка чека";
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                //Создание таблицы
                                word.Paragraph statparg = document.Paragraphs.Add();
                                word.Table stat_table = document.Tables.Add(statparg.Range, table.Rows.Count, table.Columns.Count);
                                //Настройка таблицы
                                stat_table.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                                stat_table.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                                stat_table.Rows.Alignment = word.WdRowAlignment.wdAlignRowCenter;
                                stat_table.Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                stat_table.Range.Font.Size = 11;
                                stat_table.Range.Font.Name = "Times New Roman";
                                //Заполнение таблицы
                                for (int row = 1; row <= table.Rows.Count; row++)
                                    for (int col = 1; col <= table.Columns.Count; col++)
                                    {
                                        stat_table.Cell(row, col).Range.Text = table.Rows[row - 1][col - 1].ToString();
                                    }
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                document.Paragraphs.Add();
                                //Строка даты создания
                                word.Paragraph Footparg = document.Paragraphs.Add();
                                Footparg.Range.Text = string.Format("Дата создания \t\t\t{0}", DateTime.Now.ToString());
                            }
                            catch
                            {

                            }
                            finally
                            {
                                try
                                {
                                    //Сохранение документа
                                    document.SaveAs2(name + "DOC", word.WdSaveFormat.wdFormatDocument);
                                    //Закрытие документа
                                    document.Close(SaveChanges: false);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                application.Quit();
                            }
                            break;
                        case (Document_Type.MarkSchemes):
                            //Открытие приложения, создание документа (книги) и листа в нём.
                            excel.Application application_ex = new excel.Application();
                            excel.Workbook workbook = application_ex.Workbooks.Add();
                            //excel.Worksheet worksheet = (excel.Worksheet)workbook.ActiveSheet;
                            excel.Worksheet worksheet = workbook.ActiveSheet;
                            try
                            {
                                worksheet.Name = "MarketerDoc";
                                worksheet.Cells[1][1] = "Отчёт о работе отдела маркетинга";
                                worksheet.Range[worksheet.Cells[1,1], worksheet.Cells[1, table.Columns.Count + 1]].Merge();
                                //Заполнение таблицы
                                for (int row = 0; row < table.Rows.Count; row++)
                                    for (int col = 0; col < table.Columns.Count; col++)
                                    {
                                        worksheet.Cells[row + 3, col + 1] = table.Rows[row][col].ToString();
                                    }
                                //Работа со стилем таблицы
                                excel.Range border1 = worksheet.Range[worksheet.Cells[3, 1], worksheet.Cells[table.Rows.Count + 3][table.Columns.Count]];
                                border1.Borders.LineStyle = excel.XlLineStyle.xlContinuous;
                                border1.VerticalAlignment = excel.XlVAlign.xlVAlignCenter;
                                border1.HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;
                                //Дата документа 
                                worksheet.Cells[2][table.Rows.Count + 3] = string.Format("Дата создания {0}", DateTime.Now.ToString());
                                //Объединение ячеек
                                worksheet.Range[worksheet.Cells[table.Rows.Count + 3, 2], worksheet.Cells[table.Rows.Count + 3, table.Columns.Count + 2]].Merge();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                //Сохранение и выход
                                workbook.SaveAs(name, application_ex.DefaultSaveFormat);
                                workbook.Close();
                                application_ex.Quit();
                            }
                            break;
                        case (Document_Type.Order):
                            //Открытие приложения и документа в нём
                            word.Application application_or = new word.Application();
                            word.Document document_or = application_or.Documents.Add(Visible: true);
                            //Формирование документа
                            try
                            {
                                //Начало
                                word.Range range = document_or.Range(0, 0);
                                //Поля
                                document_or.Sections.PageSetup.LeftMargin = application_or.CentimetersToPoints((float)Configuration_class.doc_Left_Merge);
                                document_or.Sections.PageSetup.RightMargin = application_or.CentimetersToPoints((float)Configuration_class.doc_Right_Merge);
                                document_or.Sections.PageSetup.TopMargin = application_or.CentimetersToPoints((float)Configuration_class.doc_Top_Merge);
                                document_or.Sections.PageSetup.BottomMargin = application_or.CentimetersToPoints((float)Configuration_class.doc_Bottom_Merge);
                                //Присвоение текстового значения в абзац
                                range.Text = Configuration_class.Organization_Name;
                                //Выравнивание
                                range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                //Интервалы в абзаце
                                range.ParagraphFormat.SpaceAfter = 1;
                                range.ParagraphFormat.SpaceBefore = 1;
                                range.ParagraphFormat.LineSpacingRule = word.WdLineSpacing.wdLineSpaceSingle;
                                //Шрифт
                                range.Font.Name = "Times New Roman";
                                range.Font.Size = 12;
                                //Параграф (один Enter)
                                document_or.Paragraphs.Add();
                                document_or.Paragraphs.Add();
                                document_or.Paragraphs.Add();
                                //Название документа
                                word.Paragraph Document_Name = document_or.Paragraphs.Add();
                                //Настройка параграфа
                                Document_Name.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                Document_Name.Range.Font.Name = "Times New Roman";
                                Document_Name.Range.Font.Size = 16;
                                Document_Name.Range.Text = table.Rows[0][0].ToString();
                                document_or.Paragraphs.Add();
                                document_or.Paragraphs.Add();
                                document_or.Paragraphs.Add();
                                //Создание таблицы
                                word.Paragraph statparg = document_or.Paragraphs.Add();
                                word.Paragraph Document_Text = document_or.Paragraphs.Add();
                                //Настройка параграфа
                                Document_Text.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                                Document_Text.Format.LeftIndent = (float)1.25;
                                Document_Text.Range.Font.Name = "Times New Roman";
                                Document_Text.Range.Font.Size = 14;
                                Document_Text.Range.Text = table.Rows[0][1].ToString();
                            }
                            catch
                            {

                            }
                            finally
                            {
                                try
                                {
                                    //Сохранение документа
                                    document_or.SaveAs2(name+"PDF", word.WdSaveFormat.wdFormatPDF);
                                    //Закрытие документа
                                    document_or.Close(SaveChanges:false);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                application_or.Quit();
                            }
                            break;
                        case (Document_Type.Storage):
                            //Открытие приложения и документа в нём
                            word.Application application_St = new word.Application();
                            word.Document document_St = application_St.Documents.Add(Visible: true);
                            //Формирование документа
                            try
                            {
                                //Начало
                                word.Range range = document_St.Range(0, 0);
                                //Поля
                                document_St.Sections.PageSetup.LeftMargin = application_St.CentimetersToPoints((float)Configuration_class.doc_Left_Merge);
                                document_St.Sections.PageSetup.RightMargin = application_St.CentimetersToPoints((float)Configuration_class.doc_Right_Merge);
                                document_St.Sections.PageSetup.TopMargin = application_St.CentimetersToPoints((float)Configuration_class.doc_Top_Merge);
                                document_St.Sections.PageSetup.BottomMargin = application_St.CentimetersToPoints((float)Configuration_class.doc_Bottom_Merge);
                                //Присвоение текстового значения в абзац
                                range.Text = Configuration_class.Organization_Name;
                                //Выравнивание
                                range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                //Интервалы в абзаце
                                range.ParagraphFormat.SpaceAfter = 1;
                                range.ParagraphFormat.SpaceBefore = 1;
                                range.ParagraphFormat.LineSpacingRule = word.WdLineSpacing.wdLineSpaceSingle;
                                //Шрифт
                                range.Font.Name = "Times New Roman";
                                range.Font.Size = 12;
                                //Параграф (один Enter)
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                //Название документа
                                word.Paragraph Document_Name = document_St.Paragraphs.Add();
                                //Настройка параграфа
                                Document_Name.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                                Document_Name.Range.Font.Name = "Times New Roman";
                                Document_Name.Range.Font.Size = 16;
                                Document_Name.Range.Text = "Выгрузка чека";
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                //Создание таблицы
                                word.Paragraph statparg = document_St.Paragraphs.Add();
                                word.Table stat_table = document_St.Tables.Add(statparg.Range, table.Rows.Count, table.Columns.Count);
                                //Настройка таблицы
                                stat_table.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                                stat_table.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                                stat_table.Rows.Alignment = word.WdRowAlignment.wdAlignRowCenter;
                                stat_table.Range.Cells.VerticalAlignment = word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                stat_table.Range.Font.Size = 11;
                                stat_table.Range.Font.Name = "Times New Roman";
                                //Заполнение таблицы
                                for (int row = 1; row <= table.Rows.Count; row++)
                                    for (int col = 1; col <= table.Columns.Count; col++)
                                    {
                                        stat_table.Cell(row, col).Range.Text = table.Rows[row - 1][col - 1].ToString();
                                    }
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                document_St.Paragraphs.Add();
                                //Строка даты создания
                                word.Paragraph Footparg = document_St.Paragraphs.Add();
                                Footparg.Range.Text = string.Format("Дата создания \t\t\t{0}", DateTime.Now.ToString());
                            }
                            catch
                            {

                            }
                            finally
                            {
                                try
                                {
                                    //Сохранение документа
                                    document_St.SaveAs2(name + "DOC", word.WdSaveFormat.wdFormatDocument);
                                    //Закрытие документа
                                    document_St.Close(SaveChanges: false);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                application_St.Quit();
                            }
                            break;
                    }
                    break;
                case (false):
                    MessageBox.Show("Введите название документа");
                    break;
            }
        }
    }
}