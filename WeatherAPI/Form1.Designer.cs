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
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.cityLabel = new System.Windows.Forms.Label();
            this.temperatureLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.humidityLabel = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cityListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(456, 12);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(332, 23);
            this.cityTextBox.TabIndex = 0;
            this.cityTextBox.TextChanged += new System.EventHandler(this.cityTextBox_TextChanged);
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.cityLabel.Location = new System.Drawing.Point(39, 15);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(0, 15);
            this.cityLabel.TabIndex = 1;
            // 
            // temperatureLabel
            // 
            this.temperatureLabel.AutoSize = true;
            this.temperatureLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.temperatureLabel.Location = new System.Drawing.Point(84, 39);
            this.temperatureLabel.Name = "temperatureLabel";
            this.temperatureLabel.Size = new System.Drawing.Size(0, 15);
            this.temperatureLabel.TabIndex = 2;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.descriptionLabel.Location = new System.Drawing.Point(78, 64);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(0, 15);
            this.descriptionLabel.TabIndex = 3;
            // 
            // humidityLabel
            // 
            this.humidityLabel.AutoSize = true;
            this.humidityLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.humidityLabel.Location = new System.Drawing.Point(68, 88);
            this.humidityLabel.Name = "humidityLabel";
            this.humidityLabel.Size = new System.Drawing.Size(0, 15);
            this.humidityLabel.TabIndex = 4;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(582, 41);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 6;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.Search);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(2, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "City:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(2, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Temperature:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(2, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(2, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Humidity:";
            // 
            // cityListBox
            // 
            this.cityListBox.FormattingEnabled = true;
            this.cityListBox.ItemHeight = 15;
            this.cityListBox.Location = new System.Drawing.Point(456, 41);
            this.cityListBox.Name = "cityListBox";
            this.cityListBox.Size = new System.Drawing.Size(332, 94);
            this.cityListBox.TabIndex = 11;
            this.cityListBox.Visible = false;
            this.cityListBox.SelectedIndexChanged += new System.EventHandler(this.cityListBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cityListBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.humidityLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.temperatureLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.cityTextBox);
            this.Name = "Form1";
            this.Text = "Weather App";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}