namespace Praktik1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl1 = new System.Windows.Forms.Label();
            this.cmbBox1 = new System.Windows.Forms.ComboBox();
            this.btnSumma = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lstBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBox1 = new System.Windows.Forms.TextBox();
            this.btnDob = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(253, 58);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(283, 16);
            this.lbl1.TabIndex = 0;
            this.lbl1.Text = "Выберите продукты ля покупки из списка:";
            // 
            // cmbBox1
            // 
            this.cmbBox1.FormattingEnabled = true;
            this.cmbBox1.Location = new System.Drawing.Point(296, 98);
            this.cmbBox1.Name = "cmbBox1";
            this.cmbBox1.Size = new System.Drawing.Size(180, 24);
            this.cmbBox1.TabIndex = 1;
            // 
            // btnSumma
            // 
            this.btnSumma.Location = new System.Drawing.Point(186, 195);
            this.btnSumma.Name = "btnSumma";
            this.btnSumma.Size = new System.Drawing.Size(78, 75);
            this.btnSumma.TabIndex = 2;
            this.btnSumma.Text = "Сумма";
            this.btnSumma.UseVisualStyleBackColor = true;
            this.btnSumma.Click += new System.EventHandler(this.btnSumma_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(501, 195);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 75);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lstBox1
            // 
            this.lstBox1.FormattingEnabled = true;
            this.lstBox1.ItemHeight = 16;
            this.lstBox1.Location = new System.Drawing.Point(296, 180);
            this.lstBox1.Name = "lstBox1";
            this.lstBox1.Size = new System.Drawing.Size(177, 100);
            this.lstBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Итог сумма:";
            // 
            // txtBox1
            // 
            this.txtBox1.Location = new System.Drawing.Point(342, 301);
            this.txtBox1.Name = "txtBox1";
            this.txtBox1.Size = new System.Drawing.Size(100, 22);
            this.txtBox1.TabIndex = 6;
            // 
            // btnDob
            // 
            this.btnDob.Location = new System.Drawing.Point(318, 139);
            this.btnDob.Name = "btnDob";
            this.btnDob.Size = new System.Drawing.Size(123, 31);
            this.btnDob.TabIndex = 7;
            this.btnDob.Text = "Добавить";
            this.btnDob.UseVisualStyleBackColor = true;
            this.btnDob.Click += new System.EventHandler(this.btnDob_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDob);
            this.Controls.Add(this.txtBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSumma);
            this.Controls.Add(this.cmbBox1);
            this.Controls.Add(this.lbl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.ComboBox cmbBox1;
        private System.Windows.Forms.Button btnSumma;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox lstBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBox1;
        private System.Windows.Forms.Button btnDob;
    }
}

