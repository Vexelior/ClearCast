namespace WeatherAPI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cityTextBox = new System.Windows.Forms.TextBox();
            cityLabel = new System.Windows.Forms.Label();
            temperatureLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            humidityLabel = new System.Windows.Forms.Label();
            searchButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            cityListBox = new System.Windows.Forms.ListBox();
            weatherPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)weatherPictureBox).BeginInit();
            SuspendLayout();
            // 
            // cityTextBox
            // 
            cityTextBox.Location = new System.Drawing.Point(12, 12);
            cityTextBox.Name = "cityTextBox";
            cityTextBox.Size = new System.Drawing.Size(332, 23);
            cityTextBox.TabIndex = 0;
            cityTextBox.TextChanged += CityTextBox_TextChanged;
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.BackColor = System.Drawing.Color.Transparent;
            cityLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cityLabel.ForeColor = System.Drawing.SystemColors.Control;
            cityLabel.Location = new System.Drawing.Point(55, 78);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new System.Drawing.Size(0, 20);
            cityLabel.TabIndex = 1;
            // 
            // temperatureLabel
            // 
            temperatureLabel.AutoSize = true;
            temperatureLabel.BackColor = System.Drawing.Color.Transparent;
            temperatureLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            temperatureLabel.ForeColor = System.Drawing.SystemColors.Control;
            temperatureLabel.Location = new System.Drawing.Point(114, 102);
            temperatureLabel.Name = "temperatureLabel";
            temperatureLabel.Size = new System.Drawing.Size(0, 20);
            temperatureLabel.TabIndex = 2;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.BackColor = System.Drawing.Color.Transparent;
            descriptionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            descriptionLabel.ForeColor = System.Drawing.SystemColors.Control;
            descriptionLabel.Location = new System.Drawing.Point(106, 127);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(0, 20);
            descriptionLabel.TabIndex = 3;
            // 
            // humidityLabel
            // 
            humidityLabel.AutoSize = true;
            humidityLabel.BackColor = System.Drawing.Color.Transparent;
            humidityLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            humidityLabel.ForeColor = System.Drawing.SystemColors.Control;
            humidityLabel.Location = new System.Drawing.Point(91, 151);
            humidityLabel.Name = "humidityLabel";
            humidityLabel.Size = new System.Drawing.Size(0, 20);
            humidityLabel.TabIndex = 4;
            // 
            // searchButton
            // 
            searchButton.Location = new System.Drawing.Point(138, 41);
            searchButton.Name = "searchButton";
            searchButton.Size = new System.Drawing.Size(75, 23);
            searchButton.TabIndex = 6;
            searchButton.Text = "Search";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += Search;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.Control;
            label1.Location = new System.Drawing.Point(12, 78);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(37, 20);
            label1.TabIndex = 7;
            label1.Text = "City:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.Control;
            label2.Location = new System.Drawing.Point(12, 102);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(96, 20);
            label2.TabIndex = 8;
            label2.Text = "Temperature:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = System.Drawing.Color.Transparent;
            label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.ForeColor = System.Drawing.SystemColors.Control;
            label3.Location = new System.Drawing.Point(12, 127);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(88, 20);
            label3.TabIndex = 9;
            label3.Text = "Description:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.ForeColor = System.Drawing.SystemColors.Control;
            label4.Location = new System.Drawing.Point(12, 151);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(73, 20);
            label4.TabIndex = 10;
            label4.Text = "Humidity:";
            // 
            // cityListBox
            // 
            cityListBox.FormattingEnabled = true;
            cityListBox.ItemHeight = 15;
            cityListBox.Location = new System.Drawing.Point(12, 30);
            cityListBox.Name = "cityListBox";
            cityListBox.Size = new System.Drawing.Size(332, 94);
            cityListBox.TabIndex = 11;
            cityListBox.Visible = false;
            // 
            // weatherPictureBox
            // 
            weatherPictureBox.BackColor = System.Drawing.Color.Transparent;
            weatherPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            weatherPictureBox.Location = new System.Drawing.Point(266, 97);
            weatherPictureBox.Name = "weatherPictureBox";
            weatherPictureBox.Size = new System.Drawing.Size(50, 50);
            weatherPictureBox.TabIndex = 13;
            weatherPictureBox.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(358, 202);
            Controls.Add(cityListBox);
            Controls.Add(weatherPictureBox);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(searchButton);
            Controls.Add(humidityLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(temperatureLabel);
            Controls.Add(cityLabel);
            Controls.Add(cityTextBox);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Weather App";
            ((System.ComponentModel.ISupportInitialize)weatherPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.Label temperatureLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label humidityLabel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox cityListBox;
        private System.Windows.Forms.PictureBox weatherPictureBox;
    }
}