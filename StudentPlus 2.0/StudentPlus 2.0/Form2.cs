using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudentPlus_2._0
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;
        public Form2()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' as [Command] FROM Students", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Students");

                dataGridView1.DataSource = dataSet.Tables["Students"];

                dataGridView1.Columns[0].HeaderText = "Код записи";
                dataGridView1.Columns[1].HeaderText = "Имя студента";
                dataGridView1.Columns[2].HeaderText = "Фамилия студента";
                dataGridView1.Columns[3].HeaderText = "Отчество студента";
                dataGridView1.Columns[4].HeaderText = "Дата урока";
                dataGridView1.Columns[5].HeaderText = "Код группы";

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Students"].Clear();
                sqlDataAdapter.Fill(dataSet, "Students");

                dataGridView1.DataSource = dataSet.Tables["Students"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-IOIHD8T\SQLEXPRESS;Initial Catalog=DB;Integrated Security=True");
            sqlConnection.Open();
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Students"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Students");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Students"].NewRow();

                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["Surname"] = dataGridView1.Rows[rowIndex].Cells["Surname"].Value;
                        row["Middlename"] = dataGridView1.Rows[rowIndex].Cells["Middlename"].Value;
                        row["Date_of_lesson"] = dataGridView1.Rows[rowIndex].Cells["Date_of_lesson"].Value;
                        row["Group"] = dataGridView1.Rows[rowIndex].Cells["Group"].Value;

                        dataSet.Tables["Students"].Rows.Add(row);

                        dataSet.Tables["Students"].Rows.RemoveAt(dataSet.Tables["Students"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Students");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                        dataSet.Tables["Students"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["Students"].Rows[r]["Surname"] = dataGridView1.Rows[r].Cells["Surname"].Value;
                        dataSet.Tables["Students"].Rows[r]["Middlename"] = dataGridView1.Rows[r].Cells["Middlename"].Value;
                        dataSet.Tables["Students"].Rows[r]["Date_of_lesson"] = dataGridView1.Rows[r].Cells["Date_of_lesson"].Value;
                        dataSet.Tables["Students"].Rows[r]["Group"] = dataGridView1.Rows[r].Cells["Group"].Value;

                        sqlDataAdapter.Update(dataSet, "Students");

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Программист балбес", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update";


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Программист балбес", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            AboutBox1 info = new AboutBox1();
            info.ShowDialog();
        }

        private void btn_ex_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }
        // Плохо работает из за метода!!!
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Name LIKE '%{textBox_search.Text}%'";
        }

    }
}
