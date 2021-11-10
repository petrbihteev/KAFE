using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для ReportWaiter.xaml
    /// </summary>
    public partial class ReportWaiter : Window
    {

        SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=Cafe;Integrated Security=True");

        public ReportWaiter()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            con.Open();
            string sql = "SELECT [Tables].NameTable,Users.LastName,WayPay.NameWayPay,StatusOrder.NameStatusOrder,DateOrder,Sum(Food.Price*ListOrder.Quantity) FROM Orders " +
"inner join Users on Orders.ID_User = Users.ID_User inner join[Tables] on Orders.ID_Table = [Tables].ID_Table inner join WayPay on Orders.ID_WayPay = WayPay.ID_Pay " +
"inner join StatusOrder on Orders.ID_Status = StatusOrder.ID_StatusOrder inner join ListOrder on Orders.ID_Order = ListOrder.ID_Order inner join Food on ListOrder.ID_Food = Food.ID_Food " +
"group by[Tables].NameTable,Users.LastName,WayPay.NameWayPay,StatusOrder.NameStatusOrder,DateOrder,Users.ID_User Having Orders.DateOrder = '" + todaysDate+ "' AND Users.ID_User='"+Helper.ID_user+"'";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, con);
            DataTable data = new DataTable("Orders");
            dataAdapter.Fill(data);
            datagridreport.ItemsSource = data.DefaultView;
            dataAdapter.Update(data);
            con.Close();
            datagridreport.Columns[0].Header = "Номер стола";
            datagridreport.Columns[1].Header = "Фамилия официанта";
            datagridreport.Columns[2].Header = "Способ оплаты";
            datagridreport.Columns[3].Header = "Статус заказа";
            datagridreport.Columns[4].Header = "Дата заказа";
            datagridreport.Columns[5].Header = "Стомость";
            (datagridreport.Columns[4] as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            (datagridreport.Columns[5] as DataGridTextColumn).Binding.StringFormat = "0.00";

        }
    }
}
