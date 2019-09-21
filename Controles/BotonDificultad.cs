using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosswordGenerator.Controles
{
    public partial class BotonDificultad : UserControl
    {
        public BotonDificultad(Dificultad diff)
        {
            InitializeComponent();
            boton.Size = Size = new Size(110, 110);
            boton.Text = diff.ToString();
            switch (diff)
            {
                case Dificultad.Facil:
                    boton.BackColor = Color.LightSeaGreen;
                    break;
                case Dificultad.Medio:
                    boton.BackColor = Color.LightGoldenrodYellow;
                    break;
                case Dificultad.Dificil:
                    boton.BackColor = Color.OrangeRed;
                    break;
            }
        }
    }
}
