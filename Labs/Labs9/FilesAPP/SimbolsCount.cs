using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FilesApp
{
    public partial class SimbolsCount : Form
    {
        // ИЗМЕНЕНО: ваше подключение к SQL Server Express
        private string connectionString = @"Server=HAZE\SQLEXPRESS;Database=FileHistoryDB;Trusted_Connection=True;";

        public SimbolsCount()
        {
            InitializeComponent();
            CreateDatabase(); // Создаем БД при запуске
        }

        private void CreateDatabase()
        {
            // Точка останова 1 -- создание базы данных
            try
            {
                // Сначала подключаемся к master, чтобы создать базу данных, если её нет
                string masterConnectionString = @"Server=HAZE\SQLEXPRESS;Database=master;Trusted_Connection=True;";

                using (var masterConnection = new SqlConnection(masterConnectionString))
                {
                    masterConnection.Open();

                    // Проверяем, существует ли база данных FileHistoryDB
                    string checkDbQuery = @"
                        IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FileHistoryDB')
                        BEGIN
                            CREATE DATABASE FileHistoryDB
                        END";

                    using (var cmd = new SqlCommand(checkDbQuery, masterConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Теперь подключаемся к нашей БД и создаем таблицу
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем, существует ли таблица
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FileOperations' AND xtype='U')
                        BEGIN
                            CREATE TABLE FileOperations (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                FilePath NVARCHAR(500),
                                Content NVARCHAR(MAX),
                                SymbolCount INT,
                                OperationType NVARCHAR(50),
                                OperationDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    using (var command = new SqlCommand(checkTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                System.Diagnostics.Debug.WriteLine("База данных и таблица созданы успешно");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при создании БД: {ex.Message}");
                MessageBox.Show($"Ошибка при создании базы данных: {ex.Message}\n\n" +
                    "Убедитесь, что SQL Server Express (HAZE\\SQLEXPRESS) запущен.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void SaveToDatabase(string filePath, string content, int symbolCount, string operationType)
        {
            // Точка останова 7 -- сохранение в БД
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    // Точка останова 8 -- подключение к БД
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO FileOperations (FilePath, Content, SymbolCount, OperationType)
                        VALUES (@FilePath, @Content, @SymbolCount, @OperationType)";

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FilePath", filePath ?? "Без пути");
                        command.Parameters.AddWithValue("@Content", content ?? "");
                        command.Parameters.AddWithValue("@SymbolCount", symbolCount);
                        command.Parameters.AddWithValue("@OperationType", operationType);

                        command.ExecuteNonQuery();
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Операция {operationType} сохранена в БД");
            }
            catch (Exception ex)
            {
                // Записываем ошибку в отладочный вывод, но не показываем пользователю
                System.Diagnostics.Debug.WriteLine($"Ошибка при сохранении в БД: {ex.Message}");
            }
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?",
               "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Debug.WriteLine("Приложение закрыто");
                Application.Exit();
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtText.Clear();
            txtCount.Clear();
            // Путь не очищаем, чтобы можно было сохранить в тот же файл
            System.Diagnostics.Debug.WriteLine("Поля очищены");
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPath.Text))
                {
                    // Если путь не указан, вызываем диалог сохранения
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                        saveFileDialog.FilterIndex = 1;
                        saveFileDialog.RestoreDirectory = true;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            txtPath.Text = saveFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                // Сохраняем файл
                File.WriteAllText(txtPath.Text, txtText.Text, Encoding.Default);

                // Подсчитываем символы
                int count = txtText.Text.Length;

                // Сохраняем в БД операцию сохранения
                SaveToDatabase(txtPath.Text, txtText.Text, count, "SAVE");

                MessageBox.Show("Файл успешно сохранен!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void btnCountUp_Click_1(object sender, EventArgs e)
        {
            // Точка останова 6 -- проверка подсчета
            int count = txtText.Text.Length;
            txtCount.Text = count.ToString();

            System.Diagnostics.Debug.WriteLine($"Подсчитано символов: {count}");
        }

        private void btnOpen_Click_1(object sender, EventArgs e)
        {
            // Точка останова 1
            System.Diagnostics.Debug.WriteLine("Начало открытия файла");

            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Настройка диалога открытия файла
                    openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    // Точка останова 2 -- проверка открытия диалога
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Точка останова 3 -- файл выбран
                        txtPath.Text = openFileDialog.FileName;
                        System.Diagnostics.Debug.WriteLine($"Выбран файл: {openFileDialog.FileName}");

                        // Точка останова 4 -- чтение файла
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                        System.Diagnostics.Debug.WriteLine($"Прочитано символов: {content.Length}");

                        // Точка останова 5 -- отображение текста
                        txtText.Text = content;

                        // Автоматический подсчет символов
                        btnCountUp_Click_1(sender, e);

                        // Сохраняем в БД операцию открытия
                        SaveToDatabase(openFileDialog.FileName, content, content.Length, "OPEN");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}