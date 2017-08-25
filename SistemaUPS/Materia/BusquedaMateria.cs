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
    public partial class BusquedaMateria : Form
    {
        private ConexionSQL conexionSql;
        private string tipo;

        public BusquedaMateria(string tipo)
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

        private void gridViewCarrera_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnConsultar.PerformClick();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            var bd = (BindingSource)gridViewMateria.DataSource;
            var dt = (DataTable)bd.DataSource;
            dt.DefaultView.RowFilter = string.Format(gridViewMateria.Columns[comboBusqueda.SelectedIndex].DataPropertyName + " like '%{0}%'", txtBusqueda.Text.Trim().Replace("'", "''"));
            gridViewMateria.Refresh();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (gridViewMateria.SelectedRows.Count >= 1)
            {
                string id;
                if (tipo.Equals("estudiante") || tipo.Equals("profesor"))
                {
                    id = gridViewMateria.SelectedRows[0].Cells[1].Value.ToString();
                    if (tipo.Equals("estudiante"))
                    {
                        RegistroEstudiante re = new RegistroEstudiante(id);
                        re.Text = "Agregar en " + id;
                        re.Show();
                    }
                    else
                    {
                        RegistroProfesor rp = new RegistroProfesor(id);
                        rp.Text = "Agregar en " + id;
                        rp.Show();
                    }
                }
                else
                {
                    id = gridViewMateria.SelectedRows[0].Cells[0].Value.ToString();
                    Materia form = new Materia(tipo, id);
                    form.Text = tipo + " Materia";
                    form.Show();
                }
                btnSalir.PerformClick();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado una materia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBusqueda.Clear();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
