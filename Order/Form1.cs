using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace Order
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Customer();
            Item();
            Loadd();
          
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-1QBMPQT\\SQLEXPRESS;Initial Catalog=Order; User ID = ; Password= ; Trusted_Connection=true");


        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        string id;
        bool Mode = true;
        string sql;

        public void Customer()
        {
            try
            {
                con.Open();
                sql = "select * from customer";
                cmd = new SqlCommand(sql, con);
                
                read = cmd.ExecuteReader();
                comboCustomer.Items.Clear();

                while (read.Read())
                {
                    //dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                    comboCustomer.Items.Add(read.GetValue(1).ToString());
                }

                read.Close();
                con.Close();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void Item()
        {
            try
            {
                con.Open();
                sql = "select * from items";
                cmd = new SqlCommand(sql, con);

                read = cmd.ExecuteReader();
                comboItems.Items.Clear();

                while (read.Read())
                {
                    //dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                    comboItems.Items.Add(read.GetValue(1).ToString());
                }

                read.Close();
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void Loadd()
        {
            try
            {
                con.Open();
                sql = "select * from orders";
                cmd = new SqlCommand(sql, con);
                
                read = cmd.ExecuteReader();

                //for(int i = 0; i < read.FieldCount; i++)
                //{
                //    Console.WriteLine(read[i]);
                //}
                   

                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0],read[3], read[4], read[5],read[6]);
                }
                read.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void getID(String id)
        {
            sql = "select * from orders where id = '" + id + "'  ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                comboItems.Text = read[3].ToString();
                textUnitPrice.Text= read[4].ToString(); ;
                textQuantity.Text= read[5].ToString(); ;
                textTotal.Text= read[6].ToString(); 
               
            }
            con.Close();
        }









        /// ///////////////

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {
            string orderNumber = textOrder.Text;
            string customerName=comboCustomer.Text;
            string item=comboItems.Text;
            string unitPrice=textUnitPrice.Text;
            string quantity=textQuantity.Text;
            string total=textTotal.Text;
            string customerAddress=textAddress.Text;
            string customerPhone=textPhone.Text;



            //string stname = txtName.Text;
            //string course = txtCourse.Text;
            //string fee = txtFee.Text;

            if (Mode == true)
            {
                sql = "insert into orders(orderNumber,customerName,item,unitPrice,quantity,total,customerAddress,customerPhone) values(@orderNumber,@customerName,@item,@unitPrice,@quantity,@total,@customerAddress,@customerPhone)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                cmd.Parameters.AddWithValue("@customerName", customerName);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@customerAddress", customerAddress);
                cmd.Parameters.AddWithValue("@customerPhone", customerPhone);

                cmd.ExecuteNonQuery();
                
                textOrder.Clear();
                comboCustomer.Items.Clear();
                comboItems.Items.Clear();
                textUnitPrice.Clear();
                textQuantity.Clear();
                textTotal.Clear();
                textAddress.Clear();
                textPhone.Clear();
                MessageBox.Show("Record Added");

             



            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update orders set orderNumber=@orderNumber,customerName=@customerName,item=@item,unitPrice=@unitPrice,quantity=@quantity,total=@total,customerAddress=@customerAddress,customerPhone=@customerPhone where id = @id" ;
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                cmd.Parameters.AddWithValue("@customerName", customerName);
                cmd.Parameters.AddWithValue("@item", item);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@customerAddress", customerAddress);
                cmd.Parameters.AddWithValue("@customerPhone", customerPhone);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updateddddd");


                textOrder.Clear();
                comboCustomer.Items.Clear();
                comboItems.Items.Clear();
                textUnitPrice.Clear();
                textQuantity.Clear();
                textTotal.Clear();
                textAddress.Clear();
                textPhone.Clear();
                Add.Text = "Add";
                Mode = true;

            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                Add.Text = "Edit";

            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from orders where id  = @id ";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id ", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted");
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loadd();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                con.Open();
                sql = "select * from orders";
                cmd = new SqlCommand(sql, con);

                read = cmd.ExecuteReader();

               


                dataGridView2.Rows.Clear();

                while (read.Read())
                {
                    dataGridView2.Rows.Add(read[0], read[1], read[2], read[3], read[4], read[5], read[6], read[7], read[8]);
                }
                read.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
