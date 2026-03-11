using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Praktikum1
{
    public partial class FormFinish : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=HAZE\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True");
        int correctAnswers;
        int totalQuestions;
        int userId;

        public FormFinish(int correctAnswers, int totalQuestions, int userId)
        {
            InitializeComponent();
            this.correctAnswers = correctAnswers;
            this.totalQuestions = totalQuestions;
            this.userId = userId;

            DisplayResults();
            LoadCurrentUserResult(); // Загружаем только текущего пользователя
        }

        void DisplayResults()
        {
            lblOtvet.Text = $"Правильных ответов: {correctAnswers} из {totalQuestions}";

            double percentage = (double)correctAnswers / totalQuestions * 100;
            string grade = GetGrade(percentage);
            lblResult.Text = $"Результат: {percentage:F1}% - {grade}";
        }

        string GetGrade(double percentage)
        {
            if (percentage >= 90) return "Отлично (5)";
            if (percentage >= 75) return "Хорошо (4)";
            if (percentage >= 60) return "Удовлетворительно (3)";
            return "Попробуйте еще раз";
        }

        void LoadCurrentUserResult()
        {
            try
            {
                conn.Open();

                // SQL запрос ТОЛЬКО для текущего пользователя
                string query = @"
                    SELECT 
                        u.FirstName + ' ' + u.LastName AS 'Пользователь',
                        FORMAT(u.TestDate, 'dd.MM.yyyy HH:mm') AS 'Дата теста',
                        @correct AS 'Баллы',
                        u.TimeSpent AS 'Время (сек)'
                    FROM Users u
                    WHERE u.Id = @userId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@correct", correctAnswers);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                // Привязываем данные к dataGridView1
                dataGridView1.DataSource = table;

                // Настройка внешнего вида DataGridView
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.RowHeadersVisible = false;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки результата: " + ex.Message);
            }
        }

        

        private void btnVixod_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnZanovo1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Пройти тест заново?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FormStart start = new FormStart();
                this.Hide();
                start.ShowDialog();
                this.Close();
            }
        }
    }
}