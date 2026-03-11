using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SkladApp
{
    public partial class Sklad : Form
    {
        private string connectionString = @"Data Source=HAZE\SQLEXPRESS;Initial Catalog=sklad;Integrated Security=True";

        public Sklad()
        {
            InitializeComponent();
            LoadProducts();
            LoadComboBox();
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT id, name FROM products";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    comboBox1.DataSource = table;
                    comboBox1.DisplayMember = "name";  // Отображаемое поле
                    comboBox1.ValueMember = "id";      // Значение поля
                    comboBox1.SelectedIndex = -1;      // Сброс выбора
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ComboBox: " + ex.Message);
            }
        }

        private void AddNewProduct()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtStillage.Text) ||
                string.IsNullOrEmpty(txtCell.Text) || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                string name = txtName.Text;
                int stillage = Convert.ToInt32(txtStillage.Text);
                int cell = Convert.ToInt32(txtCell.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO products (name, stillage, cell, quantity) VALUES (@name, @stillage, @cell, @quantity)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@stillage", stillage);
                    command.Parameters.AddWithValue("@cell", cell);
                    command.Parameters.AddWithValue("@quantity", quantity);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                LoadProducts();
                LoadComboBox(); // Обновляем ComboBox после добавления
                ClearFields();
                MessageBox.Show("Продукт добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtStillage.Clear();
            txtCell.Clear();
            txtQuantity.Clear();
            comboBox1.SelectedIndex = -1;
        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                var result = MessageBox.Show("Удалить выбранный продукт?", "Подтверждение удаления",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string query = "DELETE FROM products WHERE id=@id";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@id", id);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        LoadProducts();
                        LoadComboBox(); // Обновляем ComboBox после удаления
                        ClearFields();
                        MessageBox.Show("Продукт удален");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите продукт для удаления");
            }
        }

        private void btnAdd1_Click_1(object sender, EventArgs e)
        {
            AddNewProduct();
        }

        private void btnOpen_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                txtName.Text = dataGridView1.CurrentRow.Cells["name"].Value?.ToString();
                txtStillage.Text = dataGridView1.CurrentRow.Cells["stillage"].Value?.ToString();
                txtCell.Text = dataGridView1.CurrentRow.Cells["cell"].Value?.ToString();
                txtQuantity.Text = dataGridView1.CurrentRow.Cells["quantity"].Value?.ToString();

                // Установка выбора в ComboBox
                string selectedName = dataGridView1.CurrentRow.Cells["name"].Value?.ToString();
                if (!string.IsNullOrEmpty(selectedName))
                {
                    comboBox1.Text = selectedName;
                }
            }
            else
            {
                MessageBox.Show("Выберите продукт из таблицы");
            }
        }

        private void btnAdd2_Click_1(object sender, EventArgs e)
        {
            AddNewProduct();
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.FileName = "products_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine("СПИСОК ПРОДУКТОВ НА СКЛАДЕ");
                        writer.WriteLine("Дата экспорта: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        writer.WriteLine(new string('-', 50));

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                writer.WriteLine($"ID: {row.Cells["id"].Value}");
                                writer.WriteLine($"Название: {row.Cells["name"].Value}");
                                writer.WriteLine($"Стеллаж: {row.Cells["stillage"].Value}, Ячейка: {row.Cells["cell"].Value}");
                                writer.WriteLine($"Количество: {row.Cells["quantity"].Value}");
                                writer.WriteLine(new string('-', 30));
                            }
                        }
                    }
                    MessageBox.Show("Данные сохранены в файл: " + saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения в файл: " + ex.Message);
            }
        }

        private void btnSearchByName_Click_1(object sender, EventArgs e)
        {
            string searchName = txtSearchName.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Введите название для поиска");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM products WHERE name LIKE @name";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@name", "%" + searchName + "%");

                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }

                if (dataGridView1.Rows.Count == 1) // только новая строка
                {
                    MessageBox.Show("Продукты с таким названием не найдены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void btnSearchByCoords_Click_1(object sender, EventArgs e)
        {
            try
            {
                int stillage = Convert.ToInt32(domainUpDownStillage.Text);
                int cell = Convert.ToInt32(domainUpDownCell.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM products WHERE stillage=@stillage AND cell=@cell";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@stillage", stillage);
                    adapter.SelectCommand.Parameters.AddWithValue("@cell", cell);

                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }

                if (dataGridView1.Rows.Count == 1) // только новая строка
                {
                    MessageBox.Show("Продукты с такими координатами не найдены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                txtName.Text = dataGridView1.CurrentRow.Cells["name"].Value?.ToString();
                txtStillage.Text = dataGridView1.CurrentRow.Cells["stillage"].Value?.ToString();
                txtCell.Text = dataGridView1.CurrentRow.Cells["cell"].Value?.ToString();
                txtQuantity.Text = dataGridView1.CurrentRow.Cells["quantity"].Value?.ToString();

                // Установка выбора в ComboBox
                string selectedName = dataGridView1.CurrentRow.Cells["name"].Value?.ToString();
                if (!string.IsNullOrEmpty(selectedName))
                {
                    comboBox1.Text = selectedName;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && comboBox1.SelectedIndex >= 0)
            {
                try
                {
                    int selectedId = Convert.ToInt32(comboBox1.SelectedValue);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT * FROM products WHERE id = @id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id", selectedId);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            txtName.Text = reader["name"].ToString();
                            txtStillage.Text = reader["stillage"].ToString();
                            txtCell.Text = reader["cell"].ToString();
                            txtQuantity.Text = reader["quantity"].ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                
                }
            }
        }
    }
}