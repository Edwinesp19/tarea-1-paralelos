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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
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
            formTitle = new Label();
            tabControl1 = new TabControl();
            Tareas = new TabPage();
            dgv_tasks = new DataGridView();
            panel2 = new Panel();
            button2 = new Button();
            label6 = new Label();
            TareasForm = new TabPage();
            txtId = new TextBox();
            panel1 = new Panel();
            button1 = new Button();
            tabControl1.SuspendLayout();
            Tareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_tasks).BeginInit();
            panel2.SuspendLayout();
            TareasForm.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // titlelbl
            // 
            titlelbl.AutoSize = true;
            titlelbl.Location = new Point(44, 148);
            titlelbl.Name = "titlelbl";
            titlelbl.Size = new Size(47, 20);
            titlelbl.TabIndex = 0;
            titlelbl.Text = "Titulo";
            // 
            // txtTitle
            // 
            txtTitle.BackColor = Color.WhiteSmoke;
            txtTitle.BorderStyle = BorderStyle.None;
            txtTitle.Location = new Point(44, 177);
            txtTitle.Margin = new Padding(3, 4, 3, 4);
            txtTitle.Multiline = true;
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = " Escribe algo...";
            txtTitle.Size = new Size(457, 124);
            txtTitle.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 331);
            label1.Name = "label1";
            label1.Size = new Size(161, 20);
            label1.TabIndex = 2;
            label1.Text = "Descripcion (Opcional)";
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.WhiteSmoke;
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.Location = new Point(44, 370);
            txtDescription.Margin = new Padding(30);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(457, 264);
            txtDescription.TabIndex = 4;
            txtDescription.Text = "";
            // 
            // saveBtn
            // 
            saveBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            saveBtn.BackColor = Color.Teal;
            saveBtn.FlatStyle = FlatStyle.Flat;
            saveBtn.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            saveBtn.ForeColor = Color.White;
            saveBtn.Location = new Point(970, 25);
            saveBtn.Margin = new Padding(3, 4, 3, 4);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(143, 49);
            saveBtn.TabIndex = 5;
            saveBtn.Text = "Guardar";
            saveBtn.UseVisualStyleBackColor = false;
            saveBtn.Click += button1_Click;
            // 
            // dtpDesde
            // 
            dtpDesde.ImeMode = ImeMode.NoControl;
            dtpDesde.Location = new Point(563, 177);
            dtpDesde.Margin = new Padding(3, 4, 3, 4);
            dtpDesde.MinDate = new DateTime(2025, 1, 18, 0, 0, 0, 0);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(378, 27);
            dtpDesde.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(563, 153);
            label2.Name = "label2";
            label2.Size = new Size(93, 20);
            label2.TabIndex = 7;
            label2.Text = "Fecha Desde";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(563, 250);
            label3.Name = "label3";
            label3.Size = new Size(89, 20);
            label3.TabIndex = 9;
            label3.Text = "Fecha Hasta";
            // 
            // dtpHasta
            // 
            dtpHasta.Location = new Point(563, 274);
            dtpHasta.Margin = new Padding(3, 4, 3, 4);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(378, 27);
            dtpHasta.TabIndex = 8;
            // 
            // cbTaskStatuses
            // 
            cbTaskStatuses.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTaskStatuses.FormattingEnabled = true;
            cbTaskStatuses.ItemHeight = 20;
            cbTaskStatuses.Location = new Point(563, 370);
            cbTaskStatuses.Margin = new Padding(3, 4, 3, 4);
            cbTaskStatuses.Name = "cbTaskStatuses";
            cbTaskStatuses.Size = new Size(378, 28);
            cbTaskStatuses.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(563, 346);
            label4.Name = "label4";
            label4.Size = new Size(54, 20);
            label4.TabIndex = 11;
            label4.Text = "Estado";
            // 
            // formTitle
            // 
            formTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            formTitle.AutoSize = true;
            formTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            formTitle.ForeColor = Color.Teal;
            formTitle.Location = new Point(35, 28);
            formTitle.Name = "formTitle";
            formTitle.Size = new Size(180, 35);
            formTitle.TabIndex = 12;
            formTitle.Text = "Agregar Tarea";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(Tareas);
            tabControl1.Controls.Add(TareasForm);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1159, 743);
            tabControl1.TabIndex = 13;
            tabControl1.Click += tabControl1_Click;
            // 
            // Tareas
            // 
            Tareas.BackColor = Color.White;
            Tareas.Controls.Add(dgv_tasks);
            Tareas.Controls.Add(panel2);
            Tareas.Location = new Point(4, 29);
            Tareas.Name = "Tareas";
            Tareas.Padding = new Padding(3);
            Tareas.Size = new Size(1151, 710);
            Tareas.TabIndex = 0;
            Tareas.Text = "Tareas";
            // 
            // dgv_tasks
            // 
            dgv_tasks.AllowUserToAddRows = false;
            dgv_tasks.AllowUserToDeleteRows = false;
            dgv_tasks.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dgv_tasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv_tasks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv_tasks.BackgroundColor = Color.White;
            dgv_tasks.BorderStyle = BorderStyle.None;
            dgv_tasks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.Teal;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgv_tasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgv_tasks.ColumnHeadersHeight = 29;
            dgv_tasks.GridColor = Color.Gainsboro;
            dgv_tasks.Location = new Point(38, 126);
            dgv_tasks.Margin = new Padding(60);
            dgv_tasks.Name = "dgv_tasks";
            dgv_tasks.ReadOnly = true;
            dgv_tasks.RowHeadersWidth = 51;
            dgv_tasks.RowTemplate.Height = 70;
            dgv_tasks.RowTemplate.Resizable = DataGridViewTriState.True;
            dgv_tasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_tasks.ShowEditingIcon = false;
            dgv_tasks.Size = new Size(1070, 495);
            dgv_tasks.TabIndex = 15;
            dgv_tasks.CellDoubleClick += dgv_tasks_CellDoubleClick;
            // 
            // panel2
            // 
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.BackColor = Color.Linen;
            panel2.Controls.Add(button2);
            panel2.Controls.Add(label6);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1145, 97);
            panel2.TabIndex = 14;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            button2.BackColor = Color.DarkOrange;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(959, 25);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(156, 49);
            button2.TabIndex = 13;
            button2.Text = "Exportar en PDF";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label6.ForeColor = Color.DarkOrange;
            label6.Location = new Point(35, 28);
            label6.Name = "label6";
            label6.Size = new Size(216, 35);
            label6.TabIndex = 12;
            label6.Text = "Listado de Tareas";
            // 
            // TareasForm
            // 
            TareasForm.BackColor = Color.White;
            TareasForm.Controls.Add(txtId);
            TareasForm.Controls.Add(panel1);
            TareasForm.Controls.Add(label2);
            TareasForm.Controls.Add(txtDescription);
            TareasForm.Controls.Add(label4);
            TareasForm.Controls.Add(label1);
            TareasForm.Controls.Add(dtpDesde);
            TareasForm.Controls.Add(txtTitle);
            TareasForm.Controls.Add(cbTaskStatuses);
            TareasForm.Controls.Add(titlelbl);
            TareasForm.Controls.Add(dtpHasta);
            TareasForm.Controls.Add(label3);
            TareasForm.Location = new Point(4, 29);
            TareasForm.Name = "TareasForm";
            TareasForm.Padding = new Padding(3);
            TareasForm.Size = new Size(1151, 710);
            TareasForm.TabIndex = 1;
            TareasForm.Text = "Formulario";
            // 
            // txtId
            // 
            txtId.BackColor = Color.White;
            txtId.BorderStyle = BorderStyle.None;
            txtId.ForeColor = Color.White;
            txtId.Location = new Point(44, 107);
            txtId.Margin = new Padding(3, 4, 3, 4);
            txtId.Name = "txtId";
            txtId.Size = new Size(54, 20);
            txtId.TabIndex = 14;
            txtId.Visible = false;
            // 
            // panel1
            // 
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = Color.LightCyan;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(formTitle);
            panel1.Controls.Add(saveBtn);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1145, 97);
            panel1.TabIndex = 13;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Teal;
            button1.Location = new Point(821, 25);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(143, 49);
            button1.TabIndex = 13;
            button1.Text = "Nueva";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1159, 743);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Gestion de Tareas";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            Tareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv_tasks).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            TareasForm.ResumeLayout(false);
            TareasForm.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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
        private Label formTitle;
        private TabControl tabControl1;
        private TabPage Tareas;
        private TabPage TareasForm;
        private Panel panel1;
        private Panel panel2;
        private Label label6;
        private DataGridView dgv_tasks;
        private Button button1;
        private TextBox txtId;
        private Button button2;
    }
}
