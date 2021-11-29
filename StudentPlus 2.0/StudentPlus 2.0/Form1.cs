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
            string login = textBox_login.Text;
            string password = textBox_login.Text;

            if (login == "Admin" & password == "Admin")
            {
                this.Hide();
                Form2 s = new Form2();
                s.ShowDialog();
                this.Close();
            }
            else
            {
                try
                {
                    String loginUser = textBox_login.Text;
                    String passUser = textBox_pass.Text;

                    sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivan\source\repos\StudentPlus 2.0\StudentPlus 2.0\Database1.mdf;Integrated Security=True");
                    sqlConnection.Open();

                    DataTable table = new DataTable();

                    sqlDataAdapter = new SqlDataAdapter();

                    SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Username = @login AND Password = @password", sqlConnection);
                    command.Parameters.Add("@login", SqlDbType.VarChar).Value = loginUser;
                    command.Parameters.Add("@password", SqlDbType.VarChar).Value = passUser;
                    sqlDataAdapter.SelectCommand = command;
                    sqlDataAdapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        this.Hide();
                        AccesToken.accesToken = 0;
                        Form2 f1 = new Form2();
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
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-IOIHD8T\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True");
            sqlConnection.Open();
        }
    }
}
