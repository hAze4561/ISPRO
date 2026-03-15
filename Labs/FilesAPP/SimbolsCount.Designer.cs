namespace FilesApp
{
    partial class SimbolsCount
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnCountUp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(125, 35);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Открыть";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click_1);
            // 
            // btnCountUp
            // 
            this.btnCountUp.Location = new System.Drawing.Point(143, 12);
            this.btnCountUp.Name = "btnCountUp";
            this.btnCountUp.Size = new System.Drawing.Size(125, 35);
            this.btnCountUp.TabIndex = 1;
            this.btnCountUp.Text = "Подсчитать";
            this.btnCountUp.UseVisualStyleBackColor = true;
            this.btnCountUp.Click += new System.EventHandler(this.btnCountUp_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(274, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(125, 35);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(405, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(125, 35);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(536, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(125, 35);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(12, 73);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(300, 16);
            this.lblInfo1.TabIndex = 5;
            this.lblInfo1.Text = "Введите текст или выберите файл с текстом";
            // 
            // lblInfo2
            // 
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Location = new System.Drawing.Point(12, 369);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(210, 16);
            this.lblInfo2.TabIndex = 6;
            this.lblInfo2.Text = "Количество символов в тексте";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 93);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(662, 22);
            this.txtPath.TabIndex = 7;
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(15, 400);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(206, 22);
            this.txtCount.TabIndex = 8;
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(14, 135);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(659, 222);
            this.txtText.TabIndex = 9;
            // 
            // SimbolsCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 450);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.lblInfo1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCountUp);
            this.Controls.Add(this.btnOpen);
            this.Name = "SimbolsCount";
            this.Text = "Подсчет символов в тексте";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnCountUp;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Label lblInfo2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.TextBox txtText;
    }
}

