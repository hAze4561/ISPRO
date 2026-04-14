using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NumberSystemConverter
{
    public partial class Form1 : Form
    {
        // Строка подключения к БД (HAZE\SQLEXPRESS)
        private string connectionString = @"Server=HAZE\SQLEXPRESS;Database=NumberSystemsDB;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
            InitializeComboBoxes();
            LoadHistory();
        }

        // Заполнение ComboBox вариантами систем счисления
        private void InitializeComboBoxes()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            comboBox1.Items.Add("Двоичная (2)");
            comboBox1.Items.Add("Восьмеричная (8)");
            comboBox1.Items.Add("Десятичная (10)");
            comboBox1.Items.Add("Шестнадцатеричная (16)");

            comboBox2.Items.Add("Двоичная (2)");
            comboBox2.Items.Add("Восьмеричная (8)");
            comboBox2.Items.Add("Десятичная (10)");
            comboBox2.Items.Add("Шестнадцатеричная (16)");

            comboBox1.SelectedIndex = 2; // Десятичная по умолчанию
            comboBox2.SelectedIndex = 2;
        }

        // Валидация ввода в зависимости от выбранной системы
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            string selectedSystem = comboBox1.SelectedItem.ToString();
            string input = textBox1.Text;
            string pattern = GetValidationPattern(selectedSystem);

            if (!string.IsNullOrEmpty(input) && !Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                // Удаляем последний недопустимый символ
                textBox1.Text = input.Substring(0, input.Length - 1);
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        // Получение регулярного выражения для валидации
        private string GetValidationPattern(string system)
        {
            if (system.Contains("Двоичная"))
                return "^[01]*$";
            if (system.Contains("Восьмеричная"))
                return "^[0-7]*$";
            if (system.Contains("Десятичная"))
                return "^[0-9]*$";
            if (system.Contains("Шестнадцатеричная"))
                return "^[0-9A-Fa-f]*$";
            return "^.*$";
        }

        // Получение основания системы из строки ComboBox
        private int GetBaseFromComboBox(string systemString)
        {
            if (systemString.Contains("Двоичная")) return 2;
            if (systemString.Contains("Восьмеричная")) return 8;
            if (systemString.Contains("Десятичная")) return 10;
            if (systemString.Contains("Шестнадцатеричная")) return 16;
            return 10;
        }

        // Преобразование числа в десятичную систему (схема Горнера)
        private long ConvertToDecimal(string number, int fromBase)
        {
            if (fromBase == 10)
                return long.Parse(number);

            long result = 0;
            number = number.ToUpper();

            for (int i = 0; i < number.Length; i++)
            {
                int digitValue = GetDigitValue(number[i]);
                if (digitValue >= fromBase)
                    throw new Exception($"Недопустимая цифра '{number[i]}' для системы с основанием {fromBase}");

                result = result * fromBase + digitValue;
            }
            return result;
        }

        // Получение числового значения цифры/буквы
        private int GetDigitValue(char digit)
        {
            if (digit >= '0' && digit <= '9')
                return digit - '0';
            if (digit >= 'A' && digit <= 'F')
                return 10 + (digit - 'A');
            throw new Exception($"Недопустимый символ '{digit}'");
        }

        // Преобразование из десятичной в целевую систему
        private string ConvertFromDecimal(long decimalValue, int toBase)
        {
            if (toBase == 10)
                return decimalValue.ToString();

            if (decimalValue == 0)
                return "0";

            StringBuilder result = new StringBuilder();
            long temp = decimalValue;

            while (temp > 0)
            {
                long remainder = temp % toBase;
                result.Insert(0, GetDigitChar(remainder));
                temp /= toBase;
            }

            return result.ToString();
        }

        // Получение символа для числового значения (для систем >10)
        private string GetDigitChar(long value)
        {
            if (value < 10)
                return value.ToString();
            return ((char)('A' + (value - 10))).ToString();
        }

     
        private void LoadHistory()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT Id, InputNumber, InputBase, OutputNumber, OutputBase, ConversionDate 
                                    FROM ConversionHistory 
                                    ORDER BY ConversionDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Настройка отображения колонок
                    dataGridView1.DataSource = dataTable;

                    // Переименование колонок для удобства
                    if (dataGridView1.Columns.Contains("Id"))
                        dataGridView1.Columns["Id"].HeaderText = "ID";
                    if (dataGridView1.Columns.Contains("InputNumber"))
                        dataGridView1.Columns["InputNumber"].HeaderText = "Исходное число";
                    if (dataGridView1.Columns.Contains("InputBase"))
                        dataGridView1.Columns["InputBase"].HeaderText = "Из системы";
                    if (dataGridView1.Columns.Contains("OutputNumber"))
                        dataGridView1.Columns["OutputNumber"].HeaderText = "Результат";
                    if (dataGridView1.Columns.Contains("OutputBase"))
                        dataGridView1.Columns["OutputBase"].HeaderText = "В систему";
                    if (dataGridView1.Columns.Contains("ConversionDate"))
                        dataGridView1.Columns["ConversionDate"].HeaderText = "Дата конвертации";

                    // Форматирование даты
                    dataGridView1.Columns["ConversionDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
                }
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработка ошибок базы данных
        private void HandleDatabaseError(SqlException ex)
        {
            string errorMessage = "Ошибка при работе с базой данных:\n";

            switch (ex.Number)
            {
                case 53: // Network-related error
                case 2:  // Server not found
                    errorMessage += "Отсутствует подключение к серверу базы данных.\n";
                    errorMessage += "Проверьте, запущен ли SQL Server и правильно ли указан сервер (HAZE\\SQLEXPRESS).";
                    break;
                case 4060: // Database not found
                    errorMessage += "База данных 'NumberSystemsDB' не найдена.\n";
                    errorMessage += "Создайте базу данных и таблицу ConversionHistory.";
                    break;
                case 208: // Invalid object name
                    errorMessage += "Таблица 'ConversionHistory' не существует.\n";
                    errorMessage += "Создайте таблицу в базе данных.";
                    break;
                default:
                    errorMessage += ex.Message;
                    break;
            }

            MessageBox.Show(errorMessage, "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnKonvert_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Введите число для конвертации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string inputNumber = textBox1.Text.Trim();
                int fromBase = GetBaseFromComboBox(comboBox1.SelectedItem.ToString());
                int toBase = GetBaseFromComboBox(comboBox2.SelectedItem.ToString());

                // Конвертация в десятичную систему
                long decimalValue = ConvertToDecimal(inputNumber, fromBase);

                // Конвертация из десятичной в целевую систему
                string result = ConvertFromDecimal(decimalValue, toBase);

                textBox2.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка конвертации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Сначала выполните конвертацию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO ConversionHistory 
                                    (InputNumber, InputBase, OutputNumber, OutputBase, ConversionDate) 
                                    VALUES (@InputNumber, @InputBase, @OutputNumber, @OutputBase, @ConversionDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InputNumber", textBox1.Text.Trim());
                        command.Parameters.AddWithValue("@InputBase", GetBaseFromComboBox(comboBox1.SelectedItem.ToString()));
                        command.Parameters.AddWithValue("@OutputNumber", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@OutputBase", GetBaseFromComboBox(comboBox2.SelectedItem.ToString()));
                        command.Parameters.AddWithValue("@ConversionDate", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Конвертация успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadHistory(); // Обновляем историю
            }
            catch (SqlException ex)
            {
                HandleDatabaseError(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}