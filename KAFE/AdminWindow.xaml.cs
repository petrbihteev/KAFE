using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Excel = Microsoft.Office.Interop.Excel;


namespace KAFE
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            //gridosnova.Visibility = Visibility.Visible;
            rolecombo.ItemsSource = Helper.GetContext().RoleUsers.ToList();
            listviewuser.ItemsSource = Helper.GetContext().Users.Where(x => x.ID_Status == 1).ToList();
            datagridorder.ItemsSource = Helper.GetContext().Orders.ToList();
            combofood.ItemsSource = Helper.GetContext().Food.ToList();
            combotypefood.ItemsSource = Helper.GetContext().TypeFood.ToList();
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            combosmena.Items.Add(todaysDate.AddDays(1).ToShortDateString());
            combosmena.Items.Add(todaysDate.AddDays(2).ToShortDateString());
            combosmena.Items.Add(todaysDate.AddDays(3).ToShortDateString());
            combosmena.Items.Add(todaysDate.AddDays(4).ToShortDateString());
            combosmena.Items.Add(todaysDate.AddDays(5).ToShortDateString());
        }
        byte[] personalphotobyte = null;
        byte[] contractphotobyte = null;
        int idusersmena = 0;

        private void addpersonalphoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imageLoc = "";
                OpenFileDialog dld = new OpenFileDialog();
                dld.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg";
                dld.Title = "Выберите фотографию пользователя";
                bool? result = dld.ShowDialog();
                if (result == true)
                {
                    imageLoc = dld.FileName.ToString();
                    photopersonal.Source = new BitmapImage(new Uri(imageLoc));
                    FileStream fs = new FileStream(imageLoc, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    personalphotobyte = br.ReadBytes((int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addcontractphoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dld = new OpenFileDialog();
                dld.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg";
                dld.Title = "Выберите фотографию пользователя";
                bool? result = dld.ShowDialog();
                if (result == true)
                {
                    string imageLoc;
                    imageLoc = dld.FileName.ToString();
                    photocontract.Source = new BitmapImage(new Uri(imageLoc));
                    FileStream fs = new FileStream(imageLoc, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    contractphotobyte = br.ReadBytes((int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveadduser_Click(object sender, RoutedEventArgs e)
        {
            if (lastnametxt.Text == "" || nametxt.Text == "" || middlenametxt .Text == "" || logintxt.Text == "" || passwordtxt.Password == "" || rolecombo.Text == "" || personalphotobyte == null || contractphotobyte == null)
            {
                MessageBox.Show("Пустые значения!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var user = Helper.GetContext().Users.FirstOrDefault(x => x.Login == logintxt.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже есть в системе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var role = Helper.GetContext().RoleUsers.FirstOrDefault(x => x.RoleName == rolecombo.Text);
                    Users users = new Users
                    {
                        LastName = lastnametxt.Text,
                        FirstName = nametxt.Text,
                        MiddleName = middlenametxt.Text,
                        Login = logintxt.Text,
                        Password = passwordtxt.Password,
                        ID_Status = 1,
                        ID_Role = role.ID_Role,
                        PersonalPhoto = personalphotobyte,
                        ContractPhoto = contractphotobyte
                    };
                    Helper.GetContext().Users.Add(users);
                    Helper.GetContext().SaveChanges();
                    MessageBox.Show("Пользователь успешно добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    lastnametxt.Text = "";
                    nametxt.Text = "";
                    middlenametxt.Text = "";
                    logintxt.Text = "";
                    passwordtxt.Password = "";
                    rolecombo.Text = "";
                    personalphotobyte = null;
                    contractphotobyte = null;
                    photocontract.Source = null;
                    photopersonal.Source = null;
                }
            }
        }

        private void dropphotopersonal_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string imageLoc = System.IO.Path.GetFullPath(files[0]).ToString();
                photopersonal.Source = new BitmapImage(new Uri(imageLoc));
                FileStream fs = new FileStream(imageLoc, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                personalphotobyte = br.ReadBytes((int)fs.Length);
            }
        }

        private void dropphotocontract_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string imageLoc = System.IO.Path.GetFullPath(files[0]).ToString();
            photocontract.Source = new BitmapImage(new Uri(imageLoc));
            FileStream fs = new FileStream(imageLoc, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            contractphotobyte = br.ReadBytes((int)fs.Length);
        }

        private void griduser_Click(object sender, RoutedEventArgs e)
        {
            gridosnova.Visibility = Visibility.Hidden;
            griduser.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            griduser.Visibility = Visibility.Hidden;
            gridorder.Visibility = Visibility.Hidden;
            gridosnova.Visibility = Visibility.Visible;
            gridsmena.Visibility = Visibility.Hidden;
            gridreport.Visibility = Visibility.Hidden;
            btngotoexcel.Visibility = Visibility.Hidden;
            btngotoexcelactive.Visibility = Visibility.Hidden;
            btngotopdf.Visibility = Visibility.Hidden;
            btngotopdfactive.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            griduser.Visibility = Visibility.Hidden;
            gridadduser.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            backtouser();
        }

        public void backtouser()
        {
            gridadduser.Visibility = Visibility.Hidden;
            griduser.Visibility = Visibility.Visible;
            var listusers = Helper.GetContext().Users.Where(x => x.ID_Status == 1).ToList();
            listviewuser.ItemsSource = listusers;
            labeluser.Content = "Добавление сотрудника";
            lastnametxt.Text = "";
            nametxt.Text = "";
            middlenametxt.Text = "";
            logintxt.Text = "";
            rolecombo.Text = "";
            passwordtxt.Password = "";
            photopersonal.Source = null;
            photocontract.Source = null;
            rolecombo.IsEnabled = true;
            saveadduser.Visibility = Visibility.Visible;
            addcontractphoto.Visibility = Visibility.Visible;
            addpersonalphoto.Visibility = Visibility.Visible;
            btndeleteuser.Visibility = Visibility.Hidden;
        }

        private void btngridorder_Click(object sender, RoutedEventArgs e)
        {
            gridosnova.Visibility = Visibility.Hidden;
            gridorder.Visibility = Visibility.Visible;
        }

        private void btngridshifts_Click(object sender, RoutedEventArgs e)
        {
            datagridstaff.ItemsSource = Helper.GetContext().Users.Where(x => x.ID_Role != 1 && x.ID_Status == 1).ToList();
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            datagridsmenastaff.ItemsSource = Helper.GetContext().WorkShift.Where(x => x.DateShift ==todaysDate).ToList();
            gridosnova.Visibility = Visibility.Hidden;
            gridsmena.Visibility = Visibility.Visible;
        }

        private void btngridreport_Click(object sender, RoutedEventArgs e)
        {
            gridosnova.Visibility = Visibility.Hidden;
            gridreport.Visibility = Visibility.Visible;
        }

        private void btnworkuser_Click(object sender, RoutedEventArgs e)
        {
            listviewuser.ItemsSource = Helper.GetContext().Users.Where(x => x.ID_Status == 1).ToList();
        }

        private void btndeleteuser_Click(object sender, RoutedEventArgs e)
        {
            listviewuser.ItemsSource = Helper.GetContext().Users.Where(x => x.ID_Status == 2).ToList();
        }

        private void listviewuser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Users users = (Users)listviewuser.SelectedItem;
            if (users != null)
            {
                griduser.Visibility = Visibility.Hidden;
                gridadduser.Visibility = Visibility.Visible;
                labeluser.Content = "Информация о сотруднике";
                iduser.Text = Convert.ToString(users.ID_User);
                lastnametxt.Text = users.LastName;
                nametxt.Text = users.FirstName;
                middlenametxt.Text = users.MiddleName;
                logintxt.Text = users.Login;
                rolecombo.Text = users.RoleUsers.RoleName;
                passwordtxt.Password = users.Password;
                byte[] image = users.PersonalPhoto;
                MemoryStream ms = new MemoryStream(image);
                photopersonal.Source = BitmapFrame.Create(ms);
                byte[] image1 = users.ContractPhoto;
                MemoryStream ms1 = new MemoryStream(image1);
                photocontract.Source = BitmapFrame.Create(ms1);
                rolecombo.IsEnabled = false;
                saveadduser.Visibility = Visibility.Hidden;
                addcontractphoto.Visibility = Visibility.Hidden;
                addpersonalphoto.Visibility = Visibility.Hidden;
                btndeleteuser.Visibility = Visibility.Visible;
                if (users.ID_Status == 2)
                {
                    btndeleteuser.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btndeleteuser_Click_1(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(iduser.Text);
            if (id != 0)
            {
                var idduser = Helper.GetContext().Users.Where(x => x.ID_User == id).FirstOrDefault();
                idduser.ID_Status = 2;
                Helper.GetContext().SaveChanges();
                MessageBox.Show("Пользователь успешно удален!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                backtouser();
            }
        }

        private void backtomainwindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void datagridorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridorder.SelectedItem is Orders orders)
            {
                Helper.ID_order = orders.ID_Order;
                datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).ToList();
                if (Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault() != null)
                {
                    decimal sum = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).Sum(x => x.Quantity * x.Food.Price);
                    txtsummoney.Text = String.Format("{0:0.00}", sum);
                }
                gridorder.Visibility = Visibility.Hidden;
                gridaddfood.Visibility = Visibility.Visible;
            }
        }

        private void backtopenorder_Click(object sender, RoutedEventArgs e)
        {
            gridaddfood.Visibility = Visibility.Hidden;
            gridorder.Visibility = Visibility.Visible;
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
                    datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).ToList();
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

        private void combotypefood_DropDownClosed(object sender, EventArgs e)
        {
            if (combotypefood.SelectedItem != null)
            {
                combofood.ItemsSource = Helper.GetContext().Food.Where(x => x.TypeFood.NameTypeFood == combotypefood.Text).ToList();
            }
        }

        private void combosmena_DropDownClosed(object sender, EventArgs e)
        {
            if (combosmena.SelectedIndex != -1)
            {
                var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                switch (combosmena.SelectedIndex)
                {
                    case 0:
                        todaysDate = todaysDate.AddDays(1);
                        break;
                    case 1:
                        todaysDate = todaysDate.AddDays(2);
                        break;
                    case 2:
                        todaysDate = todaysDate.AddDays(3);
                        break;
                    case 3:
                        todaysDate = todaysDate.AddDays(4);
                        break;
                    case 4:
                        todaysDate = todaysDate.AddDays(5);
                        break;
                }
                datagridsmenastaff.ItemsSource = Helper.GetContext().WorkShift.Where(x => x.DateShift == todaysDate).ToList();
            }
        }

        private void datagridstaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridstaff.SelectedItem is Users users)
            {
                txtuserlastname.Text = users.LastName;
                idusersmena = users.ID_User;
            }
        }

        private void btnaddsmena_Click(object sender, RoutedEventArgs e)
        {

            if (txtuserlastname.Text == "" || combosmena.Text=="")
            {
                MessageBox.Show("Не указана дата или не выбран работник!");
            }
            else
            {
                var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                switch (combosmena.SelectedIndex)
                {
                    case 0:
                       todaysDate = todaysDate.AddDays(1);
                        break;
                    case 1:
                        todaysDate = todaysDate.AddDays(2);
                        break;
                    case 2:
                        todaysDate = todaysDate.AddDays(3);
                        break;
                    case 3:
                        todaysDate = todaysDate.AddDays(4);
                        break;
                    case 4:
                        todaysDate = todaysDate.AddDays(5);
                        break;
                }
                if (combosmena.SelectedIndex != -1)
                {
                    if (Helper.GetContext().WorkShift.Where(x => x.ID_User == idusersmena && x.DateShift == todaysDate).FirstOrDefault()!=null)
                    {
                        MessageBox.Show("Этот работник уже назначен на эту смену");
                    }
                    else
                    {
                        if (Helper.GetContext().WorkShift.Where(x => x.DateShift == todaysDate).Count() >= 6)
                        {
                            MessageBox.Show("Максимальное количество работников на смене!");
                        }
                        else
                        {
                            WorkShift workShift = new WorkShift
                            {
                                ID_User = idusersmena,
                                DateShift = todaysDate
                            };
                            Helper.GetContext().WorkShift.Add(workShift);
                            Helper.GetContext().SaveChanges();
                            idusersmena = 0;
                            datagridsmenastaff.ItemsSource = Helper.GetContext().WorkShift.Where(x => x.DateShift == todaysDate).ToList();
                            txtuserlastname.Text = "";
                            combosmena.Text = "";
                            MessageBox.Show("Смена назначена!");
                        }
                    }
                }   
            }
        }

        private void datagridsmenastaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridsmenastaff.SelectedItem is WorkShift workShift)
            {
                MessageBoxResult result = MessageBox.Show("Вы хотите убрать человека со смены?", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                        Helper.GetContext().WorkShift.Remove(workShift);
                        Helper.GetContext().SaveChanges();
                        datagridsmenastaff.ItemsSource = Helper.GetContext().WorkShift.Where(x=>x.DateShift==todaysDate).ToList();
                        combosmena.Text = "";
                        txtuserlastname.Text = "";
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void btnreportdata_Click(object sender, RoutedEventArgs e)
        {

            if (datapicker.SelectedDate == null)
            {
                MessageBox.Show("Вы не выбрали дату");
            }
            else
            {
                btngotoexcel.Visibility = Visibility.Visible;
                btngotopdf.Visibility = Visibility.Visible;
            }
        }

        private void btnreportactive_Click(object sender, RoutedEventArgs e)
        {
            btngotoexcelactive.Visibility = Visibility.Visible;
            btngotopdfactive.Visibility = Visibility.Visible;
        }

        private void btngotopdf_Click(object sender, RoutedEventArgs e)
        {
            var report = Helper.GetContext().Orders.Where(x => x.DateOrder == datapicker.SelectedDate).ToList();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения отчета";
            saveFileDialog.FileName = "Отчет за " + datapicker.Text + " - PDF";
            saveFileDialog.Filter = "PDF Files |*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                var application = new Word.Application();

                Word.Document document = application.Documents.Add();

                Word.Paragraph paragraph1 = document.Paragraphs.Add();
                Word.Range range1 = paragraph1.Range;
                range1.Text = "Отчет по заказам за " + datapicker.Text;
                paragraph1.set_Style("Заголовок");
                paragraph1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                range1.Font.Size = 18;
                range1.InsertParagraphAfter();

                Word.Paragraph tableparagraph = document.Paragraphs.Add();
                Word.Range tablerange = tableparagraph.Range;
                Word.Table paymentstable = document.Tables.Add(tablerange, report.Count() + 1, 6);
                paymentstable.Borders.InsideLineStyle = paymentstable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentstable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = paymentstable.Cell(1, 1).Range;
                cellRange.Text = "Дата заказа";
                cellRange = paymentstable.Cell(1, 2).Range;
                cellRange.Text = "Номер стола";
                cellRange = paymentstable.Cell(1, 3).Range;
                cellRange.Text = "Официант";
                cellRange = paymentstable.Cell(1, 4).Range;
                cellRange.Text = "Статус заказа";
                cellRange = paymentstable.Cell(1, 5).Range;
                cellRange.Text = "Способ оплаты";
                cellRange = paymentstable.Cell(1, 6).Range;
                cellRange.Text = "Стоимость";

                paymentstable.Rows[1].Range.Bold = 1;
                paymentstable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < report.Count(); i++)
                {
                    var orders = report[i];

                    cellRange = paymentstable.Cell(i + 2, 1).Range;
                    cellRange.Text = orders.DateOrder.ToString("dd.MM.yyyy");

                    cellRange = paymentstable.Cell(i + 2, 2).Range;
                    cellRange.Text = orders.Tables.NameTable;

                    cellRange = paymentstable.Cell(i + 2, 3).Range;
                    cellRange.Text = orders.Users.LastName + " " + orders.Users.FirstName.Substring(0, 1) + ". " + orders.Users.MiddleName.Substring(0, 1) + ".";

                    cellRange = paymentstable.Cell(i + 2, 4).Range;
                    cellRange.Text = orders.StatusOrder.NameStatusOrder;

                    cellRange = paymentstable.Cell(i + 2, 5).Range;
                    cellRange.Text = orders.WayPay.NameWayPay;

                    decimal sum;
                    try
                    {
                        sum = orders.ListOrder.Where(x => x.ID_Order == orders.ID_Order).Sum(x => x.Quantity * x.Food.Price);
                    }
                    catch
                    {
                        sum = 0;
                    }
                    cellRange = paymentstable.Cell(i + 2, 6).Range;
                    cellRange.Text = String.Format("{0:0.00}", sum) + " руб.";

                    //cellRange.InsertParagraphAfter();
                }
                document.SaveAs2(saveFileDialog.FileName, Word.WdExportFormat.wdExportFormatPDF);
                btngotoexcel.Visibility = Visibility.Hidden;
                btngotopdf.Visibility = Visibility.Hidden;
                MessageBox.Show("Отчет сохранен!");
            }
        }

        private void btngotoexcel_Click(object sender, RoutedEventArgs e)
        {
            var report = Helper.GetContext().Orders.Where(x => x.DateOrder == datapicker.SelectedDate).ToList();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения отчета";
            saveFileDialog.FileName = "Отчет за " + datapicker.Text + " - Excel";
            saveFileDialog.Filter = "Excel Files |*.xlsx";
            if (saveFileDialog.ShowDialog()==true)
            {
                var application = new Excel.Application();

                application.SheetsInNewWorkbook = 1;

                Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);

                int startRowIndex = 1;

                    Excel.Worksheet worksheet = application.Worksheets.Item[1];

                    worksheet.Name = "Отчет за " + datapicker.Text;

                    worksheet.Cells[1][startRowIndex] = "Дата заказа";
                    worksheet.Cells[2][startRowIndex] = "Номер стола";
                    worksheet.Cells[3][startRowIndex] = "Официант";
                    worksheet.Cells[4][startRowIndex] = "Статус заказа";
                    worksheet.Cells[5][startRowIndex] = "Способ оплаты";
                    worksheet.Cells[6][startRowIndex] = "Стоимость";

                    startRowIndex++;

                    foreach (var orders in report)
                    {

                        worksheet.Cells[1][startRowIndex] = orders.DateOrder.ToString("dd.MM.yyyy");
                        worksheet.Cells[2][startRowIndex] = orders.Tables.NameTable;
                        worksheet.Cells[3][startRowIndex] = orders.Users.LastName;
                        worksheet.Cells[4][startRowIndex] = orders.StatusOrder.NameStatusOrder;
                        worksheet.Cells[5][startRowIndex] = orders.WayPay.NameWayPay;

                        decimal sum = orders.ListOrder.Where(x => x.ID_Order == orders.ID_Order).Sum(x => x.Quantity * x.Food.Price);

                        worksheet.Cells[6][startRowIndex] = sum;

                        startRowIndex++;
                    }

                    Excel.Range sumRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[5][startRowIndex]];
                    sumRange.Merge();
                    sumRange.Value = "Итого:";
                    sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                    worksheet.Cells[6][startRowIndex].Formula = $"=SUM(F{startRowIndex - report.Count()}:" + $"F{startRowIndex - 1})";

                    sumRange.Font.Bold = worksheet.Cells[6][startRowIndex].Font.Bold = true;
                    //worksheet.Cells[6][startRowIndex].NumberFormat = "#,###.00";

                    startRowIndex++;

                    Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[6][startRowIndex - 1]];
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;


                    worksheet.Columns.AutoFit();
                
                workbook.SaveAs(saveFileDialog.FileName);
                btngotoexcel.Visibility = Visibility.Hidden;
                btngotopdf.Visibility = Visibility.Hidden;
                MessageBox.Show("Отчет сохранен!");
            }
        }

        private void btngotopdfactive_Click(object sender, RoutedEventArgs e)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var report = Helper.GetContext().Orders.Where(x => x.DateOrder == todaysDate).ToList();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения отчета";
            saveFileDialog.FileName = "Отчет за " + todaysDate.ToString("dd.MM.yyyy") + " - PDF";
            saveFileDialog.Filter = "PDF Files |*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                var application = new Word.Application();

                Word.Document document = application.Documents.Add();

                Word.Paragraph paragraph1 = document.Paragraphs.Add();
                Word.Range range1 = paragraph1.Range;
                range1.Text = "Отчет по заказам за " + todaysDate.ToString("dd.MM.yyyy");
                paragraph1.set_Style("Заголовок");
                paragraph1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                range1.Font.Size = 18;
                range1.InsertParagraphAfter();

                Word.Paragraph tableparagraph = document.Paragraphs.Add();
                Word.Range tablerange = tableparagraph.Range;
                Word.Table paymentstable = document.Tables.Add(tablerange, report.Count() + 1, 6);
                paymentstable.Borders.InsideLineStyle = paymentstable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentstable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = paymentstable.Cell(1, 1).Range;
                cellRange.Text = "Дата заказа";
                cellRange = paymentstable.Cell(1, 2).Range;
                cellRange.Text = "Номер стола";
                cellRange = paymentstable.Cell(1, 3).Range;
                cellRange.Text = "Официант";
                cellRange = paymentstable.Cell(1, 4).Range;
                cellRange.Text = "Статус заказа";
                cellRange = paymentstable.Cell(1, 5).Range;
                cellRange.Text = "Способ оплаты";
                cellRange = paymentstable.Cell(1, 6).Range;
                cellRange.Text = "Стоимость";

                paymentstable.Rows[1].Range.Bold = 1;
                paymentstable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < report.Count(); i++)
                {
                    var orders = report[i];

                    cellRange = paymentstable.Cell(i + 2, 1).Range;
                    cellRange.Text = orders.DateOrder.ToString("dd.MM.yyyy");

                    cellRange = paymentstable.Cell(i + 2, 2).Range;
                    cellRange.Text = orders.Tables.NameTable;

                    cellRange = paymentstable.Cell(i + 2, 3).Range;
                    cellRange.Text = orders.Users.LastName + " " + orders.Users.FirstName.Substring(0, 1) + ". " + orders.Users.MiddleName.Substring(0, 1) + ".";

                    cellRange = paymentstable.Cell(i + 2, 4).Range;
                    cellRange.Text = orders.StatusOrder.NameStatusOrder;

                    cellRange = paymentstable.Cell(i + 2, 5).Range;
                    cellRange.Text = orders.WayPay.NameWayPay;

                    decimal sum;
                    try
                    {
                        sum = orders.ListOrder.Where(x => x.ID_Order == orders.ID_Order).Sum(x => x.Quantity * x.Food.Price);
                    }
                    catch
                    {
                        sum = 0;
                    }
                    cellRange = paymentstable.Cell(i + 2, 6).Range;
                    cellRange.Text = String.Format("{0:0.00}", sum) + " руб.";

                    //cellRange.InsertParagraphAfter();
                }
                document.SaveAs2(saveFileDialog.FileName, Word.WdExportFormat.wdExportFormatPDF);
                btngotoexcelactive.Visibility = Visibility.Hidden;
                btngotopdfactive.Visibility = Visibility.Hidden;
                MessageBox.Show("Отчет сохранен!");
            }
        }

        private void btngotoexcelactive_Click(object sender, RoutedEventArgs e)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var report = Helper.GetContext().Orders.Where(x => x.DateOrder == todaysDate).ToList();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения отчета";
            saveFileDialog.FileName = "Отчет за " + todaysDate.ToString("dd.MM.yyyy") + " - Excel";
            saveFileDialog.Filter = "Excel Files |*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                var application = new Excel.Application();

                application.SheetsInNewWorkbook = 1;

                Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);

                int startRowIndex = 1;

                Excel.Worksheet worksheet = application.Worksheets.Item[1];

                worksheet.Name = "Отчет за " + todaysDate.ToString("dd.MM.yyyy");

                worksheet.Cells[1][startRowIndex] = "Дата заказа";
                worksheet.Cells[2][startRowIndex] = "Номер стола";
                worksheet.Cells[3][startRowIndex] = "Официант";
                worksheet.Cells[4][startRowIndex] = "Статус заказа";
                worksheet.Cells[5][startRowIndex] = "Способ оплаты";
                worksheet.Cells[6][startRowIndex] = "Стоимость";

                startRowIndex++;

                foreach (var orders in report)
                {

                    worksheet.Cells[1][startRowIndex] = orders.DateOrder.ToString("dd.MM.yyyy");
                    worksheet.Cells[2][startRowIndex] = orders.Tables.NameTable;
                    worksheet.Cells[3][startRowIndex] = orders.Users.LastName;
                    worksheet.Cells[4][startRowIndex] = orders.StatusOrder.NameStatusOrder;
                    worksheet.Cells[5][startRowIndex] = orders.WayPay.NameWayPay;

                    decimal sum = orders.ListOrder.Where(x => x.ID_Order == orders.ID_Order).Sum(x => x.Quantity * x.Food.Price);

                    worksheet.Cells[6][startRowIndex] = sum;

                    startRowIndex++;
                }

                Excel.Range sumRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[5][startRowIndex]];
                sumRange.Merge();
                sumRange.Value = "Итого:";
                sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                worksheet.Cells[6][startRowIndex].Formula = $"=SUM(F{startRowIndex - report.Count()}:" + $"F{startRowIndex - 1})";

                sumRange.Font.Bold = worksheet.Cells[6][startRowIndex].Font.Bold = true;
                //worksheet.Cells[6][startRowIndex].NumberFormat = "#,###.00";

                startRowIndex++;

                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[6][startRowIndex - 1]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;


                worksheet.Columns.AutoFit();

                workbook.SaveAs(saveFileDialog.FileName);
                btngotoexcelactive.Visibility = Visibility.Hidden;
                btngotopdfactive.Visibility = Visibility.Hidden;
                MessageBox.Show("Отчет сохранен!");
            }
        }
    }
}
