using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Font = System.Drawing.Font;

namespace task_app_desktop

{
    public partial class Form1 : Form
    {
        // Clase para encapsular el ID y el Name
        public class ComboBoxItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name; // Esto asegura que el ComboBox muestre el nombre
            }
        }
        public Form1()
        {
            InitializeComponent();
            string mysqlCon = "server=127.0.0.1; user=root; database=task_app; password=";
            MySqlConnection mySqlConnection = new MySqlConnection(mysqlCon);
            try
            {
                mySqlConnection.Open();
                //MessageBox.Show("Mysql conection success");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        public void clearFields()
        {
            this.formTitle.Text = "Agregar Tarea";
            this.txtId.Text = "";
            this.cbTaskStatuses.SelectedIndex = 0;
            this.txtTitle.Text = "";
            this.txtDescription.Text = "";
            this.dtpDesde.Value = DateTime.Now;
            this.dtpHasta.Value = DateTime.Now;

        }

        private void LoadTasks()
        {
            string mysqlCon = "server=127.0.0.1; user=root; database=task_app; password=";
            MySqlConnection conn = new MySqlConnection(mysqlCon);

            try
            {
                conn.Open();

                // Consulta ajustada
                string query = @"
        SELECT t.id,t.title, t.description,ts.name AS status_name, t.date_from, t.due_date
        FROM tasks t
        INNER JOIN task_statuses ts ON t.status = ts.id";

                MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                DataTable tasksTable = new DataTable();
                tasksTable.Load(reader);

                dgv_tasks.DataSource = tasksTable;



                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            CustomizeDataGridView();
        }

        // Método para personalizar el DataGridView
        private void CustomizeDataGridView()
        {
            // Cambiar nombres de los encabezados
            dgv_tasks.Columns["id"].HeaderText = "ID";
            dgv_tasks.Columns["title"].HeaderText = "Titulo";
            dgv_tasks.Columns["description"].HeaderText = "Descripción";
            dgv_tasks.Columns["status_name"].HeaderText = "Estado";
            dgv_tasks.Columns["date_from"].HeaderText = "Desde";
            dgv_tasks.Columns["due_date"].HeaderText = "Hasta";

            // Ajustar ancho de las columnas
            dgv_tasks.Columns["id"].Width = 50;
            dgv_tasks.Columns["description"].Width = 250;



            dgv_tasks.DefaultCellStyle.SelectionBackColor = Color.DarkOrange;

            // Personalizar los encabezados de las columnas
            dgv_tasks.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgv_tasks.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            dgv_tasks.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv_tasks.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
            dgv_tasks.EnableHeadersVisualStyles = false; // Necesario para aplicar colores personalizados

            // Personalizar bordes de encabezados
            dgv_tasks.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble; // Borde doble
            dgv_tasks.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single; // Estilo del borde de encabezados



            // Ajustar el diseño general
            dgv_tasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_tasks.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
            dgv_tasks.DefaultCellStyle.Font = new Font("Arial", 10);
            dgv_tasks.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_tasks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv_tasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            dgv_tasks.ColumnHeadersHeight = 50;
            dgv_tasks.RowTemplate.Height = 80;
            dgv_tasks.RowHeadersVisible = false;


        }




        private void Form1_Load(object sender, EventArgs e)
        {
            // Cargar las tareas al cargar el formulario
            LoadTasks();

            string mysqlCon = "server=127.0.0.1; user=root; database=task_app; password=";
            MySqlConnection conn = new MySqlConnection(mysqlCon);

            try
            {
                conn.Open();

                // Query para obtener los datos de la base de datos
                MySqlCommand mySqlCommand = new MySqlCommand("SELECT id, name FROM task_statuses", conn);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();

                // Agregar los datos al ComboBox como objetos
                while (reader.Read())
                {
                    cbTaskStatuses.Items.Add(new ComboBoxItem
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString()
                    });
                }

                reader.Close();
                conn.Close();

                // Configurar el ComboBox para mostrar el Name
                cbTaskStatuses.DisplayMember = "name";
                cbTaskStatuses.ValueMember = "id";

                // Seleccionar el primer elemento como predeterminado
                if (cbTaskStatuses.Items.Count > 0)
                {
                    cbTaskStatuses.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int taskStatusSelectedID = 1;

            // Verifica que el elemento seleccionado sea de tipo ComboBoxItem
            if (cbTaskStatuses.SelectedItem is ComboBoxItem selectedItem)
            {
                int selectedId = selectedItem.Id; // Obtén el ID
                taskStatusSelectedID = selectedId;
            }

            string mysqlCon = "server=127.0.0.1; user=root; database=task_app; password=";
            MySqlConnection conn = new MySqlConnection(mysqlCon);

            string title = txtTitle.Text.ToString();
            string description = txtDescription.Text.ToString();
            DateTime fechaDesde = dtpDesde.Value;
            DateTime fechaHasta = dtpHasta.Value;

            // Validar que el título no esté vacío
            if (String.IsNullOrEmpty(title))
            {
                MessageBox.Show("El campo titulo es requerido");
                return;
            }

            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                // Si hay un ID en txtId, es una actualización
                if (!string.IsNullOrEmpty(txtId.Text))
                {
                    int taskId = int.Parse(txtId.Text); // Obtener el ID de la tarea

                    // Actualizar la tarea
                    cmd.CommandText = @"
                UPDATE tasks
                SET title = ?title, description = ?description, status = ?status, date_from = ?date_from, due_date = ?due_date
                WHERE id = ?id;
            ";
                    cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = taskId;
                }
                else
                {
                    // Insertar nueva tarea
                    cmd.CommandText = @"
                INSERT INTO tasks(title, description, status, date_from, due_date) 
                VALUES (?title, ?description, ?status, ?date_from, ?due_date);
            ";
                }

                // Agregar los parámetros comunes
                cmd.Parameters.Add("?title", MySqlDbType.VarChar).Value = title;
                cmd.Parameters.Add("?description", MySqlDbType.VarChar).Value = description;
                cmd.Parameters.Add("?status", MySqlDbType.Int64).Value = taskStatusSelectedID;
                cmd.Parameters.Add("?date_from", MySqlDbType.DateTime).Value = fechaDesde;
                cmd.Parameters.Add("?due_date", MySqlDbType.DateTime).Value = fechaHasta;

                cmd.ExecuteNonQuery();
                conn.Close();

                // Mostrar mensaje de éxito
                if (!string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Tarea actualizada correctamente");
                }
                else
                {
                    MessageBox.Show("Tarea creada correctamente");
                    this.clearFields();
                    this.button1.Visible = false;
                }

                // Recargar las tareas y limpiar los campos
                this.LoadTasks();
                this.clearFields();

                // Cambiar al tab de tareas
                this.tabControl1.SelectedIndex = 0;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error en la base de datos. Error: " + ex.Message);
            }
        }


        private void dgv_tasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verificar que no sea el encabezado
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dgv_tasks.Rows[e.RowIndex];

                // Llenar los campos con los valores seleccionados
                string estadoNombre = selectedRow.Cells["status_name"].Value.ToString();

                // Buscar el objeto ComboBoxItem cuyo Name coincida con el valor de estadoNombre
                foreach (ComboBoxItem item in cbTaskStatuses.Items)
                {
                    if (item.Name == estadoNombre)
                    {
                        cbTaskStatuses.SelectedItem = item;
                        break; // Encontrado, salir del bucle
                    }
                }
                this.txtId.Text = selectedRow.Cells["id"].Value.ToString();
                this.txtTitle.Text = selectedRow.Cells["title"].Value.ToString();
                this.txtDescription.Text = selectedRow.Cells["description"].Value.ToString();

                // Convertir los valores de las fechas y asignarlos
                this.dtpDesde.Value = Convert.ToDateTime(selectedRow.Cells["date_from"].Value);
                this.dtpHasta.Value = Convert.ToDateTime(selectedRow.Cells["due_date"].Value);

                // Cambiar al TabPage deseado (TabIndex 1)
                this.tabControl1.SelectedIndex = 1;
                this.button1.Visible = true;

                this.formTitle.Text = "Tarea #" + this.txtId.Text;

            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            this.LoadTasks();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            this.clearFields();
            this.button1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Definir la carpeta donde se guardará el archivo PDF
            string directoryPath = Path.Combine(Application.StartupPath, "ExportedPDFs");

            // Verificar si la carpeta existe, si no, crearla
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Definir el nombre del archivo PDF
            //string fileName = "Tareas_Exportadas.pdf"; // O cualquier nombre que desees
            string fileName = "Tareas_Exportadas_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

            string filePath = Path.Combine(directoryPath, fileName);

            // Crear el documento PDF
            Document doc = new Document(PageSize.A4);
            try
            {
                // Crear un PdfWriter para el archivo PDF
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                // Abrir el documento
                doc.Open();

                // Crear la tabla con el número de columnas igual al número de columnas del DataGridView
                PdfPTable pdfTable = new PdfPTable(dgv_tasks.Columns.Count);

                // Agregar los encabezados de columna al PDF
                foreach (DataGridViewColumn column in dgv_tasks.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY; // Color de fondo de los encabezados
                    pdfTable.AddCell(cell);
                }

                // Agregar las filas de datos
                foreach (DataGridViewRow row in dgv_tasks.Rows)
                {
                    // Solo agregar filas que no estén vacías
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            pdfTable.AddCell(cell.Value?.ToString() ?? ""); // Agregar el valor de la celda, si está vacío agregamos un string vacío
                        }
                    }
                }

                // Agregar la tabla al documento PDF
                doc.Add(pdfTable);

                // Cerrar el documento
                doc.Close();

                // Notificar al usuario
                MessageBox.Show("Archivo PDF exportado exitosamente en: " + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar a PDF: " + ex.Message);
            }
        }
    }
}
