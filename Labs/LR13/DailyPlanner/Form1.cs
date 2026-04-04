using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DailyPlanner
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Server=HAZE\SQLEXPRESS;Database=DailyPlannerDB;Trusted_Connection=True;";
        private int? selectedNoteId = null; // ID выбранной заметки

        public Form1()
        {
            InitializeComponent();

            // Настройка DateTimePicker для отображения только времени
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;

            // Загрузка заметок на сегодняшнюю дату
            LoadNotes();
        }

        // Загрузка заметок на выбранную дату
        private void LoadNotes()
        {
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT Id, NoteDate, NoteText 
                                   FROM Notes 
                                   WHERE CAST(NoteDate AS DATE) = @SelectedDate 
                                   ORDER BY NoteDate ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    listNotes.Items.Clear();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime noteDate = reader.GetDateTime(1);
                        string noteText = reader.GetString(2);

                        string displayText = $"{noteDate:HH:mm} - {TruncateText(noteText, 60)}";

                        ListBoxItem item = new ListBoxItem
                        {
                            Id = id,
                            DisplayText = displayText,
                            FullText = noteText,
                            NoteDateTime = noteDate
                        };

                        listNotes.Items.Add(item);
                    }

                    reader.Close();

                    if (listNotes.Items.Count == 0)
                    {
                        listNotes.Items.Add("Нет заметок на этот день");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Обрезка длинного текста
        private string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;
            return text.Substring(0, maxLength) + "...";
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string noteText = txtNoteText.Text.Trim();

            if (string.IsNullOrWhiteSpace(noteText))
            {
                MessageBox.Show("Введите текст заметки!", "Предупреждение",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            DateTime selectedTime = dateTimePicker1.Value;
            DateTime fullDateTime = selectedDate.Add(selectedTime.TimeOfDay);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Notes (NoteDate, NoteText, CreatedAt) 
                                   VALUES (@NoteDate, @NoteText, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NoteDate", fullDateTime);
                    cmd.Parameters.AddWithValue("@NoteText", noteText);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Заметка успешно добавлена!", "Успех",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtNoteText.Clear();
                    dateTimePicker1.Value = DateTime.Now;
                    selectedNoteId = null;
                    LoadNotes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listNotes_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listNotes.SelectedItem is ListBoxItem item && item.Id > 0)
            {
                selectedNoteId = item.Id;
                txtNoteText.Text = item.FullText;
                dateTimePicker1.Value = item.NoteDateTime;
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (selectedNoteId == null)
            {
                MessageBox.Show("Сначала выберите заметку для редактирования!", "Предупреждение",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string noteText = txtNoteText.Text.Trim();

            if (string.IsNullOrWhiteSpace(noteText))
            {
                MessageBox.Show("Введите текст заметки!", "Предупреждение",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            DateTime selectedTime = dateTimePicker1.Value;
            DateTime fullDateTime = selectedDate.Add(selectedTime.TimeOfDay);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE Notes 
                                   SET NoteDate = @NoteDate, NoteText = @NoteText, CreatedAt = GETDATE() 
                                   WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NoteDate", fullDateTime);
                    cmd.Parameters.AddWithValue("@NoteText", noteText);
                    cmd.Parameters.AddWithValue("@Id", selectedNoteId.Value);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Заметка успешно обновлена!", "Успех",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtNoteText.Clear();
                    dateTimePicker1.Value = DateTime.Now;
                    selectedNoteId = null;
                    LoadNotes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtNoteText.Clear();
            dateTimePicker1.Value = DateTime.Now;
            selectedNoteId = null;
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedNoteId == null)
            {
                MessageBox.Show("Выберите заметку для удаления!", "Предупреждение",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту заметку?",
                                                  "Подтверждение удаления",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Notes WHERE Id = @Id";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", selectedNoteId.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Заметка удалена!", "Успех",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);

                        txtNoteText.Clear();
                        dateTimePicker1.Value = DateTime.Now;
                        selectedNoteId = null;
                        LoadNotes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadNotes();
        }
    }

    // Вспомогательный класс для хранения данных заметки в ListBox
    public class ListBoxItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public string FullText { get; set; }
        public DateTime NoteDateTime { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}