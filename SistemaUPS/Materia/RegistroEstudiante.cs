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
    public partial class RegistroEstudiante : Form
    {
        private ConexionSQL conexionSql;
        private string id;

        public RegistroEstudiante(string id)
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
            fillComboEstudiante();
            comboEstudiante.SelectedIndex = 0;
        }

        private string getNombreCarrera()
        {
            string id = "";

            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select [Nombre Carrera] from materiaCarreraGiron where [Nombre Materia] = '" + this.id + "'", conexionSql.getConnection());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = reader.GetString(0);
            }
            conexionSql.Desconectar();

            return id;
        }

        private void fillComboEstudiante()
        {
            comboEstudiante.Items.Clear();
            string carrera = getNombreCarrera();
            conexionSql.Conectar();
            string query = "select [Nombre Estudiante] from carreraEstudianteGiron where not [Nombre Estudiante] in (Select [Nombre Estudiante] from estudianteMateriaGiron where [Nombre Materia] = '" + id + "') AND [Nombre Carrera] = '" + carrera + "' order by [Nombre Estudiante]";
            SqlCommand sqlCmd = new SqlCommand(query, conexionSql.getConnection());
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                comboEstudiante.Items.Add(sqlReader["Nombre Estudiante"].ToString());
            }

            sqlReader.Close();
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from estudianteMateriaGiron where [Nombre Materia] = '" + id + "'";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewEstudiante.ReadOnly = true;
            gridViewEstudiante.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewEstudiante.Columns[1].Visible = false;
            gridViewEstudiante.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private int getIDEstudiante(string nombre, string apellido)
        {
            int id = 0;

            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select ID_ESTUDIANTE from ESTUDIANTE_GIRON where NOMBRE_EST = '" + nombre + "' AND APELLIDO_EST = '" + apellido + "'", conexionSql.getConnection());
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

        private void guardarEstudianteMateria()
        {
            int materia = getIDMateria(id);
            string[] est = comboEstudiante.Text.Split(null);
            int estudiante = getIDEstudiante(est[0], est[1]);
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("registrarESTUDIANTE_MATERIA", conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_MATERIA", SqlDbType.Int).Value = materia;
            cmd.Parameters.Add("@ID_ESTUDIANTE", SqlDbType.Int).Value = estudiante;
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardarEstudianteMateria();
            actualizarComponentes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int materia = getIDMateria(id);
                string[] est = comboEstudiante.Text.Split(null);
                int estudiante = getIDEstudiante(est[0], est[1]);
                conexionSql.Conectar();
                SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_ESTUDIANTE_MATERIA where ID_MATERIA = '" + materia + "' AND ID_ESTUDIANTE = '" + estudiante + "'  commit", conexionSql.getConnection());
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
