using MySql.Data.MySqlClient;
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
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        public void clearFields()
        {
            this.cbTaskStatuses.SelectedIndex = 0;
            this.txtTitle.Text = "";
            this.txtDescription.Text = "";
            this.dtpDesde.Value = DateTime.Now;
            this.dtpHasta.Value = DateTime.Now;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        

            if (String.IsNullOrEmpty(title))
            {
                MessageBox.Show("El campo titulo es requerido");
                return;
            }

            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO tasks(title,description,status,date_from,due_date) VALUES(?title,?description,?status,?date_from,?due_date);";
                cmd.Parameters.Add("?title", MySqlDbType.VarChar).Value = title;
                cmd.Parameters.Add("?description", MySqlDbType.VarChar).Value = description;
                cmd.Parameters.Add("?status", MySqlDbType.Int64).Value = taskStatusSelectedID;
                cmd.Parameters.Add("?date_from", MySqlDbType.DateTime).Value = fechaDesde;
                cmd.Parameters.Add("?due_date", MySqlDbType.DateTime).Value = fechaHasta;
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Tarea creada correctamente");

                this.clearFields();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error in adding mysql row. Error: " + ex.Message);
            } 

        }
    }
}
