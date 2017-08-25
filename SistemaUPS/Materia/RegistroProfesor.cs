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
    public partial class RegistroProfesor : Form
    {
        private ConexionSQL conexionSql;
        private string id;

        public RegistroProfesor(string id)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            this.CenterToScreen();
            MaximizeBox = false;
            MinimizeBox = false;
            this.id = id;
            actualizarComponentes();
        }

        private void actualizarComponentes()
        {
            initGridView();
            fillComboProfesor();
            comboProfesor.SelectedIndex = 0;
        }

        private void fillComboProfesor()
        {
            comboProfesor.Items.Clear();
            conexionSql.Conectar();
            string query = "select [Nombre Profesor] from profesorNombreGiron where not [Nombre Profesor] in (Select [Nombre Profesor] from profesorMateriaGiron where NOMBRE = '" + id + "') order by [Nombre Profesor]";
            SqlCommand sqlCmd = new SqlCommand(query, conexionSql.getConnection());
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                comboProfesor.Items.Add(sqlReader["Nombre Profesor"].ToString());
            }

            sqlReader.Close();
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from profesorMateriaGiron where NOMBRE = '" + id + "'";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewProfesor.ReadOnly = true;
            gridViewProfesor.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewProfesor.Columns[1].Visible = false;
            gridViewProfesor.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private int getIDProfesor(string nombre, string apellido)
        {
            int id = 0;

            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select ID_PROF from PROFESOR_DATOS_GIRON where NOMBRE_PROF = '" + nombre + "' AND APELLIDO_PROF = '" + apellido + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            conexionSql.Desconectar();

            return id;
        }

        private int getIDMateria(string materia)
        {
            int id = 0;

            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select ID_MATERIA from MATERIA_GIRON where NOMBRE = '" + materia + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            conexionSql.Desconectar();

            return id;
        }

        private void guardarProfesorMateria()
        {
            int materia = getIDMateria(id);
            string[] prof = comboProfesor.Text.Split(null);
            int profesor = getIDProfesor(prof[0], prof[1]);
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("registrarPROFESOR_MATERIA", conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_MATERIA", SqlDbType.Int).Value = materia;
            cmd.Parameters.Add("@ID_PROF", SqlDbType.Int).Value = profesor;
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardarProfesorMateria();
            actualizarComponentes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int materia = getIDMateria(id);
                string[] prof = comboProfesor.Text.Split(null);
                int profesor = getIDProfesor(prof[0], prof[1]);
                conexionSql.Conectar();
                SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_PROFESOR_MATERIA where ID_MATERIA = '" + materia + "' AND ID_PROF = '" + profesor + "'  commit", conexionSql.getConnection());
                try
                {
                    comando.ExecuteNonQuery();
                    actualizarComponentes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conexionSql.Desconectar();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
