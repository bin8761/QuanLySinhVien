using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLYSV.UC_Controll
{
    public partial class BoMon : UserControl
    {
        public BoMon()
        {
            InitializeComponent();
            loaddata();
        }

        private void loaddata()
        {
            // Chuỗi kết nối tới cơ sở dữ liệu
            string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS ;Initial Catalog=QLYSV;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            // Câu truy vấn chỉ lấy dữ liệu của Giảng viên
            string query = "SELECT * FROM BoMon";

            // Tạo SqlDataAdapter và DataTable để lưu dữ liệu
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            // Đưa dữ liệu vào DataGridView (đã có sẵn cột)
            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = dataGridView1.Rows.Add(); // Thêm một dòng mới vào DataGridView

                // Đưa dữ liệu vào các cột tương ứng của DataGridView
                dataGridView1.Rows[rowIndex].Cells["MaBoMon"].Value = row["MaBoMon"];
                dataGridView1.Rows[rowIndex].Cells["TenBoMon"].Value = row["TenBoMon"];
               
            }
        }

        string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS ;Initial Catalog=QLYSV;Integrated Security=True;";
        private void button5_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Mã bộ môn và tên bộ môn là bắt buộc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the data from the form
            string mabomon = textBox5.Text;
            string tenbomon = textBox6.Text;


            // Connection and query
            string query = "SET IDENTITY_INSERT BoMon ON; " +  // Bật IDENTITY_INSERT
               "INSERT INTO BoMon (MaBoMon, TenBoMon) " +
               "VALUES (@mabomon, @tenbomon); " +
               "SET IDENTITY_INSERT BoMon OFF;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@MaBoMon", mabomon);
                    cmd.Parameters.AddWithValue("@TenBoMon", tenbomon);


                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm bộ môn thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Không thêm được bộ môn. Vui lòng thử lại.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            dataGridView1.Rows.Clear();
            loaddata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    textBox5.Text = row.Cells["MaBoMon"].Value.ToString();
                    textBox6.Text = row.Cells["TenBoMon"].Value.ToString();
                   
                }
            }
            catch
            {



            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "") return;

            DialogResult kq = MessageBox.Show("Xóa bộ môn " + textBox6.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS ;Initial Catalog=QLYSV;Integrated Security=True;";

                // Sử dụng câu truy vấn với tham số
                string sql = "DELETE FROM BoMon WHERE MaBoMon = @MaBoMon";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Thêm tham số để tránh SQL Injection
                        cmd.Parameters.AddWithValue("@MaBoMon", textBox5.Text);

                        try
                        {
                            conn.Open();  // Mở kết nối với cơ sở dữ liệu
                            int rowsAffected = cmd.ExecuteNonQuery();  // Thực thi câu lệnh xóa

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa bộ môn thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy bộ môn để xóa.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message);
                        }
                    }

                    // Cập nhật lại DataGridView sau khi xóa thành công
                    dataGridView1.Rows.Clear();
                    loaddata();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Mã bộ môn và tên bộ môn là bắt buộc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult kq = MessageBox.Show("Sửa bộ môn " + textBox6.Text + " không?", "Sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string connectionString = "Data Source=ADMIN-PC\\SQLEXPRESS ;Initial Catalog=QLYSV;Integrated Security=True;";

                // Sử dụng câu truy vấn với tham số để cập nhật bộ môn
                string sql = "UPDATE BoMon SET TenBoMon = @TenBoMon WHERE MaBoMon = @MaBoMon";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Thêm tham số để tránh SQL Injection
                        cmd.Parameters.AddWithValue("@MaBoMon", textBox5.Text); // Mã bộ môn cần sửa
                        cmd.Parameters.AddWithValue("@TenBoMon", textBox6.Text); // Tên bộ môn mới

                        try
                        {
                            conn.Open();  // Mở kết nối cơ sở dữ liệu
                            int rowsAffected = cmd.ExecuteNonQuery();  // Thực thi câu lệnh sửa

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Sửa bộ môn thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy bộ môn để sửa.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message);
                        }
                    }

                    // Cập nhật lại DataGridView sau khi sửa thành công
                    dataGridView1.Rows.Clear();
                    loaddata(); // Hàm tải lại dữ liệu vào DataGridView
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
