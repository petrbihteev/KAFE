using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Word = Microsoft.Office.Interop.Word;

namespace KAFE
{
    /// <summary>
    /// Логика взаимодействия для WaiterWindow.xaml
    /// </summary>
    public partial class WaiterWindow : Window
    {
        public WaiterWindow()
        {
            InitializeComponent();
            combotable.ItemsSource = Helper.GetContext().Tables.ToList();
            combowaypay.ItemsSource = Helper.GetContext().WayPay.ToList();
            combofood.ItemsSource = Helper.GetContext().Food.ToList();
            combotypefood.ItemsSource = Helper.GetContext().TypeFood.ToList();
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            datagridorder.ItemsSource = Helper.GetContext().Orders.Where(x=>x.DateOrder==todaysDate).ToList();
        }

        private void btnaddorder_Click(object sender, RoutedEventArgs e)
        {
            gridaddorder.Visibility = Visibility.Visible;
            gridopen.Visibility = Visibility.Hidden;
        }

        private void backtomainwindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void addorder_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            var idtable = Helper.GetContext().Tables.FirstOrDefault(x => x.NameTable == combotable.Text);
            var idwaypay = Helper.GetContext().WayPay.FirstOrDefault(x => x.NameWayPay == combowaypay.Text);
            Orders orders = new Orders
            {
                ID_Table = idtable.ID_Table,
                ID_User = Helper.ID_user,
                ID_WayPay = idwaypay.ID_Pay,
                ID_Status = 1,
                DateOrder = dateTime
            };
            Helper.GetContext().Orders.Add(orders);
            Helper.GetContext().SaveChanges();
            gridopen.Visibility = Visibility.Visible;
            gridaddorder.Visibility = Visibility.Hidden;
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            datagridorder.ItemsSource = Helper.GetContext().Orders.Where(x=>x.DateOrder==todaysDate).ToList();
            MessageBox.Show("Заказ сформирован");
        }

        private void datagridorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridorder.SelectedItem is Orders orders)
            {
                Helper.ID_order = orders.ID_Order;
                gridaddorder.Visibility = Visibility.Hidden;
                gridopen.Visibility = Visibility.Hidden;
                gridaddfood.Visibility = Visibility.Visible;
                datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.Where(x=>x.ID_Order==Helper.ID_order).ToList();
                if (Helper.GetContext().ListOrder.Where(x=>x.ID_Order==Helper.ID_order).FirstOrDefault()!=null)
                {
                    decimal sum = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).Sum(x => x.Quantity * x.Food.Price);
                    txtsummoney.Text = String.Format("{0:0.00}", sum);
                }
            }
        }

        private void combotypefood_DropDownClosed(object sender, EventArgs e)
        {
            if (combotypefood.SelectedItem != null)
            {
                combofood.ItemsSource = Helper.GetContext().Food.Where(x=>x.TypeFood.NameTypeFood==combotypefood.Text).ToList();
            }
        }

        private void btnaddfood_Click(object sender, RoutedEventArgs e)
        {
            var idstatusorder = Helper.GetContext().Orders.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();
            if (idstatusorder.ID_Status != 1)
            {
                MessageBox.Show("Нельзя редактировать содержимое заказа, когда он уже оплачен!");
            }
            else
            {
                if (combofood.Text == "" || comboquantity.Text == "")
                {
                    MessageBox.Show("Еда не выбрана!");
                }
                else
                {
                    var food = Helper.GetContext().Food.Where(x => x.NameFood == combofood.Text).FirstOrDefault();
                    ListOrder listOrder = new ListOrder
                    {
                        ID_Order = Helper.ID_order,
                        ID_Food = food.ID_Food,
                        Quantity = Convert.ToInt32(comboquantity.Text)
                    };
                    Helper.GetContext().ListOrder.Add(listOrder);
                    Helper.GetContext().SaveChanges();
                    datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.Where(x=>x.ID_Order==Helper.ID_order).ToList();
                    decimal sum = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).Sum(x => x.Quantity * x.Food.Price);
                    txtsummoney.Text = String.Format("{0:0.00}", sum);
                }         
            }    
        }

        private void datagridfoodorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridfoodorder.SelectedItem is ListOrder listOrder)
            {
                var idstatusorder = Helper.GetContext().Orders.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();
                if (idstatusorder.ID_Status != 1)
                {
                    MessageBox.Show("Нельзя редактировать содержимое заказа, когда он уже оплачен!");
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Вы хотите убрать это блюдо из заказа?", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Helper.GetContext().ListOrder.Remove(listOrder);
                            Helper.GetContext().SaveChanges();
                            datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.ToList();
                            decimal sum = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).Sum(x => x.Quantity * x.Food.Price);
                            txtsummoney.Text = String.Format("{0:0.00}", sum);
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
        }

        private void btnbuyfood_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите распечатать чек? После оплаты заказа, его нельзя будет редактировать!", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var listorders = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).ToList();
                    var orders = Helper.GetContext().Orders.ToList();

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "Выберите место для сохранения чека";
                    saveFileDialog.FileName = "Чек" + " - PDF";
                    saveFileDialog.Filter = "PDF Files |*.pdf";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var application = new Word.Application();

                        Word.Document document = application.Documents.Add();

                        Word.Paragraph paragraph1 = document.Paragraphs.Add();
                        Word.Range range1 = paragraph1.Range;
                        range1.Text = "Чек из \"Alberto Del Rio\"";
                        paragraph1.set_Style("Заголовок");
                        range1.Font.Size = 18;
                        range1.InsertParagraphAfter();


                        Word.Paragraph paragraph2 = document.Paragraphs.Add();
                        Word.Range range2 = paragraph2.Range;
                        range2.Text = "Дата и время: " + DateTime.Now;
                        paragraph2.set_Style("Заголовок");
                        range2.Font.Size = 18;
                        range2.InsertParagraphAfter();

                        var sposobpayway = Helper.GetContext().Orders.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();
                        Word.Paragraph paragraph7 = document.Paragraphs.Add();
                        Word.Range range7 = paragraph7.Range;
                        range7.Text = "Способ оплаты: " + sposobpayway.WayPay.NameWayPay;
                        paragraph7.set_Style("Заголовок");
                        range7.Font.Size = 18;
                        range7.InsertParagraphAfter();

                        Word.Paragraph paragraph4 = document.Paragraphs.Add();
                        Word.Range cellrange = paragraph4.Range;
                        Word.InlineShape inlineShape = cellrange.InlineShapes.AddPicture(@"C:\Users\Acer\Desktop\KAFE\KAFE\Images\logo.png");
                        inlineShape.Width = inlineShape.Height = 200;
                        paragraph4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        cellrange.InsertParagraphAfter();

                        Word.Paragraph paragraph3 = document.Paragraphs.Add();
                        Word.Range range3 = paragraph3.Range;
                        range3.Text = "Перечень продуктов";
                        paragraph3.set_Style("Выделенная цитата");
                        range3.Font.Size = 18;
                        range3.Font.Color = Word.WdColor.wdColorBlack;
                        range3.InsertParagraphAfter();


                        foreach (var lisord in listorders)
                        {
                            Word.Paragraph paragraph = document.Paragraphs.Add();
                            Word.Range range = paragraph.Range;
                            range.Text = "| " + lisord.Food.NameFood + " x " + lisord.Quantity + " - " + lisord.Food.Price.ToString("0.00") + " руб. |";
                            paragraph.set_Style("Обычный");
                            range.Font.Size = 14;
                            range.InsertParagraphAfter();

                            //Word.Paragraph tableparagraph = document.Paragraphs.Add();
                            //Word.Range tablerange = tableparagraph.Range;
                            //Word.Table table = document.Tables.Add(tablerange, orders.Count() + 1, 3);
                            //table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            //table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            //Word.Range cellrange;


                        }

                        Word.Paragraph paragraph5 = document.Paragraphs.Add();
                        Word.Range range5 = paragraph5.Range;
                        range5.Text = "Всего: " + txtsummoney.Text + " руб.";
                        paragraph5.set_Style("Выделенная цитата");
                        range5.Font.Size = 24;
                        range5.Font.Color = Word.WdColor.wdColorRed;
                        range5.Bold = 5;
                        range5.InsertParagraphAfter();

                        //application.Visible = true;

                        document.SaveAs2(saveFileDialog.FileName, Word.WdExportFormat.wdExportFormatPDF);
                        //application.Quit();


                        //document.SaveAs2(@"C:\Users\Acer\Desktop\Test.docx");
                        //document.SaveAs2(@"C:\Users\Acer\Desktop\Test.pdf", Word.WdExportFormat.wdExportFormatPDF);
                    }

                    //Обновление статуса заказа
                    var buyorders = Helper.GetContext().Orders.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();
                    if (buyorders.ID_Status==1)
                    {
                        buyorders.ID_Status = 2;
                    }
                    Helper.GetContext().SaveChanges();

                    var idcheck = Helper.GetContext().Check.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();

                    if (idcheck == null)
                    {
                        //Добавление в Чек-таблицу
                        Check check = new Check
                        {
                            ID_Order = Helper.ID_order,
                            DateCheck = DateTime.Now,
                            TotalPrice = Convert.ToDecimal(txtsummoney.Text)
                        };
                        Helper.GetContext().Check.Add(check);
                        Helper.GetContext().SaveChanges();
                    }

                    var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                    datagridorder.ItemsSource = Helper.GetContext().Orders.Where(x=>x.DateOrder==todaysDate).ToList();
                    txtsummoney.Text = "";
                    gridaddfood.Visibility = Visibility.Hidden;
                    gridopen.Visibility = Visibility.Visible;
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void backtopenorder_Click(object sender, RoutedEventArgs e)
        {
            if (gridaddorder.Visibility == Visibility.Visible || gridaddfood.Visibility == Visibility.Visible)
            {
                gridaddfood.Visibility = Visibility.Hidden;
                gridaddorder.Visibility = Visibility.Hidden;
                gridopen.Visibility = Visibility.Visible;
            }
        }

        private void btnreport_Click(object sender, RoutedEventArgs e)
        {
            var user = Helper.GetContext().Users.Where(x => x.ID_User == Helper.ID_user).FirstOrDefault();
            ReportWaiter reportWaiter = new ReportWaiter();
            reportWaiter.Title = "Отчета за смену - " + user.LastName;
            reportWaiter.ShowDialog();
        }
    }
}
