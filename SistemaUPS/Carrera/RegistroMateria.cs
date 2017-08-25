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
    public partial class RegistroMateria : Form
    {
        private ConexionSQL conexionSql;
        private string id;

        public RegistroMateria(string id)
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
            fillComboMateria();
            comboMateria.SelectedIndex = 0;
        }

        private int getIDCarrera(string carrera)
        {
            int id = 0;

            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("select ID_CARRERA from CARRERA_GIRON where NOMBRE = '" + carrera + "'", conexionSql.getConnection());
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

        private void guardarMateriaCarrera()
        {
            int carrera = getIDCarrera(id);
            int materia = getIDMateria(comboMateria.Text);
            conexionSql.Conectar();
            SqlCommand cmd = new SqlCommand("registrarMATERIA_CARRERA", conexionSql.getConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_MATERIA", SqlDbType.Int).Value = materia;
            cmd.Parameters.Add("@ID_CARRERA", SqlDbType.Int).Value = carrera;
            cmd.Parameters.Add("@NODO", SqlDbType.Int).Value = 1;

            cmd.ExecuteNonQuery();
            conexionSql.Desconectar();
        }

        private void fillComboMateria()
        {
            comboMateria.Items.Clear();

            conexionSql.Conectar();
            string query = "select [Nombre Materia] from materiaCarreraGiron where not [Nombre Materia] in (Select [Nombre Materia] from materiaCarreraGiron where [Nombre Carrera] = '" + id + "') order by [Nombre Materia]";
            SqlCommand sqlCmd = new SqlCommand(query, conexionSql.getConnection());
            SqlDataReader sqlReader = sqlCmd.ExecuteReader();
            while (sqlReader.Read())
            {
                comboMateria.Items.Add(sqlReader["Nombre Materia"].ToString());
            }

            sqlReader.Close();
            conexionSql.Desconectar();
        }

        private void initGridView()
        {
            conexionSql.Conectar();
            string query = "select * from materiaCarreraGiron where [Nombre Carrera] = '" + id + "'";
            var dataAdapter = new SqlDataAdapter(query, conexionSql.getConnection());
            var ds = new DataTable();
            dataAdapter.Fill(ds);
            BindingSource bsSource = new BindingSource();
            bsSource.DataSource = ds;
            gridViewMateria.ReadOnly = true;
            gridViewMateria.DataSource = bsSource;
            conexionSql.Desconectar();

            gridViewMateria.Columns[1].Visible = false;
            gridViewMateria.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardarMateriaCarrera();
            actualizarComponentes();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea eliminar?", "Eliminar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int carrera = getIDCarrera(id);
                int materia = getIDMateria(gridViewMateria.SelectedRows[0].Cells[0].Value.ToString());
                conexionSql.Conectar();
                SqlCommand comando = new SqlCommand("set xact_abort on begin distributed transaction delete from V_MATERIA_CARRERA where ID_CARRERA = '" + carrera + "' AND ID_MATERIA = '" + materia + "'  commit", conexionSql.getConnection());
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
    }
}
