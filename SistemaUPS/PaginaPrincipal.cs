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
using SistemaUPS.Estudiante;
using SistemaUPS.Materia;
using SistemaUPS.Profesor;

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
            Materia.Materia ma = new Materia.Materia("registro", "");
            ma.Show();
        }

        private void consultarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BusquedaMateria bm = new BusquedaMateria("Consultar");
            bm.Show();
        }

        private void modificarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BusquedaMateria bm = new BusquedaMateria("Modificar");
            bm.Show();
        }

        private void eliminarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BusquedaMateria bm = new BusquedaMateria("Eliminar");
            bm.Show();
        }

        private void registrarEstudianteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaMateria bm = new BusquedaMateria("estudiante");
            bm.Show();
        }

        private void registrarProfesorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusquedaMateria bm = new BusquedaMateria("profesor");
            bm.Show();
        }

        private void registrarToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Profesor.Profesor p = new Profesor.Profesor("registro", "");
            p.Show();
        }

        private void consultarToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BusquedaProfesor bp = new BusquedaProfesor("Consultar");
            bp.Show();
        }

        private void modificarToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BusquedaProfesor bp = new BusquedaProfesor("Modificar");
            bp.Show();
        }

        private void eliminarToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BusquedaProfesor bp = new BusquedaProfesor("Eliminar");
            bp.Show();
        }

        private void registrarToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Estudiante.Estudiante es = new Estudiante.Estudiante("registro", "");
            es.Show();
        }

        private void consultarToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            BusquedaEstudiante be = new BusquedaEstudiante("Consultar");
            be.Show();
        }

        private void modificarToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            BusquedaEstudiante be = new BusquedaEstudiante("Modificar");
            be.Show();
        }

        private void eliminarToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            BusquedaEstudiante be = new BusquedaEstudiante("Eliminar");
            be.Show();
        }

        private void consultarToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            BusquedaProfesor bp = new BusquedaProfesor("Consultar Nomina");
            bp.Show();
        }

        private void modificarToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            BusquedaProfesor bp = new BusquedaProfesor("Modificar Nomina");
            bp.Show();
        }
    }
}
