using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dBDataSet.Teachers". При необходимости она может быть перемещена или удалена.
            this.teachersTableAdapter.Fill(this.dBDataSet.Teachers);
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-IOIHD8T\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
            if (AccesToken.accesToken == 0)
            {
                tabControl_ex.Visible = false;
                btn_setting.Visible = false;
            }
        }
        private void LoadData()
        {
            sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Exams", sqlConnection);
            sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

            sqlBuilder.GetInsertCommand();
            sqlBuilder.GetUpdateCommand();
            sqlBuilder.GetDeleteCommand();

            dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet, "Exams");

            dataGridView1.DataSource = dataSet.Tables["Exams"];

            dataGridView1.Columns[0].HeaderText = "Код экзамена";
            dataGridView1.Columns[1].HeaderText = "Название экзамена";
            dataGridView1.Columns[2].HeaderText = "Учитель";
            dataGridView1.Columns[3].HeaderText = "Колличество допущенных студентов";
            dataGridView1.Columns[4].HeaderText = "Дата проведения";
            dataGridView1.Columns[5].HeaderText = "Описание";
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

            row["Name_of_exam"] = textBox_name.Text;
            row["Teacher"] = comboBox_teach.SelectedValue;
            row["Count_students"] = textBox_cout_stud.Text;
            row["Date_of_exams"] = Convert.ToDateTime(textBox_date.Text);
            row["Description"] = textBox_dec.Text;

            dataSet.Tables["Exams"].Rows.Add(row);

            sqlDataAdapter.Update(dataSet, "Exams");



            textBox_name.Clear();
            textBox_cout_stud.Clear();
            textBox_date.Clear();
            textBox_dec.Clear();

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
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            textBox_name_update.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Name_of_exam"].Value);
            textBox_count_stud_update.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Count_students"].Value);
            textBox_date_update.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Date_of_exams"].Value);
            textBox_desc_update.Text = Convert.ToString(dataGridView1.Rows[rowIndex].Cells["Description"].Value);
        }
        //Доделать
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Name_of_exam LIKE '%{textBox_search.Text}%'";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            dataSet.Tables["Exams"].Rows[rowIndex]["Name_of_exam"] = textBox_name_update.Text;
            dataSet.Tables["Exams"].Rows[rowIndex]["Teacher"] = Convert.ToInt32(comboBox_name_teach_update.SelectedValue); //Convert.ToInt32(textBox12.Text);
            dataSet.Tables["Exams"].Rows[rowIndex]["Count_students"] = textBox_count_stud_update.Text;
            dataSet.Tables["Exams"].Rows[rowIndex]["Date_of_exams"] = Convert.ToDateTime(textBox_date_update.Text);
            dataSet.Tables["Exams"].Rows[rowIndex]["Description"] = textBox_desc_update.Text;

            sqlDataAdapter.Update(dataSet, "Exams");
            ReloadData();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            AboutBox1 info = new AboutBox1();
            info.ShowDialog();
        }
    }
}
