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
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ivan\source\repos\StudentPlus 2.0\StudentPlus 2.0\Database1.mdf;Integrated Security=True");
            sqlConnection.Open();
            LoadData();

        }
        private void LoadData()
        {
            sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Exams", sqlConnection);
            sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

            sqlBuilder.GetInsertCommand();
            sqlBuilder.GetUpdateCommand();
            sqlBuilder.GetDeleteCommand();

            DataTable table = new DataTable();

            dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet, "Exams");

            dataGridView1.DataSource = dataSet.Tables["Exams"];
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Exams"].Clear();

                sqlDataAdapter.Fill(dataSet, "Exams");

                dataGridView1.DataSource = dataSet.Tables["Exams"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow row = dataSet.Tables["Exams"].NewRow();

            row["Name_of_exam"] = textBox1.Text;
            row["Teacher"] = textBox2.Text;
            row["Count_students"] = textBox3.Text;
            row["Date_of_exams"] = Convert.ToDateTime(textBox4.Text);
            row["Description"] = textBox5.Text;

            dataSet.Tables["Exams"].Rows.Add(row);

            sqlDataAdapter.Update(dataSet, "Exams");
            ReloadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;                

                    dataSet.Tables["Exams"].Rows[rowIndex].Delete();

                    sqlDataAdapter.Update(dataSet, "Exams");
                
                    ReloadData();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex + 1;

            textBox6.Text = Convert.ToString(rowIndex);
        }
    }
}
