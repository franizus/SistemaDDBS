using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaUPS.Carrera;

namespace SistemaUPS
{
    public partial class PaginaPrincipal : Form
    {
        public PaginaPrincipal()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void registrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Carrera.Carrera carrera = new Carrera.Carrera("registro", "");
            carrera.Text = "Registrar Carrera";
            carrera.Show();
        }

        private void consultarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaCarrera bc = new BusquedaCarrera("Consultar");
            bc.Show();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaCarrera bc = new BusquedaCarrera("Modificar");
            bc.Show();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaCarrera bc = new BusquedaCarrera("Eliminar");
            bc.Show();
        }

        private void registrarMateriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaCarrera bc = new BusquedaCarrera("materia");
            bc.Show();
        }

        private void registrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void consultarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void modificarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void registrarEstudianteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void registrarProfesorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void registrarToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void consultarToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void modificarToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void registrarToolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void consultarToolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void modificarToolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }
    }
}
