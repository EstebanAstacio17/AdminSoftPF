using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminSoftPF
{
    public partial class ComentariosUsuarios : Form
    {
        public ComentariosUsuarios()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Mostrar cuadro de diálogo de confirmación
            DialogResult result = MessageBox.Show(
                "¿Estás seguro de que deseas Salir?",
                "Confirmar cierre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Verificar la respuesta del usuario
            if (result == DialogResult.Yes)
            {
                Close(); // Cerrar el formulario
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar el contenido del RichTextBox
            rtbComentario.Clear();
        }
    }
}
