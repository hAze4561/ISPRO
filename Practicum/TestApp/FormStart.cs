using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Praktikum1;

namespace Praktikum1
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        private void btnNacalo_Click(object sender, EventArgs e)
        {
            // Проверка, что поля не пустые
            if (string.IsNullOrWhiteSpace(txtImya.Text) || string.IsNullOrWhiteSpace(txtFamilia.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя и фамилию!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем и открываем форму с вопросами
            FormQuestions formQuestions = new FormQuestions(txtImya.Text, txtFamilia.Text);
            this.Hide(); // Скрываем текущую форму
            formQuestions.ShowDialog(); // Показываем форму вопросов модально
            this.Close(); // Закрываем стартовую форму после возврата
        }
    }
}
