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

namespace SistemaUPS.Profesor
{
    public partial class BusquedaProfesor : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;

        public BusquedaProfesor(string tipo)
        {
            InitializeComponent();
            conexionSql = new ConexionSQL();
            MaximizeBox = false;
            MinimizeBox = false;
            this.tipo = tipo;
            if (tipo.Equals("Consultar Nomina") || tipo.Equals("Modificar Nomina"))
            {
                initGridView("profesorNomina");
                comboBusqueda.Items.Clear();
                comboBusqueda.Items.Add("ID");
                comboBusqueda.Items.Add("Nombre");
                comboBusqueda.Items.Add("Apellido");
                comboBusqueda.Items.Add("Salario");
            }
            else
            {
                initGridView("profesorDatosGiron");
            }
            comboBusqueda.SelectedIndex = 0;
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

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (gridViewProfesor.SelectedRows.Count >= 1)
            {
                string id = gridViewProfesor.SelectedRows[0].Cells[0].Value.ToString();
                Profesor form = new Profesor(tipo, id);
                form.Text = tipo + " Profesor";
                form.Show();
                btnSalir.PerformClick();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado un profesor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBusqueda.Clear();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridViewProfesor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar.PerformClick();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)gridViewProfesor.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format(gridViewProfesor.Columns[comboBusqueda.SelectedIndex].DataPropertyName + " like '%{0}%'", txtBusqueda.Text.Trim().Replace("'", "''"));
            gridViewProfesor.Refresh();
        }
    }
}
