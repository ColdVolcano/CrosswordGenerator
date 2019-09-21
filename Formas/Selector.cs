using CrosswordGenerator.Controles;
using System.Windows.Forms;
using System.Drawing;

namespace CrosswordGenerator.Formas
{
    public partial class Selector : Form
    {
        public Selector()
        {
            InitializeComponent();
            Controls.Add(new BotonDificultad(Dificultad.Facil)
            {
                Location = new Point(37, 115)
            });

            Controls.Add(new BotonDificultad(Dificultad.Medio)
            {
                Location = new Point(172, 115)
            });

            Controls.Add(new BotonDificultad(Dificultad.Dificil)
            {
                Location = new Point(307, 115)
            });
        }
    }
}
