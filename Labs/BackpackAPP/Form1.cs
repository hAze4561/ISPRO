using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BackpackApp.Debugging;
using BackpackApp.Models;

namespace BackpackApp
{
    public partial class Form1 : Form
    {
        private List<Item> allItems; // Все предметы из базы данных
        private List<Item> bestCombination; // Лучшая комбинация предметов
        private int maxWeight; // Максимальный вес рюкзака
        private int maxCost; // Максимальная стоимость

        // Строка подключения к базе данных
        private const string ConnectionString = @"Server=HAZE\SQLEXPRESS;Database=backpack;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
            InitializeListView();

            // Проверяем подключение к БД через DatabaseTester
            DatabaseTester.TestConnection();

            // Загружаем данные из БД
            LoadData();
        }

        private void InitializeListView()
        {
            // Очищаем колонки
            listView1.Columns.Clear();

            // Добавляем три колонки: Название, Вес, Стоимость
            listView1.Columns.Add("Название", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Вес (кг)", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Стоимость (руб)", 150, HorizontalAlignment.Right);

            // Настройки ListView
            listView1.View = View.Details; // Табличный вид
            listView1.FullRowSelect = true; // Выделение всей строки
            listView1.GridLines = true; // Отображать сетку
            listView1.MultiSelect = false; // Запрет множественного выбора

            // Очищаем элементы
            listView1.Items.Clear();
        }

        private void LoadData()
        {
            try
            {
                DebugLogger.Log("Загрузка данных из таблицы objects");

                allItems = new List<Item>();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT Id, Name, Weight, Cost FROM objects ORDER BY Id";

                    DebugLogger.LogSqlQuery(query);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Item item = new Item
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Weight = reader.GetInt32(2),
                                    Cost = reader.GetInt32(3)
                                };

                                allItems.Add(item);
                            }
                        }
                    }
                }

                DebugLogger.Log($"Загружено предметов: {allItems.Count}");

                // Отображаем исходные данные в таблице
                DisplayItemsInListView(allItems);
            }
            catch (Exception ex)
            {
                DebugLogger.Log($"Ошибка при загрузке данных: {ex.Message}");
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayItemsInListView(List<Item> items)
        {
            // Очищаем таблицу
            listView1.Items.Clear();

            if (items == null || items.Count == 0)
            {
                DebugLogger.Log("Нет предметов для отображения");
                return;
            }

            // Добавляем каждый предмет в таблицу
            foreach (Item item in items)
            {
                // Создаем строку с названием
                ListViewItem listItem = new ListViewItem(item.Name);

                // Добавляем ячейки: вес и стоимость
                listItem.SubItems.Add(item.Weight.ToString());
                listItem.SubItems.Add(item.Cost.ToString());

                // Сохраняем объект в теге
                listItem.Tag = item;

                // Добавляем строку в таблицу
                listView1.Items.Add(listItem);
            }

            DebugLogger.Log($"Отображено предметов в таблице: {items.Count}");
        }
        private List<Item> SolveBackpackProblem(List<Item> items, int maxWeight, out int totalCost)
        {
            using (new ExecutionTimer("Решение задачи о рюкзаке"))
            {
                DebugLogger.Log($"Начало решения задачи. Максимальный вес: {maxWeight} кг");
                DebugLogger.LogItems(items, "Исходные предметы");

                List<Item> bestCombination = new List<Item>();
                int bestCost = 0;
                int combinationsChecked = 0;

                int n = items.Count;
                int totalCombinations = (int)Math.Pow(2, n);

                DebugLogger.Log($"Всего возможных комбинаций: {totalCombinations}");

                // Полный перебор всех комбинаций (2^n)
                for (int i = 0; i < totalCombinations; i++)
                {
                    List<Item> currentCombination = new List<Item>();
                    int currentWeight = 0;
                    int currentCost = 0;

                    for (int j = 0; j < n; j++)
                    {
                        // Проверяем, включен ли j-й предмет в комбинацию
                        if ((i & (1 << j)) != 0)
                        {
                            currentCombination.Add(items[j]);
                            currentWeight += items[j].Weight;
                            currentCost += items[j].Cost;
                        }
                    }

                    combinationsChecked++;

                    // Проверяем, подходит ли комбинация по весу и лучше ли по стоимости
                    if (currentWeight <= maxWeight && currentCost > bestCost)
                    {
                        bestCost = currentCost;
                        bestCombination = new List<Item>(currentCombination);

                        DebugLogger.Log($"Найдена лучшая комбинация: вес={currentWeight}, стоимость={currentCost}");
                    }

                    // Логируем каждую 100-ю комбинацию
                    if (combinationsChecked % 100 == 0)
                    {
                        DebugLogger.Log($"Проверено комбинаций: {combinationsChecked}/{totalCombinations}");
                    }
                }

                totalCost = bestCost;

                DebugLogger.Log($"Решение завершено. Лучшая стоимость: {bestCost}");
                DebugLogger.LogItems(bestCombination, "Лучшая комбинация");

                return bestCombination;
            }
        }

        private void btnReshiti_Click_1(object sender, EventArgs e)
        {
            DebugLogger.Log("Нажата кнопка Решить");

            try
            {
                // Проверяем ввод максимального веса
                if (!int.TryParse(txtVes.Text, out maxWeight))
                {
                    MessageBox.Show("Пожалуйста, введите корректное число для максимального веса",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (maxWeight <= 0)
                {
                    MessageBox.Show("Максимальный вес должен быть положительным числом",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (allItems == null || allItems.Count == 0)
                {
                    MessageBox.Show("Нет данных о предметах. Пожалуйста, загрузите данные.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Решаем задачу
                bestCombination = SolveBackpackProblem(allItems, maxWeight, out maxCost);

                // Отображаем результат в таблице
                DisplayItemsInListView(bestCombination);

                // Показываем сообщение с результатом
                MessageBox.Show($"Найдено оптимальное решение!\n\n" +
                              $"Общая стоимость: {maxCost} руб.\n" +
                              $"Количество предметов: {bestCombination.Count}",
                              "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DebugLogger.Log($"Ошибка при решении задачи: {ex.Message}");
                MessageBox.Show($"Ошибка при решении задачи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDannie_Click_1(object sender, EventArgs e)
        {
            DebugLogger.Log("Нажата кнопка Показать исходные данные");

            if (allItems != null && allItems.Count > 0)
            {
                // Отображаем исходные данные в таблице
                DisplayItemsInListView(allItems);
            }
            else
            {
                // Если данных нет, загружаем их
                LoadData();
            }
        }
    }
}