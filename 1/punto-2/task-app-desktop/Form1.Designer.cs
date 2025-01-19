namespace task_app_desktop
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
            titlelbl = new Label();
            txtTitle = new TextBox();
            label1 = new Label();
            txtDescription = new RichTextBox();
            saveBtn = new Button();
            dtpDesde = new DateTimePicker();
            label2 = new Label();
            label3 = new Label();
            dtpHasta = new DateTimePicker();
            cbTaskStatuses = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // titlelbl
            // 
            titlelbl.AutoSize = true;
            titlelbl.Location = new Point(44, 68);
            titlelbl.Name = "titlelbl";
            titlelbl.Size = new Size(37, 15);
            titlelbl.TabIndex = 0;
            titlelbl.Text = "Titulo";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(44, 86);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(352, 23);
            txtTitle.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 131);
            label1.Name = "label1";
            label1.Size = new Size(128, 15);
            label1.TabIndex = 2;
            label1.Text = "Descripcion (Opcional)";
            // 
            // txtDescription
            // 
            txtDescription.BackColor = SystemColors.ControlLightLight;
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.Location = new Point(44, 159);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(352, 208);
            txtDescription.TabIndex = 4;
            txtDescription.Text = "";
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(44, 391);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(95, 33);
            saveBtn.TabIndex = 5;
            saveBtn.Text = "Guardar";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += button1_Click;
            // 
            // dtpDesde
            // 
            dtpDesde.Location = new Point(441, 86);
            dtpDesde.MinDate = new DateTime(2025, 1, 18, 0, 0, 0, 0);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(200, 23);
            dtpDesde.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(441, 68);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 7;
            label2.Text = "Fecha Desde";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(441, 141);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 9;
            label3.Text = "Fecha Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(441, 159);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(200, 23);
            dtpHasta.TabIndex = 8;
            // 
            // cbTaskStatuses
            // 
            cbTaskStatuses.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTaskStatuses.FormattingEnabled = true;
            cbTaskStatuses.Location = new Point(441, 235);
            cbTaskStatuses.Name = "cbTaskStatuses";
            cbTaskStatuses.Size = new Size(200, 23);
            cbTaskStatuses.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(441, 217);
            label4.Name = "label4";
            label4.Size = new Size(42, 15);
            label4.TabIndex = 11;
            label4.Text = "Estado";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label5.Location = new Point(44, 23);
            label5.Name = "label5";
            label5.Size = new Size(132, 25);
            label5.TabIndex = 12;
            label5.Text = "Agregar Tarea";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HighlightText;
            ClientSize = new Size(741, 485);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(cbTaskStatuses);
            Controls.Add(label3);
            Controls.Add(dtpHasta);
            Controls.Add(label2);
            Controls.Add(dtpDesde);
            Controls.Add(saveBtn);
            Controls.Add(txtDescription);
            Controls.Add(label1);
            Controls.Add(txtTitle);
            Controls.Add(titlelbl);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titlelbl;
        private TextBox txtTitle;
        private Label label1;
        private RichTextBox txtDescription;
        private Button saveBtn;
        private DateTimePicker dtpDesde;
        private Label label2;
        private Label label3;
        private DateTimePicker dtpHasta;
        private ComboBox cbTaskStatuses;
        private Label label4;
        private Label label5;
    }
}
