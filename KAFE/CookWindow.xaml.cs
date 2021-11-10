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

namespace KAFE
{
    /// <summary>
    /// Логика взаимодействия для CookWindow.xaml
    /// </summary>
    public partial class CookWindow : Window
    {
        public CookWindow()
        {
            InitializeComponent();
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            datagridorder.ItemsSource = Helper.GetContext().Orders.Where(x => x.ID_Status != 1&&x.DateOrder==todaysDate).ToList();
        }

        int idstatus = 2;

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
                gridstatus.Visibility = Visibility.Visible;
                txtstatus.Text = "Статус - " + orders.StatusOrder.NameStatusOrder;
                txttable.Text = "Стол - " + orders.Tables.NameTable;
                txtwaiter.Text = "Официант - " + orders.Users.LastName +" " + orders.Users.FirstName.Substring(0,1)+". " + orders.Users.MiddleName.Substring(0,1)+".";
                datagridfoodorder.ItemsSource = Helper.GetContext().ListOrder.Where(x => x.ID_Order == Helper.ID_order).ToList();
                if (orders.ID_Status == 2)
                {
                    combostatus.Items.Add("Готовится");
                    idstatus = 3;
                }
                else if (orders.ID_Status == 3)
                {
                    combostatus.Items.Add("Готов");
                    idstatus = 4;
                }
                else
                {
                    gridstatus.Visibility = Visibility.Hidden;
                    MessageBox.Show("Заказ уже готов!");
                }
            }
        }

        private void savestatus_Click(object sender, RoutedEventArgs e)
        {
            gridstatus.Visibility = Visibility.Hidden;
            combostatus.Items.Clear();
            var update = Helper.GetContext().Orders.Where(x => x.ID_Order == Helper.ID_order).FirstOrDefault();
            update.ID_Status = idstatus;
            Helper.GetContext().SaveChanges();
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            datagridorder.ItemsSource = Helper.GetContext().Orders.Where(x => x.ID_Status != 1&&x.DateOrder==todaysDate).ToList();
        }

        private void btntoback_Click(object sender, RoutedEventArgs e)
        {
            gridstatus.Visibility = Visibility.Hidden;
        }
    }
}
