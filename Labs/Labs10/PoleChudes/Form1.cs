using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace PoleChudes
{
    public partial class Form1 : Form
    {
        // Подключение к базе данных
        private string connectionString = @"Data Source=HAZE\SQLEXPRESS;Initial Catalog=PoleChudesDB;Integrated Security=True";

        // Переменные игры
        private string currentWord; // Загаданное слово
        private string shuffledWord; // Перемешанное слово
        private List<Button> letterButtons; // Список кнопок с буквами
        private List<string> userLetters; // Собранные пользователем буквы
        private Stack<Tuple<int, string>> undoStack; // Стек для отмены действий

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Инициализация списков
            letterButtons = new List<Button>();
            userLetters = new List<string>();
            undoStack = new Stack<Tuple<int, string>>();

            // Добавляем все кнопки с буквами в список
            letterButtons.Add(btn1);
            letterButtons.Add(btn2);
            letterButtons.Add(btn3);
            letterButtons.Add(btn4);
            letterButtons.Add(btn5);
            letterButtons.Add(btn6);
            letterButtons.Add(btn7);
            letterButtons.Add(btn8);
            letterButtons.Add(btn9);
            letterButtons.Add(btn10);

            // Подписываем все кнопки на одно событие
            foreach (Button btn in letterButtons)
            {
                btn.Click += LetterButton_Click;
            }

            // Очищаем текстовое поле
            txtSlovo.Clear();
            txtSlovo.ReadOnly = true;

            // Делаем кнопку "Проверить" доступной
            btnProverka.Enabled = true;
        }

        // Метод для получения случайного слова из базы данных
        private string GetRandomWordFromDB()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 Word FROM Words ORDER BY NEWID()";
                    SqlCommand command = new SqlCommand(query, connection);
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("В базе данных нет слов!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Метод для перемешивания букв
        private string ShuffleWord(string word)
        {
            char[] array = word.ToCharArray();
            Random rng = new Random();

            // Алгоритм Фишера-Йетса для перемешивания
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return new string(array);
        }

        // Метод для обновления интерфейса (отображения букв на кнопках)
        private void UpdateInterface()
        {
            // Скрываем все кнопки
            foreach (Button btn in letterButtons)
            {
                btn.Visible = false;
            }

            // Отображаем кнопки с буквами
            for (int i = 0; i < shuffledWord.Length; i++)
            {
                if (i < letterButtons.Count)
                {
                    letterButtons[i].Text = shuffledWord[i].ToString();
                    letterButtons[i].Visible = true;
                    letterButtons[i].Enabled = true;
                }
            }

            // Обновляем текстовое поле с собранным словом
            txtSlovo.Text = string.Join("", userLetters);

            // Визуально показываем длину загаданного слова
            this.Text = $"Поле Чудес - Загадано слово из {currentWord.Length} букв";
        }

        // Обработчик нажатия на букву
        private void LetterButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && clickedButton.Enabled)
            {
                // Сохраняем действие для отмены
                int currentPosition = userLetters.Count;
                string letter = clickedButton.Text;
                undoStack.Push(new Tuple<int, string>(currentPosition, letter));

                // Добавляем букву к собранному слову
                userLetters.Add(letter);

                // Делаем кнопку недоступной
                clickedButton.Enabled = false;

                // Обновляем интерфейс
                UpdateInterface();
            }
        }

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            currentWord = GetRandomWordFromDB();

            if (string.IsNullOrEmpty(currentWord))
            {
                return;
            }

            // Приводим к верхнему регистру и заменяем Ё на Е
            currentWord = currentWord.ToUpper();
            currentWord = currentWord.Replace('Ё', 'Е');

            // Перемешиваем буквы
            shuffledWord = ShuffleWord(currentWord);

            // Очищаем собранные буквы
            userLetters.Clear();

            // Очищаем стек отмены
            undoStack.Clear();

            // Обновляем интерфейс
            UpdateInterface();
        }

        private void btnProverka_Click_1(object sender, EventArgs e)
        {
            string userWord = string.Join("", userLetters);

            if (string.IsNullOrEmpty(userWord))
            {
                MessageBox.Show("Вы еще не выбрали ни одной буквы!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, совпадает ли собранное слово с загаданным
            if (userWord.Equals(currentWord, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Поздравляю! Вы угадали слово: {currentWord}",
                    "Победа!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Неправильно! Загаданное слово: {currentWord}",
                    "Результат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOtmena_Click_1(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                // Получаем последнее действие
                var lastAction = undoStack.Pop();
                int position = lastAction.Item1;
                string letter = lastAction.Item2;

                // Удаляем последнюю букву из собранного слова
                if (userLetters.Count > 0)
                {
                    userLetters.RemoveAt(userLetters.Count - 1);
                }

                // Находим и активируем кнопку с этой буквой
                foreach (Button btn in letterButtons)
                {
                    if (btn.Text == letter && !btn.Enabled && btn.Visible)
                    {
                        btn.Enabled = true;
                        break;
                    }
                }

                // Обновляем интерфейс
                UpdateInterface();
            }
            else
            {
                MessageBox.Show("Нет действий для отмены!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}