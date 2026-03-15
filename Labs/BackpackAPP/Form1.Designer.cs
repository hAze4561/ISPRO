namespace BackpackApp
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVes = new System.Windows.Forms.TextBox();
            this.btnReshiti = new System.Windows.Forms.Button();
            this.btnDannie = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(437, 426);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(467, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Максимальный вес рюкзака: ";
            // 
            // txtVes
            // 
            this.txtVes.Location = new System.Drawing.Point(680, 34);
            this.txtVes.Name = "txtVes";
            this.txtVes.Size = new System.Drawing.Size(89, 22);
            this.txtVes.TabIndex = 2;
            // 
            // btnReshiti
            // 
            this.btnReshiti.Location = new System.Drawing.Point(588, 86);
            this.btnReshiti.Name = "btnReshiti";
            this.btnReshiti.Size = new System.Drawing.Size(95, 34);
            this.btnReshiti.TabIndex = 1;
            this.btnReshiti.Text = "Решить";
            this.btnReshiti.Click += new System.EventHandler(this.btnReshiti_Click_1);
            // 
            // btnDannie
            // 
            this.btnDannie.Location = new System.Drawing.Point(542, 387);
            this.btnDannie.Name = "btnDannie";
            this.btnDannie.Size = new System.Drawing.Size(208, 37);
            this.btnDannie.TabIndex = 0;
            this.btnDannie.Text = "Показать исходные данные";
            this.btnDannie.Click += new System.EventHandler(this.btnDannie_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 450);
            this.Controls.Add(this.btnDannie);
            this.Controls.Add(this.btnReshiti);
            this.Controls.Add(this.txtVes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Задача о рюкзаке";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVes;
        private System.Windows.Forms.Button btnReshiti;
        private System.Windows.Forms.Button btnDannie;
    }
}

