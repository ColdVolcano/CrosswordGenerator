using System.Windows.Forms;

namespace CrosswordGenerator.Controles
{
    public partial class BotonLetra : UserControl
    {
        public BotonLetra(LetraEsperada letra)
        {
            InitializeComponent();
            label1.Text = "";
            Boton.Text = $"{letra.Letra}";
            foreach (int num in letra.LetraInicial)
                label1.Text += $"{num}\n";
        }
    }
}
