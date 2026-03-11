using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Praktikum1
{
    public partial class FormQuestions : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=HAZE\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True");

        List<Question> questions = new List<Question>();
        int currentIndex = 0;
        string firstName, lastName;
        int userId = -1;
        int timeLeft = 1500; // 25 минут = 1500 секунд
        int[] userAnswers;
        DateTime startTime;

        public FormQuestions(string firstName, string lastName)
        {
            InitializeComponent();
            this.firstName = firstName;
            this.lastName = lastName;
            startTime = DateTime.Now; // Запоминаем время начала теста

            userAnswers = new int[15];
            for (int i = 0; i < 15; i++) userAnswers[i] = -1;

            LoadQuestions();
            SaveUser();
            SetupTimer();
            ShowQuestion(0);
        }

        void LoadQuestions()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Questions ORDER BY Id", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    questions.Add(new Question
                    {
                        Id = (int)reader["Id"],
                        QuestionText = reader["QuestionText"].ToString(),
                        Option1 = reader["Option1"].ToString(),
                        Option2 = reader["Option2"].ToString(),
                        Option3 = reader["Option3"].ToString(),
                        Option4 = reader["Option4"].ToString(),
                        CorrectOption = (int)reader["CorrectOption"]
                    });
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки вопросов: " + ex.Message);
            }
        }

        void SaveUser()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (FirstName, LastName, TestDate) OUTPUT INSERTED.Id VALUES (@fn, @ln, @date)", conn);
                cmd.Parameters.AddWithValue("@fn", firstName);
                cmd.Parameters.AddWithValue("@ln", lastName);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                userId = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения пользователя: " + ex.Message);
            }
        }

        void SetupTimer()
        {
            timer1.Interval = 1000;
            timer1.Tick += (sender, e) =>
            {
                timeLeft--;
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                label1.Text = $"Осталось времени: {minutes:00}:{seconds:00}";

                if (timeLeft <= 0)
                {
                    timer1.Stop();
                    label1.Text = "Время вышло!";
                    MessageBox.Show("Время вышло! Тест будет завершен.", "Время истекло",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FinishTest();
                }
            };
            timer1.Start();
        }

        void ShowQuestion(int index)
        {
            if (index < 0 || index >= questions.Count) return;

            var q = questions[index];

            lblVopros.Text = $"Вопрос {index + 1} из {questions.Count}";
            lblVoprosi.Text = q.QuestionText;
            radioButton1.Text = q.Option1;
            radioButton2.Text = q.Option2;
            radioButton3.Text = q.Option3;
            radioButton4.Text = q.Option4;

            // Сбрасываем выделение RadioButton
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            // Если уже был выбран ответ, отмечаем его
            if (userAnswers[index] != -1)
            {
                switch (userAnswers[index])
                {
                    case 1: radioButton1.Checked = true; break;
                    case 2: radioButton2.Checked = true; break;
                    case 3: radioButton3.Checked = true; break;
                    case 4: radioButton4.Checked = true; break;
                }
            }

            btnNazad.Enabled = index > 0;
            btnDalee.Text = index == questions.Count - 1 ? "Завершить" : "Далее";
            currentIndex = index;
        }

        bool IsAnswerSelected()
        {
            return radioButton1.Checked || radioButton2.Checked ||
                   radioButton3.Checked || radioButton4.Checked;
        }

        void SaveCurrentAnswer()
        {
            if (radioButton1.Checked) userAnswers[currentIndex] = 1;
            else if (radioButton2.Checked) userAnswers[currentIndex] = 2;
            else if (radioButton3.Checked) userAnswers[currentIndex] = 3;
            else if (radioButton4.Checked) userAnswers[currentIndex] = 4;
        }

        private void btnDalee_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли ответ
            if (!IsAnswerSelected())
            {
                MessageBox.Show("Пожалуйста, выберите вариант ответа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveCurrentAnswer();

            if (currentIndex < questions.Count - 1)
            {
                currentIndex++;
                ShowQuestion(currentIndex);
            }
            else
            {
                FinishTest();
            }
        }

        private void btnNazad_Click(object sender, EventArgs e)
        {
            // При переходе назад тоже сохраняем ответ, если он выбран
            if (IsAnswerSelected())
            {
                SaveCurrentAnswer();
            }
            currentIndex--;
            ShowQuestion(currentIndex);
        }

        void FinishTest()
        {
            timer1.Stop();

            // Подсчет времени прохождения в секундах
            int timeSpent = (int)(DateTime.Now - startTime).TotalSeconds;

            int correct = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                if (userAnswers[i] == questions[i].CorrectOption) correct++;
            }

            try
            {
                conn.Open();

                // Обновляем время прохождения в таблице Users
                SqlCommand updateUser = new SqlCommand("UPDATE Users SET TimeSpent = @time WHERE Id = @id", conn);
                updateUser.Parameters.AddWithValue("@time", timeSpent);
                updateUser.Parameters.AddWithValue("@id", userId);
                updateUser.ExecuteNonQuery();

                // Сохраняем ответы
                for (int i = 0; i < questions.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO UserAnswers (UserId, QuestionId, SelectedOption, IsCorrect) VALUES (@uid, @qid, @opt, @cor)", conn);
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.Parameters.AddWithValue("@qid", questions[i].Id);
                    cmd.Parameters.AddWithValue("@opt", userAnswers[i] == -1 ? DBNull.Value : (object)userAnswers[i]);
                    cmd.Parameters.AddWithValue("@cor", userAnswers[i] == questions[i].CorrectOption);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения ответов: " + ex.Message);
            }

            FormFinish finish = new FormFinish(correct, questions.Count, userId);
            this.Hide();
            finish.ShowDialog();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timeLeft > 0 && currentIndex < questions.Count - 1)
            {
                if (MessageBox.Show("Прервать тест?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }

    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int CorrectOption { get; set; }
    }
}