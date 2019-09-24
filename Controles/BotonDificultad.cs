using System.Drawing;
using System.Windows.Forms;
using CrosswordGenerator.Formas;

namespace CrosswordGenerator.Controles
{
    public partial class BotonDificultad : UserControl
    {
        private readonly Dificultad diff;
        public BotonDificultad(Dificultad diff)
        {
            this.diff = diff;
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
            boton.Click += (_, e) => InvokeOnClick(this, e);
            Click += (_, __) => AbrirCrucigrama();

        }

        private void AbrirCrucigrama()
        {
            new Crossword(diff).ShowDialog();
        }
    }
}
