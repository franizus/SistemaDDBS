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

namespace SistemaUPS.Carrera
{
    public partial class Carrera : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;
        private string id;

        public Carrera(string tipo, string id)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            this.CenterToScreen();
            MaximizeBox = false;
            MinimizeBox = false;
            this.tipo = tipo;
            this.id = id;
            initGridView();
            initComponents();
        }

        private void initComponents()
        {
            if (tipo.Equals("Modificar"))
            {
                btnGuardar.Text = "Modificar";
                btnGuardar.Width = 85;
                txtID.Enabled = false;
                txtID.Text = id;
                fillForm();
            }
            else if(tipo.Equals("Consultar") || tipo.Equals("Eliminar"))
            {
                btnGuardar.Visible = false;
                btnLimpiar.Visible = false;
                txtNombre.Enabled = false;
                txtID.Enabled = false;
                numericSemestres.Enabled = false;
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
            SqlCommand cmd = new SqlCommand("select * from CARRERA_GIRON where ID_CARRERA = '" + txtID.Text + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                txtNombre.Text = reader.GetString(1);
                numericSemestres.Value = reader.GetInt32(2);
            }
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from CarreraGiron";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewCarrera.ReadOnly = true;
            gridViewCarrera.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewCarrera.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewCarrera.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private bool validarRegistro()
        {
            bool temp = true;

            var err = "Campo Vacio :\n";
            if (string.IsNullOrWhiteSpace(txtID.Text))
                err += "-->ID Carrera\n";
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                err += "-->Nombre Carrera\n";

            if (!err.Equals("Campo Vacio :\n"))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temp = false;
            }

            return temp;
        }

        private void guardarCarrera(string procedure)
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand(procedure, conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_CARRERA", SqlDbType.Int).Value = Int32.Parse(txtID.Text);
            cmd.Parameters.Add("@NOMBRE", SqlDbType.Char).Value = txtNombre.Text;
            cmd.Parameters.Add("@N_SEMESTRES", SqlDbType.Int).Value = Decimal.ToInt32(numericSemestres.Value);
            cmd.Parameters.Add("@CAMPUS", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarRegistro())
            {
                if (tipo.Equals("Modificar"))
                {
                    guardarCarrera("actualizarCARRERA");
                    MessageBox.Show("Carrera modificada con éxito.", "Modificar Carrera", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnGuardar.Enabled = false;
                    initGridView();
                }
                else
                {
                    conexionSql.Conectar();
                    SqlCommand cmd = new SqlCommand("select * from V_CARRERA where ID_CARRERA = '" + txtID.Text + "'", conexionSql.getConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        guardarCarrera("registrarCARRERA");
                        conexionSql.Desconectar();
                        MessageBox.Show("Carrera registrada con éxito.", "Registrar Carrera", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        btnGuardar.Enabled = false;
                        initGridView();
                    }
                    else
                    {
                        MessageBox.Show("Carrera ya se encuentra registrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnLimpiar.PerformClick();
                        conexionSql.Desconectar();
                    }
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (tipo.Equals("Eliminar"))
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conexionSql.Conectar();
                    SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_CARRERA where ID_CARRERA = '" + txtID.Text + "' commit", conexionSql.getConnection());
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
                numericSemestres.Value = 1;
            }
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
