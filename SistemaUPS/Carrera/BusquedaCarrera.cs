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
    public partial class BusquedaCarrera : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;

        public BusquedaCarrera(string tipo)
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

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (gridViewCarrera.SelectedRows.Count >= 1)
            {
                string id;
                if (tipo.Equals("materia"))
                {
                    id = gridViewCarrera.SelectedRows[0].Cells[1].Value.ToString();
                    RegistroMateria rm = new RegistroMateria(id);
                    rm.Text = "Agregar en " + id;
                    rm.Show();
                }
                else
                {
                    id = gridViewCarrera.SelectedRows[0].Cells[0].Value.ToString();
                    Carrera form = new Carrera(tipo, id);
                    form.Text = tipo + " Carrera";
                    form.Show();
                }
                btnSalir.PerformClick();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado una carrera.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBusqueda.Clear();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridViewCarrera_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar.PerformClick();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)gridViewCarrera.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format(gridViewCarrera.Columns[comboBusqueda.SelectedIndex].DataPropertyName + " like '%{0}%'", txtBusqueda.Text.Trim().Replace("'", "''"));
            gridViewCarrera.Refresh();
        }
    }
}
