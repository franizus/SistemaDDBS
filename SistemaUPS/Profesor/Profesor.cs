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

namespace SistemaUPS.Profesor
{
    public partial class Profesor : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;
        private string id;

        public Profesor(string tipo, string id)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            this.CenterToScreen();
            MaximizeBox = false;
            MinimizeBox = false;
            this.tipo = tipo;
            this.id = id;
            if (tipo.Equals("Consultar Nomina") || tipo.Equals("Modificar Nomina"))
            {
                initGridView("profesorNomina");
            }
            else
            {
                initGridView("profesorDatosGiron");
            }
            fillComboCategoria();
            initComponents();
        }

        private void fillComboCategoria()
        {
            comboCategoria.Items.Clear();

            conexionSql.Conectar();
            string query = "select NOM_CAT from CATEGORIA";
            SqlCommand sqlCmd = new SqlCommand(query, conexionSql.getConnection());
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                comboCategoria.Items.Add(sqlReader["NOM_CAT"].ToString());
            }

            sqlReader.Close();
            conexionSql.Desconectar();
        }

        private void initComponents()
        {
            dateSalida.Enabled = false;
            comboCategoria.SelectedIndex = 0;

            if (tipo.Equals("Modificar"))
            {
                tableLayoutPanel2.RowStyles[6].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[6].Height = 0;
                tableLayoutPanel2.RowStyles[7].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[7].Height = 0;
                tableLayoutPanel2.RowStyles[8].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[8].Height = 0;

                btnGuardar.Text = "Modificar";
                btnGuardar.Width = 85;
                txtID.Enabled = false;
                txtID.Text = id;
                fillForm();
            }
            else if (tipo.Equals("Consultar") || tipo.Equals("Eliminar"))
            {
                tableLayoutPanel2.RowStyles[6].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[6].Height = 0;
                tableLayoutPanel2.RowStyles[7].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[7].Height = 0;
                tableLayoutPanel2.RowStyles[8].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[8].Height = 0;

                btnGuardar.Visible = false;
                btnLimpiar.Visible = false;
                txtNombre.Enabled = false;
                txtID.Enabled = false;
                
                txtApellido.Enabled = false;
                txtCedula.Enabled = false;
                txtEmail.Enabled = false;
                
                comboCategoria.Enabled = false;

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
            else if (tipo.Equals("Consultar Nomina") || tipo.Equals("Modificar Nomina"))
            {
                tableLayoutPanel2.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[1].Height = 0;
                tableLayoutPanel2.RowStyles[2].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[2].Height = 0;
                tableLayoutPanel2.RowStyles[5].SizeType = SizeType.Percent;
                tableLayoutPanel2.RowStyles[5].Height = 0;

                txtCedula.Visible = false;
                txtEmail.Visible = false;
                comboCategoria.Visible = false;
                txtNombre.Enabled = false;
                txtApellido.Enabled = false;

                if (tipo.Equals("Consultar Nomina"))
                {
                    btnGuardar.Visible = false;
                    btnLimpiar.Visible = false;
                    
                    txtID.Enabled = false;
                    numericSalario.Enabled = false;
                    dateInicio.Enabled = false;
                    checkSalida.Enabled = false;
                    dateSalida.Enabled = false;

                    txtID.Text = id;
                    fillForm();

                    tableLayoutPanel4.ColumnStyles[1].SizeType = SizeType.Percent;
                    tableLayoutPanel4.ColumnStyles[1].Width = 0;
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                    tableLayoutPanel1.RowStyles[1].Height = 0;
                }
                else
                {
                    btnGuardar.Text = "Modificar";
                    btnGuardar.Width = 85;
                    txtID.Enabled = false;
                    txtID.Text = id;
                    fillForm();
                }
            }
        }

        private void fillForm()
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select * from profesorGiron where ID_PROF = '" + txtID.Text + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboCategoria.SelectedIndex = reader.GetInt32(1) - 1;
                txtNombre.Text = reader.GetString(2);
                txtApellido.Text = reader.GetString(3);
                txtEmail.Text = reader.GetString(4);
                txtCedula.Text = reader.GetString(5);
                numericSalario.Value = reader.GetDecimal(6);
                dateInicio.Value = reader.GetDateTime(7);
                if (!reader.IsDBNull(8))
                {
                    dateSalida.Value = reader.GetDateTime(8);
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

        private void initGridView(string view)
        {
            conexionSql.Conectar();
            string query = "select * from " + view;
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewProfesor.ReadOnly = true;
            gridViewProfesor.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewProfesor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewProfesor.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (tipo.Equals("Eliminar"))
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conexionSql.Conectar();
                    SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_PROFESOR where ID_PROF = '" + txtID.Text + "' commit", conexionSql.getConnection());
                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    comando = new SqlCommand("delete from PROFESOR_NOMINA where ID_PRO = '" + txtID.Text + "'", conexionSql.getConnection());
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
                if (tipo.Equals("registro") || tipo.Equals("Modificar"))
                {
                    if (tipo.Equals("registro"))
                    {
                        txtID.Clear();
                    }
                    txtNombre.Clear();
                    txtApellido.Clear();
                    txtCedula.Clear();
                    txtEmail.Clear();
                    comboCategoria.SelectedIndex = 0;
                }
                else if (tipo.Equals("Modificar Nomina"))
                {
                    numericSalario.Value = 0;
                    dateSalida.Value = DateTime.Today;
                    checkSalida.Checked = false;
                    dateInicio.Value = DateTime.Today;
                }
                
            }
        }

        private bool validarRegistro()
        {
            bool temp = true;

            var err = "Campo Vacio :\n";
            if (string.IsNullOrWhiteSpace(txtID.Text))
                err += "-->ID Profesor\n";
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

        private void guardarProfesor(string procedure)
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand(procedure, conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_PROF", SqlDbType.Int).Value = Int32.Parse(txtID.Text);
            cmd.Parameters.Add("@ID_CATEGORIA", SqlDbType.Int).Value = comboCategoria.SelectedIndex + 1;
            cmd.Parameters.Add("@NOMBRE_PROF", SqlDbType.VarChar).Value = txtNombre.Text;
            cmd.Parameters.Add("@APELLIDO_PROF", SqlDbType.VarChar).Value = txtApellido.Text;
            cmd.Parameters.Add("@EMAIL_PROF", SqlDbType.VarChar).Value = txtEmail.Text;
            cmd.Parameters.Add("@CI_PROF", SqlDbType.VarChar).Value = txtCedula.Text;
            cmd.Parameters.Add("@SALARIO", SqlDbType.Money).Value = numericSalario.Value;
            cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.Date).Value = dateInicio.Value;
            if (checkSalida.Checked)
            {
                cmd.Parameters.Add("@FECHA_SALIDA", SqlDbType.Date).Value = dateSalida.Value;
            }
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarRegistro())
            {
                if (tipo.Equals("Modificar") || tipo.Equals("Modificar Nomina"))
                {
                    guardarProfesor("actualizarPROFESOR");
                    MessageBox.Show("Profesor modificado con éxito.", "Modificar Profesor", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnGuardar.Enabled = false;
                    if (tipo.Equals("Modificar"))
                    {
                        initGridView("profesorDatosGiron");
                    }
                    else
                    {
                        initGridView("profesorNomina");
                    }
                }
                else
                {
                    conexionSql.Conectar();
                    SqlCommand cmd = new SqlCommand("select * from V_PROFESOR_DATOS where ID_PROF = '" + txtID.Text + "'", conexionSql.getConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        guardarProfesor("registrarPROFESOR");
                        conexionSql.Desconectar();
                        MessageBox.Show("Profesor registrado con éxito.", "Registrar Profesor", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        btnGuardar.Enabled = false;
                        initGridView("profesorDatosGiron");
                    }
                    else
                    {
                        MessageBox.Show("Profesor ya se encuentra registrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnLimpiar.PerformClick();
                        conexionSql.Desconectar();
                    }
                }
            }
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
