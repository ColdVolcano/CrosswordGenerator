using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace CrosswordGenerator.Controles
{
    public partial class LetraCrucigrama : UserControl
    {
        public readonly LetraEsperada Letra;

        private bool palabraCorrecta;

        public bool PalabraCorrecta 
        { 
            set
            {
                palabraCorrecta = value;
                Display.BackColor = Focused ? Color.LightCyan : value ? Color.LawnGreen : Color.Transparent;
            }
        }

        public bool LetraCorrecta;

        private Coordenada coord;
        public Coordenada Coordenada
        {
            get
            {
                return coord;
            }
            set
            {
                coord = value;
                Location = new Point(value.X * 20 + 10, value.Y * 20 + 10);
            }
        }

        public LetraCrucigrama(LetraEsperada letra)
        {
            Letra = letra;
            TabStop = false;
            GotFocus += (_, __) => { Display.BackColor = Color.LightCyan; ActualizarSeleccion(coord); };
            LostFocus += (_, __) => Display.BackColor = palabraCorrecta ? Color.LawnGreen : Color.Transparent;
            InitializeComponent();
            Display.TabStop = false;
        }

        public event Action<List<int>> LetraActualizada;
        public event Action<Coordenada> ActualizarSeleccion;

        private void actualizarLetra(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z && Display.Text != e.KeyCode.ToString())
            {
                Display.Text = e.KeyCode.ToString();
                LetraCorrecta = e.KeyCode.ToString() == string.Format("{0}", Letra.Letra);
                LetraActualizada(Letra.NumeroPalabra);
            }
        }

        private void generarIniciales(object sender, System.EventArgs e)
        {
            foreach (var num in Letra.LetraInicial)
            {
                Controls.Add(new Label()
                {
                    Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular, GraphicsUnit.Pixel),
                    AutoSize = true,
                    Text = string.Format("{0}\n", num.Key),
                    Location = new Point(num.Value == Orientacion.Vertical ? 10 : 0, num.Value == Orientacion.Vertical ? 0 : 10),
                    
                });
            }
        }
    }
}
