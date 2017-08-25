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

namespace SistemaUPS.Materia
{
    public partial class Materia : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;
        private string id;

        public Materia(string tipo, string id)
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
            else if (tipo.Equals("Consultar") || tipo.Equals("Eliminar"))
            {
                btnGuardar.Visible = false;
                btnLimpiar.Visible = false;
                txtNombre.Enabled = false;
                txtID.Enabled = false;
                numericCreditos.Enabled = false;
                numericSemestreRef.Enabled = false;

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
            SqlCommand cmd = new SqlCommand("select * from MATERIA_GIRON where ID_MATERIA = '" + txtID.Text + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                txtNombre.Text = reader.GetString(1);
                numericSemestreRef.Value = reader.GetInt32(2);
                numericCreditos.Value = reader.GetInt32(3);
            }
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from materiaGiron";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewMateria.ReadOnly = true;
            gridViewMateria.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewMateria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewMateria.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private bool validarRegistro()
        {
            bool temp = true;

            var err = "Campo Vacio :\n";
            if (string.IsNullOrWhiteSpace(txtID.Text))
                err += "-->ID Materia\n";
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                err += "-->Nombre Materia\n";

            if (!err.Equals("Campo Vacio :\n"))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temp = false;
            }

            return temp;
        }

        private void guardarMateria(string procedure)
        {
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand(procedure, conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_MATERIA", SqlDbType.Int).Value = Int32.Parse(txtID.Text);
            cmd.Parameters.Add("@NOMBRE", SqlDbType.Char).Value = txtNombre.Text;
            cmd.Parameters.Add("@SEM_REF", SqlDbType.Int).Value = Decimal.ToInt32(numericSemestreRef.Value);
            cmd.Parameters.Add("@N_CREDITOS", SqlDbType.Int).Value = Decimal.ToInt32(numericCreditos.Value);
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
                    guardarMateria("actualizarMATERIA");
                    MessageBox.Show("Materia modificada con éxito.", "Modificar Materia", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnGuardar.Enabled = false;
                    initGridView();
                }
                else
                {
                    conexionSql.Conectar();
                    SqlCommand cmd = new SqlCommand("select * from V_MATERIA where ID_MATERIA = '" + txtID.Text + "'", conexionSql.getConnection());
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        guardarMateria("registrarMATERIA");
                        conexionSql.Desconectar();
                        MessageBox.Show("Materia registrada con éxito.", "Registrar Materia", MessageBoxButtons.OK,
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (tipo.Equals("Eliminar"))
            {
                if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    conexionSql.Conectar();
                    SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_MATERIA where ID_MATERIA = '" + txtID.Text + "' commit", conexionSql.getConnection());
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
                numericSemestreRef.Value = 1;
                numericCreditos.Value = 1;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
