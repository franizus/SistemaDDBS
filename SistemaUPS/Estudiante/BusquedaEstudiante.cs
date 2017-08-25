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
using SistemaUPS.Carrera;

namespace SistemaUPS.Estudiante
{
    public partial class BusquedaEstudiante : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;

        public BusquedaEstudiante(string tipo)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            MaximizeBox = false;
            MinimizeBox = false;
            this.tipo = tipo;
            initGridView();
            comboBusqueda.SelectedIndex = 0;
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

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (gridViewEstudiante.SelectedRows.Count >= 1)
            {
                string id = gridViewEstudiante.SelectedRows[0].Cells[0].Value.ToString();
                Estudiante form = new Estudiante(tipo, id);
                form.Text = tipo + " Estudiante";
                form.Show();
                btnSalir.PerformClick();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado un estudiante.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBusqueda.Clear();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridViewEstudiante_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar.PerformClick();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)gridViewEstudiante.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format(gridViewEstudiante.Columns[comboBusqueda.SelectedIndex].DataPropertyName + " like '%{0}%'", txtBusqueda.Text.Trim().Replace("'", "''"));
            gridViewEstudiante.Refresh();
        }
    }
}
