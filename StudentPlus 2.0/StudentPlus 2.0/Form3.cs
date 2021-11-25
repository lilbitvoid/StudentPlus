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

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
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
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            textBox11.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Name_of_exam"].Value);
            textBox12.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Teacher"].Value);
            textBox10.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Count_students"].Value);
            textBox8.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Date_of_exams"].Value);
            textBox9.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Description"].Value);
        }
        //Доделать
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Name_of_exam LIKE '%{textBox7.Text}%'";
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Teacher LIKE '%{textBox7.Text}%'";
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Count_students LIKE '%{textBox7.Text}%'";            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            dataSet.Tables["Exams"].Rows[rowIndex]["Name_of_exam"] = textBox11.Text;
            dataSet.Tables["Exams"].Rows[rowIndex]["Teacher"] = textBox12.Text;
            dataSet.Tables["Exams"].Rows[rowIndex]["Count_students"] = textBox10.Text;
            dataSet.Tables["Exams"].Rows[rowIndex]["Date_of_exams"] = Convert.ToDateTime(textBox8.Text);
            dataSet.Tables["Exams"].Rows[rowIndex]["Description"] = textBox9.Text;

            sqlDataAdapter.Update(dataSet, "Exams");
            ReloadData();
        }
    }
}
