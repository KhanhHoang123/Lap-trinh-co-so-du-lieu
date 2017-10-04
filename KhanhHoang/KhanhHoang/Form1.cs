using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
namespace KhanhHoang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection cn = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            cn = new SqlConnection();
            string cnStr = "Server = .; Database = QLBanHang; Integrated security = true;";
            cn = new SqlConnection(cnStr);
            dlgPro.DataSource = GetData();
        }
        public void Connect()
        {
            try
            {
                if (cn != null && cn.State != ConnectionState.Open)
                {
                    cn.Open();
                    //MessageBox.Show("Ket noi thanh cong");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Cung cap ten server hoac ket noi da mo" + ex.Message);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Đã xảy ra lỗi kết nối cấp độ khi mở kết nối " + ex.Message);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(" Co hai muc cung ten " + ex.Message);
            }
        }
        public void Disconnect()
        {
            try
            {
                if (cn != null && cn.State != ConnectionState.Closed)
                {
                    cn.Close();
                    MessageBox.Show("Đã ngắt kết nối ");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi mức kết nối đã xảy ra trong khi mở kết nối" + ex.Message);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            Connect();
            string sql = "DELETE FROM LoaiSP WHERE  MaLoaiSP = "+ txtMa.Text.Trim();
            SqlCommand cmd = new SqlCommand(sql, cn);
            int number = 0;
            number = cmd.ExecuteNonQuery();
            MessageBox.Show("Số dòng đã xóa: " + number.ToString());
            dlgPro.DataSource = GetData();
            Disconnect();
        }
        private List<object> GetData()
        {
            Connect();

            string sql = "SELECT * FROM LoaiSP";

            List<object> list = new List<object>();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr = cmd.ExecuteReader();

                string tenloai;
                int maloai;
                while (dr.Read())
                {
                    maloai = dr.GetInt32(0);
                    tenloai = dr.GetString(1);
                   
                    // ...

                    var prod = new
                    {
                        MaSP = maloai,
                        TenSP = tenloai,
                    };
                    list.Add(prod);
                }
                dr.Close();
            }
            catch (SqlException ex) // Internet => Exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return list;
        }

        }
 }

