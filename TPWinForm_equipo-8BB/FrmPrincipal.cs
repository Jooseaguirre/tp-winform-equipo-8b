using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPWinForm_equipo_8BB
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenido a la aplicacion");  
            cboTipo.Items.Add("Nombre");
            cboTipo.Items.Add("Codigo");
            cboTipo.Items.Add("Marca");
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Gracias por usar la aplicacion");  
        }
    }
}
