using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Praktik1
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=HAZE\\SQLEXPRESS;Initial Catalog=proekt;Integrated Security=True";


        public Form1()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                cmbBox1.Items.Clear();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, name, price FROM products";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string productInfo = $"{reader["name"]} - {reader["price"]} руб.";
                        cmbBox1.Items.Add(productInfo);
                    }
                    reader.Close();
                }

                if (cmbBox1.Items.Count > 0)
                    cmbBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void btnDob_Click(object sender, EventArgs e)
        {
            if (cmbBox1.SelectedItem != null)
            {
                lstBox1.Items.Add(cmbBox1.SelectedItem);
            }
            else
            {
                MessageBox.Show("Выберите продукт из списка");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstBox1.Items.Clear();
            txtBox1.Text = "";

        }

        private void btnSumma_Click(object sender, EventArgs e)
        {
            decimal total = 0;

            foreach (var item in lstBox1.Items)
            {
                string itemString = item.ToString();
                int lastDashIndex = itemString.LastIndexOf('-');
                if (lastDashIndex != -1)
                {
                    string priceString = itemString.Substring(lastDashIndex + 1)
                        .Replace("руб.", "")
                        .Trim();

                    if (decimal.TryParse(priceString, out decimal price))
                    {
                        total += price;
                    }
                }
            }

            txtBox1.Text = $"{total:F2} руб.";

        }
    }
}

