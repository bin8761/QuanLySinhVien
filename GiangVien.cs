using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLYSV.UC_Controll
{
    public partial class GiangVien : UserControl
    {
        public GiangVien()
        {
            InitializeComponent();
           
        }

        private void Loaddata()
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS ;Initial Catalog=QLYSV;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            // Câu truy vấn chỉ lấy dữ liệu của Giảng viên
            string query = "SELECT * FROM Member WHERE ChucVu = 'GV'";

            // Tạo SqlDataAdapter và DataTable để lưu dữ liệu
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Đưa dữ liệu vào DataGridView (đã có sẵn cột)
            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = dataGridView2.Rows.Add(); // Thêm một dòng mới vào DataGridView

                // Đưa dữ liệu vào các cột tương ứng của DataGridView
                dataGridView2.Rows[rowIndex].Cells["IDGV"].Value = row["ID"];
                dataGridView2.Rows[rowIndex].Cells["TENGV"].Value = row["HoTen"];
                dataGridView2.Rows[rowIndex].Cells["DiachiGV"].Value = row["DiaChi"];
                dataGridView2.Rows[rowIndex].Cells["SDTGV"].Value = row["SoDienThoai"];
                dataGridView2.Rows[rowIndex].Cells["BOMON"].Value = row["BoMon"];


            }

        }

        private void GiangVien_Load(object sender, EventArgs e)
        {
            Loaddata();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            Loaddata();
        }



        //private void button7_Click(object sender, EventArgs e)
        //{
        //    // Lấy từ khóa tìm kiếm từ TextBox
        //    string searchName = textBox9.Text.Trim();

        //    // Chuỗi kết nối tới cơ sở dữ liệu
        //    string connectionString = "Data Source=DESKTOP-NEW\\SERVER;Initial Catalog=QLYSV;Integrated Security=True;";
        //    SqlConnection connection = new SqlConnection(connectionString);

        //    // Câu truy vấn chỉ lấy dữ liệu của Giảng viên có tên chứa từ khóa tìm kiếm
        //    string query = "SELECT * FROM Member WHERE ChucVu = 'GV' AND HoTen LIKE @searchName";

        //    // Tạo SqlCommand để thêm tham số vào truy vấn
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");

        //    // Tạo SqlDataAdapter và DataTable để lưu dữ liệu
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        //    DataTable dataTable = new DataTable();
        //    dataAdapter.Fill(dataTable);

        //    // Đưa dữ liệu vào DataGridView (đã có sẵn cột)
        //    dataGridView2.Rows.Clear(); // Xóa dữ liệu cũ trong DataGridView
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        int rowIndex = dataGridView2.Rows.Add(); // Thêm một dòng mới vào DataGridView

        //        // Đưa dữ liệu vào các cột tương ứng của DataGridView
        //        dataGridView2.Rows[rowIndex].Cells["IDGV"].Value = row["ID"];
        //        dataGridView2.Rows[rowIndex].Cells["TENGV"].Value = row["HoTen"];
        //        dataGridView2.Rows[rowIndex].Cells["DiachiGV"].Value = row["DiaChi"];
        //        dataGridView2.Rows[rowIndex].Cells["SDTGV"].Value = row["SoDienThoai"];
        //    }

        //}
    }
}
