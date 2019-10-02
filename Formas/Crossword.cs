using CrosswordGenerator.Controles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CrosswordGenerator.Formas
{
    public partial class Crossword : Form
    {
        private readonly Dictionary<string, string> palabras = new Dictionary<string, string>();
        private readonly Cuadricula<LetraCrucigrama> casillas = new Cuadricula<LetraCrucigrama>();

        private readonly SortedList<int, string> verticales = new SortedList<int, string>();
        private readonly SortedList<int, string> horizontales = new SortedList<int, string>();

        private Coordenada seleccionado;

        public Crossword(Dificultad diff)
        {
            Text = string.Format("Crucigrama ({0})", diff.ToString());
            MessageBox.Show("Utilice las teclas 2, 4, 6 y 8 para moverse por el crucigrama. La casilla seleccionada se tornará azul.");
            InitializeComponent();

            foreach (string s in File.ReadAllLines(@"Palabras.txt"))
            {
                if (s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length != 2)
                    continue;
                string word = s.Split('|')[0];
                string def = s.Split('|')[1];

                if (!palabras.ContainsKey(word))
                    palabras.Add(word.ToUpper(), def);
            }

            if (palabras.Count == 0)
            {
                MessageBox.Show("No se han cargado palabras para mostrar en el crucigrama.");
                Close();
            }

            Random rng = new Random();
            int numPalabras = 1;
            int limitePalabras = 0;

            switch (diff)
            {
                case Dificultad.Facil:
                    limitePalabras = 5;
                    break;
                case Dificultad.Medio:
                    limitePalabras = 7;
                    break;
                case Dificultad.Dificil:
                    limitePalabras = rng.Next(10, 21);
                    break;
            }

            while (numPalabras <= limitePalabras)
            {
                string palabra = palabras.Keys.Skip(rng.Next(palabras.Count - 1)).First();

                if (casillas.Width == 0) // Primera palabra
                {
                    añadirPalabra(palabra, new Coordenada(0, 0), 0, rng.Next(2) == 0 ? Orientacion.Horizontal : Orientacion.Vertical, numPalabras++);
                }
                else
                {
                    var entradas = new SortedList<int, Dictionary<LetraCrucigrama, Orientacion>>();

                    for (int i = 0; i < palabra.Length; i++)
                    {
                        var entradasPorIndice = casillas.Entradas.Where(ent => ent.Value.Letra.Letra == palabra[i]);
                        if (!entradasPorIndice.Any())
                            continue;

                        var entradasValidas = new Dictionary<LetraCrucigrama, Orientacion>();
                        foreach (var ent in entradasPorIndice)
                        {
                            Orientacion orien;
                            if ((orien = esLibreParaNuevaPalabra(ent.Key, palabra, i)) != 0)
                                entradasValidas.Add(ent.Value, orien);
                        }

                        if (entradasValidas.Any())
                            entradas.Add(i, entradasValidas);
                    }

                    if (entradas.Count == 0)
                        continue;

                    int indiceInterseccion = entradas.Keys.ToArray()[rng.Next(entradas.Count)];
                    Coordenada puntoIntersec = entradas[indiceInterseccion].Keys.ToArray()[rng.Next(entradas[indiceInterseccion].Count)].Coordenada;

                    añadirPalabra(palabra, puntoIntersec, indiceInterseccion, esLibreParaNuevaPalabra(puntoIntersec, palabra, indiceInterseccion), numPalabras++);
                }
            }

            foreach (var entrada in casillas.Entradas)
            {
                entrada.Value.KeyDown += moverCursor;
                entrada.Value.LetraActualizada += verificarPalabra;
                entrada.Value.ActualizarSeleccion += (nuevaSeleccion) => seleccionado = nuevaSeleccion;
                Controls.Add(entrada.Value);
            }

            seleccionado = casillas.Entradas.First(ent => ent.Value.Letra.LetraInicial.Any(num => num.Key == 1)).Key;
            casillas[seleccionado].Select();
            if (horizontales.Any())
            {
                label1.Text += "Horizontales:\n";
                foreach (var palabra in horizontales)
                {
                    label1.Text += string.Format("{0}: {1}\n", palabra.Key, palabra.Value);
                }
            }
            if (horizontales.Any() && verticales.Any())
                label1.Text += '\n';
            if (verticales.Any())
            {
                label1.Text += "Verticales:\n";
                foreach (var palabra in verticales)
                {
                    label1.Text += string.Format("{0}: {1}\n", palabra.Key, palabra.Value);
                }
            }
            label1.Location = new System.Drawing.Point(casillas.ColumnaFin * 20 + 60, 15);
            Size = new System.Drawing.Size(label1.Width + label1.Location.X + 15, Math.Max(label1.Height + 80, casillas.Height * 20 + 70));
            Focus();
        }

        private void verificarPalabra(List<int> palabras)
        {
            foreach (var entrada in casillas.Entradas.Where(ent => ent.Value.Letra.NumeroPalabra.Intersect(palabras).Any()))
            {
                bool[] palabraCorrecta = new bool[entrada.Value.Letra.NumeroPalabra.Count];
                for (int i = 0; i < entrada.Value.Letra.NumeroPalabra.Count; i++)
                    palabraCorrecta[i] = casillas.Entradas.Where(ent => ent.Value.Letra.NumeroPalabra.Any(pal => pal == entrada.Value.Letra.NumeroPalabra[i])).All(letra => letra.Value.LetraCorrecta);
                entrada.Value.PalabraCorrecta = palabraCorrecta.Any(esCorrecta => esCorrecta);
            }
            if (casillas.Entradas.All(ent => ent.Value.LetraCorrecta))
            {
                MessageBox.Show("Felicidades, completaste el crucigrama\nPor favor cierre este para crear uno nuevo");
                label1.Select();
            }
        }

        private void añadirPalabra(string word, Coordenada coord, int indiceInicial, Orientacion orientacion, int numeroPalabra)
        {
            if (orientacion == Orientacion.Vertical)
            {
                verticales.Add(numeroPalabra, palabras[word]);
                coord.Y -= indiceInicial;
            }
            else
            {
                horizontales.Add(numeroPalabra, palabras[word]);
                coord.X -= indiceInicial;
            }

            palabras.Remove(word);

            for (int i = 0; i < word.Length; i++)
            {
                if (casillas[coord] == null)
                    casillas[coord] = new LetraCrucigrama(new LetraEsperada
                    {
                        Letra = word[i],
                        NumeroPalabra = new List<int>(new int[] { numeroPalabra }),
                        LetraInicial = new Dictionary<int, Orientacion>()
                    });
                else if (word[i] == casillas[coord].Letra.Letra)
                    casillas[coord].Letra.NumeroPalabra.Add(numeroPalabra);

                if (i == 0)
                    casillas[coord].Letra.LetraInicial.Add(numeroPalabra, orientacion);
                if (orientacion == Orientacion.Vertical)
                    coord.Y++;
                else
                    coord.X++;
            }

            casillas.Desplazar(new Coordenada(casillas.ColumnaInicio, casillas.FilaInicio));

            foreach (var entrada in casillas.Entradas)
                entrada.Value.Coordenada = entrada.Key;
        }

        private Orientacion esLibreParaNuevaPalabra(Coordenada coord, string word, int offset)
        {
            int tam = word.Length;
            int x = coord.X;
            int y = coord.Y;

            if (casillas[x - 1, y] != null || casillas[x + 1, y] != null)
            {
                if (casillas[x, y + 1] == null && casillas[x, y - 1] == null)
                {
                    if (y - offset < casillas.FilaInicio || y - offset + tam - 1 > casillas.FilaFin)
                    {
                        if (Math.Max(y - offset + tam - 1, casillas.FilaFin) - Math.Min(y - offset, casillas.FilaInicio) + 1 > 30)
                            return Orientacion.Ninguno;
                    }

                    if (casillas[x, y - offset - 1] != null)                // *+*
                        return Orientacion.Ninguno;                         // *X*
                                                                            // *X*

                    if (casillas[x, y - offset + tam] != null)              // *X*
                        return Orientacion.Ninguno;                         // *X*
                                                                            // *+*
                    for (int i = y - offset; i < tam + y - offset; i++)
                    {
                        if (i == y)
                            continue;
                        if (casillas[x, i] != null && casillas[x, i].Letra.Letra != word[i - (y - offset)] || casillas[x - 1, i] != null || casillas[x + 1, i] != null) // ***
                            return Orientacion.Ninguno;                                                                                                                 // +X+
                                                                                                                                                                        // ***
                    }

                    return Orientacion.Vertical;
                }
            }
            if (casillas[x, y - 1] != null || casillas[x, y + 1] != null)
            {
                if (casillas[x + 1, y] == null && casillas[x - 1, y] == null)
                {
                    if (x - offset < casillas.ColumnaInicio || x - offset + tam - 1 > casillas.ColumnaFin)
                    {
                        if (Math.Max(x - offset + tam - 1, casillas.ColumnaFin) - Math.Min(x - offset, casillas.ColumnaInicio) + 1 > 25)
                            return Orientacion.Ninguno;
                    }

                    if (casillas[x - offset - 1, y] != null)                // ***
                        return Orientacion.Ninguno;                         // +XX
                                                                            // *X*

                    if (casillas[x - offset + tam, y] != null)              // ***
                        return Orientacion.Ninguno;                         // XX+
                                                                            // ***
                    for (int i = x - offset; i < tam + x - offset; i++)
                    {
                        if (i == x)
                            continue;
                        if (casillas[i, y] != null && casillas[i, y].Letra.Letra != word[i - (x - offset)] || casillas[i, y - 1] != null || casillas[i, y + 1] != null) // *+*
                            return Orientacion.Ninguno;                                                                                                                 // *X*
                                                                                                                                                                        // *+*
                    }

                    return Orientacion.Horizontal;
                }
            }

            return Orientacion.Ninguno;
        }

        private void moverCursor(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad8:
                case Keys.NumPad2:
                case Keys.NumPad4:
                case Keys.NumPad6:
                    Coordenada target = seleccionado;
                    switch (e.KeyCode)
                    {
                        case Keys.NumPad8:
                            target.Y--;
                            break;
                        case Keys.NumPad2:
                            target.Y++;
                            break;
                        case Keys.NumPad4:
                            target.X--;
                            break;
                        case Keys.NumPad6:
                            target.X++;
                            break;
                    }
                    e.Handled = true;
                    casillas[target]?.Select();
                    break;
            }
        }
    }
}
