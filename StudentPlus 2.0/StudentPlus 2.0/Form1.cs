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

namespace StudentPlus_2._0
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        
        private SqlDataAdapter sqlDataAdapter = null;
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox1.Text;

            if (login == "Admin" & password == "Admin")
            {
                Form s = new Form2();
                s.Show();
                this.Close();
            }
            else
            {
                try
                {
                    String loginUser = textBox1.Text;
                    String passUser = textBox2.Text;

                    sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivan\source\repos\StudentPlus 2.0\StudentPlus 2.0\Database1.mdf;Integrated Security=True");
                    sqlConnection.Open();

                    DataTable table = new DataTable();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

                    SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Username = @login AND Password = @password", sqlConnection);
                    command.Parameters.Add("@login", SqlDbType.VarChar).Value = loginUser;
                    command.Parameters.Add("@password", SqlDbType.VarChar).Value = passUser;
                    sqlDataAdapter.SelectCommand = command;
                    sqlDataAdapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        this.Hide();
                        Form f1 = new Form2();
                        f1.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели неправильный логин или пароль!", "Ошибка ввода!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivan\source\repos\StudentPlus 2.0\StudentPlus 2.0\Database1.mdf;Integrated Security=True");
            sqlConnection.Open();
        }

        
    }
}
