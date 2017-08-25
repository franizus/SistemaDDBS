using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaUPS.Estudiante
{
    public partial class Estudiante : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;
        private string id;

        public Estudiante(string tipo, string id)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            this.CenterToScreen();
            MaximizeBox = false;
            MinimizeBox = false;
            this.tipo = tipo;
            this.id = id;
            initGridView();
            fillComboCarrera();
            initComponents();
        }

        private void fillComboCarrera()
        {
            comboCarrera.Items.Clear();

            conexionSql.Conectar();
            string query = "select NOMBRE from CARRERA_GIRON";
            SqlCommand sqlCmd = new SqlCommand(query, conexionSql.getConnection());
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                comboCarrera.Items.Add(sqlReader["NOMBRE"].ToString());
            }

            sqlReader.Close();
            conexionSql.Desconectar();
        }

        private void initComponents()
        {
            dateSalida.Enabled = false;
            comboCarrera.SelectedIndex = 0;

            if (tipo.Equals("Modificar"))
            {
                btnGuardar.Text = "Modificar";
                btnGuardar.Width = 85;
                txtID.Enabled = false;
                txtID.Text = id;
                fillForm();
            }
            else if (tipo.Equals("Consultar") || tipo.Equals("Eliminar"))
            {
                btnGuardar.Visible = false;
                btnLimpiar.Visible = false;
                txtNombre.Enabled = false;
                txtID.Enabled = false;

                txtApellido.Enabled = false;
                txtCedula.Enabled = false;
                txtEmail.Enabled = false;
                dateInicio.Enabled = false;
                tableLayoutPanel4.ColumnStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel4.ColumnStyles[1].Width = 0;

                comboCarrera.Enabled = false;

                txtID.Text = id;
                fillForm();

                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[1].Height = 0;

                if (tipo.Equals("Eliminar"))
                {
                    btnLimpiar.Visible = true;
                    btnLimpiar.Text = "Eliminar";
                    btnLimpiar.Width = 85;
                }
            }
        }

        private void fillForm()
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select * from ESTUDIANTE_GIRON where ID_ESTUDIANTE = '" + txtID.Text + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboCarrera.SelectedIndex = reader.GetInt32(1) - 1;
                txtNombre.Text = reader.GetString(2);
                txtApellido.Text = reader.GetString(3);
                txtEmail.Text = reader.GetString(4);
                txtCedula.Text = reader.GetString(5);
                dateInicio.Value = reader.GetDateTime(6);
                if (!reader.IsDBNull(7))
                {
                    dateSalida.Value = reader.GetDateTime(7);
                    checkSalida.Checked = true;
                }
                else
                {
                    dateSalida.Value = DateTime.Today;
                    checkSalida.Checked = false;
                }
            }
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from estudianteGiron";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewEstudiante.ReadOnly = true;
            gridViewEstudiante.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewEstudiante.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewEstudiante.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarRegistro())
            {
                if (tipo.Equals("Modificar"))
                {
                    guardarEstudiante("actualizarESTUDIANTE");
                    MessageBox.Show("Estudiante modificado con éxito.", "Modificar Estudiante", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnGuardar.Enabled = false;
                    initGridView();
                }
                else
                {
                    conexionSql.Conectar();
                    SqlCommand cmd = new SqlCommand("select * from V_ESTUDIANTE where ID_ESTUDIANTE = '" + txtID.Text + "'", conexionSql.getConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        guardarEstudiante("registrarESTUDIANTE");
                        conexionSql.Desconectar();
                        MessageBox.Show("Estudiante registrado con éxito.", "Registrar Estudiante", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        btnGuardar.Enabled = false;
                        initGridView();
                    }
                    else
                    {
                        MessageBox.Show("Estudiante ya se encuentra registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnLimpiar.PerformClick();
                        conexionSql.Desconectar();
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (tipo.Equals("Eliminar"))
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conexionSql.Conectar();
                    SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_ESTUDIANTE where ID_ESTUDIANTE = '" + txtID.Text + "' commit", conexionSql.getConnection());
                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    conexionSql.Desconectar();
                }
            }
            else
            {
                if (tipo.Equals("registro"))
                {
                    txtID.Clear();
                }
                txtNombre.Clear();
                txtApellido.Clear();
                txtCedula.Clear();
                txtEmail.Clear();
                comboCarrera.SelectedIndex = 0;
                dateSalida.Value = DateTime.Today;
                checkSalida.Checked = false;
                dateInicio.Value = DateTime.Today;
            }
        }

        private bool validarRegistro()
        {
            bool temp = true;

            var err = "Campo Vacio :\n";
            if (string.IsNullOrWhiteSpace(txtID.Text))
                err += "-->ID Estudiante\n";
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                err += "-->Nombre\n";
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                err += "-->E-Mail\n";
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
                err += "-->Apellido\n";
            if (string.IsNullOrWhiteSpace(txtCedula.Text))
                err += "-->Cedula\n";

            if (!err.Equals("Campo Vacio :\n"))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temp = false;
            }

            return temp;
        }

        private void guardarEstudiante(string procedure)
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand(procedure, conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_ESTUDIANTE", SqlDbType.Int).Value = Int32.Parse(txtID.Text);
            cmd.Parameters.Add("@ID_CARRERA", SqlDbType.Int).Value = comboCarrera.SelectedIndex + 1;
            cmd.Parameters.Add("@NOMBRE_EST", SqlDbType.VarChar).Value = txtNombre.Text;
            cmd.Parameters.Add("@APELLIDO_EST", SqlDbType.VarChar).Value = txtApellido.Text;
            cmd.Parameters.Add("@EMAIL_EST", SqlDbType.VarChar).Value = txtEmail.Text;
            cmd.Parameters.Add("@CI_EST", SqlDbType.VarChar).Value = txtCedula.Text;
            cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = dateInicio.Value;
            if (checkSalida.Checked)
            {
                cmd.Parameters.Add("@FECHA_SALIDA", SqlDbType.Date).Value = dateSalida.Value;
            }
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkSalida_CheckedChanged(object sender, EventArgs e)
        {
            if (checkSalida.Checked)
            {
                dateSalida.Enabled = true;
            }
            else
            {
                dateSalida.Value = DateTime.Today;
                dateSalida.Enabled = false;
            }
        }
    }
}
