namespace ClientMySQL
{
    partial class Register3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register3));
            this.passCheck = new System.Windows.Forms.Button();
            this.loginCheck = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.login = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.passport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FIO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // passCheck
            // 
            this.passCheck.BackColor = System.Drawing.Color.White;
            this.passCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.passCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.passCheck.ForeColor = System.Drawing.SystemColors.ControlText;
            this.passCheck.Location = new System.Drawing.Point(288, 38);
            this.passCheck.Name = "passCheck";
            this.passCheck.Size = new System.Drawing.Size(28, 23);
            this.passCheck.TabIndex = 33;
            this.passCheck.Text = "?";
            this.passCheck.UseVisualStyleBackColor = false;
            this.passCheck.Click += new System.EventHandler(this.PassCheck_Click);
            // 
            // loginCheck
            // 
            this.loginCheck.BackColor = System.Drawing.Color.White;
            this.loginCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginCheck.ForeColor = System.Drawing.SystemColors.ControlText;
            this.loginCheck.Location = new System.Drawing.Point(288, 9);
            this.loginCheck.Name = "loginCheck";
            this.loginCheck.Size = new System.Drawing.Size(28, 23);
            this.loginCheck.TabIndex = 32;
            this.loginCheck.Text = "?";
            this.loginCheck.UseVisualStyleBackColor = false;
            this.loginCheck.Click += new System.EventHandler(this.LoginCheck_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(12, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(271, 16);
            this.label7.TabIndex = 31;
            this.label7.Text = "Персональные данные сотрудника:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Crimson;
            this.label6.Location = new System.Drawing.Point(18, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 30;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.AliceBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(158, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 37);
            this.button1.TabIndex = 29;
            this.button1.Text = "Регистрация";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(158, 41);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(124, 20);
            this.password.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(12, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Пароль";
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(158, 11);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(124, 20);
            this.login.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(12, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Логин";
            // 
            // passport
            // 
            this.passport.Location = new System.Drawing.Point(152, 117);
            this.passport.Name = "passport";
            this.passport.Size = new System.Drawing.Size(249, 20);
            this.passport.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Номер паспорта*";
            // 
            // FIO
            // 
            this.FIO.Location = new System.Drawing.Point(152, 90);
            this.FIO.Name = "FIO";
            this.FIO.Size = new System.Drawing.Size(249, 20);
            this.FIO.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "ФИО*";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(15, 152);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(215, 19);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "Может вносить изменения в БД";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // Register3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(429, 246);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.passCheck);
            this.Controls.Add(this.loginCheck);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.login);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FIO);
            this.Controls.Add(this.label1);
            this.Name = "Register3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация сотрудника";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button passCheck;
        private System.Windows.Forms.Button loginCheck;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox login;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox passport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FIO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}