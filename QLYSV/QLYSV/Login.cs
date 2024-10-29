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

namespace QLYSV
{
    // Lớp tĩnh để lưu thông tin phiên làm việc của người dùng
    public static class Session
    {
        public static int UserId { get; set; }      // Biến tĩnh lưu ID người dùng
        public static string ChucVu { get; set; }   // Biến lưu chức vụ người dùng
    }

    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ TextBox
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Chuỗi kết nối tới cơ sở dữ liệu
            string connectionString = "Data Source=LAPTOP-2ADC50PC\\SQLEXPRESS;Initial Catalog=QLYSV;Integrated Security=True;";

            // Câu truy vấn kiểm tra username và password và lấy ID, chức vụ
            string query = "SELECT ID, ChucVu FROM Member WHERE Username = @Username AND Password = @Password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Lưu ID người dùng và chức vụ vào Session
                    Session.UserId = Convert.ToInt32(reader["ID"]);
                    Session.ChucVu = reader["ChucVu"].ToString();

                    // Điều hướng tới form tương ứng dựa trên ChucVu
                    if (Session.ChucVu == "GV")
                    {
                        Form1 formGiangVien = new Form1();
                        formGiangVien.FormClosed += (s, args) => this.Show();  // Hiển thị lại formLogin khi form1 đóng
                        formGiangVien.Show();
                    }
                    else if (Session.ChucVu == "Sinh viên")
                    {
                        FrmSV formSinhVien = new FrmSV();
                        formSinhVien.FormClosed += (s, args) => this.Show();  // Hiển thị lại formLogin khi frmSV đóng
                        formSinhVien.Show();
                    }

                    // Ẩn form login sau khi đăng nhập thành công
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Xóa các trường nhập sau khi xử lý
                textBox1.Text = "";
                textBox2.Text = "";

                reader.Close();

                // Kiểm tra và hiển thị các sự kiện sắp tới
                CheckUpcomingEvents(conn);

                conn.Close();
            }
        }

        // Phương thức kiểm tra các sự kiện trong vòng 3 ngày tới
        private void CheckUpcomingEvents(SqlConnection conn)
        {
            // Câu truy vấn để lấy các sự kiện diễn ra từ hôm nay đến 3 ngày sau
            string eventQuery = "SELECT TenSuKien, ThoiGian, Khoa FROM SuKien WHERE ThoiGian >= @CurrentDate AND ThoiGian <= @EndDate ORDER BY ThoiGian ASC";

            // Lấy ngày hiện tại
            DateTime currentDate = DateTime.Now;
            DateTime endDate = currentDate.AddDays(3); // Ngày kết thúc là 3 ngày sau

            using (SqlCommand eventCmd = new SqlCommand(eventQuery, conn))
            {
                eventCmd.Parameters.AddWithValue("@CurrentDate", currentDate);
                eventCmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataReader eventReader = eventCmd.ExecuteReader();

                // Kiểm tra nếu có sự kiện trong vòng 3 ngày tới
                if (eventReader.HasRows)
                {
                    // Tạo một chuỗi để lưu trữ danh sách các sự kiện
                    StringBuilder eventDetails = new StringBuilder();
                    eventDetails.AppendLine("Các sự kiện diễn ra trong vòng 3 ngày tới:");

                    while (eventReader.Read())
                    {
                        string eventName = eventReader["TenSuKien"].ToString();
                        DateTime eventTime = (DateTime)eventReader["ThoiGian"];
                        string khoa = eventReader["Khoa"].ToString();

                        // Thêm thông tin của từng sự kiện vào chuỗi
                        eventDetails.AppendLine($"Sự kiện: {eventName}");
                        eventDetails.AppendLine($"Thời gian: {eventTime:dd/MM/yyyy}");
                        eventDetails.AppendLine($"Khoa: {khoa}");
                        eventDetails.AppendLine(); // Dòng trống giữa các sự kiện
                    }

                    // Hiển thị thông báo về tất cả các sự kiện trong vòng 3 ngày tới
                    MessageBox.Show(eventDetails.ToString(), "Thông báo sự kiện", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có sự kiện nào trong vòng 3 ngày tới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                eventReader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Có thể thực hiện các thao tác khởi tạo khi form login tải lên
        }
    }
}
