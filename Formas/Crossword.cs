using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System;
using CrosswordGenerator.Controles;

namespace CrosswordGenerator.Formas
{
    public partial class Crossword : Form
    {
        private readonly Dictionary<string, string> palabras = new Dictionary<string, string>();
        private readonly Cuadricula<LetraEsperada> casillas = new Cuadricula<LetraEsperada>();

        private readonly SortedList<int, KeyValuePair<string, string>> verticales = new SortedList<int, KeyValuePair<string, string>>();
        private readonly SortedList<int, KeyValuePair<string, string>> horizontales = new SortedList<int, KeyValuePair<string, string>>();
        public Crossword(Dificultad diff)
        {
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
            int numPalabras = 0;
            int limitePalabras = 0;

            switch (diff)
            {
                case Dificultad.Facil:
                    limitePalabras = 10;
                    break;
                case Dificultad.Medio:
                    limitePalabras = 25;
                    break;
                case Dificultad.Dificil:
                    limitePalabras = 40;
                    break;
            }
            while (numPalabras + 1 < limitePalabras)
            {
                string palabra = palabras.Keys.Skip(rng.Next(palabras.Count - 1)).First();

                if (casillas.Width == 0) // Primera palabra
                {
                    añadirPalabra(palabra, 0, 0, 0, rng.Next(2) == 0 ? Orientacion.Horizontal : Orientacion.Vertical, 1);
                }
                else
                {
                    var entradas = new SortedList<int, Dictionary<Coordenada, LetraEsperada>>();

                    for (int i = 0; i < palabra.Length; i++)
                    {
                        var entradasPorIndice = casillas.Entradas.Where(ent => ent.Value.Letra == palabra[i] && esLibreParaNuevaPalabra(ent.Key, palabra, i) != 0);
                        if (!entradasPorIndice.Any())
                            continue;

                        entradas.Add(i, new Dictionary<Coordenada, LetraEsperada>());
                        foreach (var ent in entradasPorIndice)
                            entradas[i].Add(ent.Key, ent.Value);
                    }

                    if (entradas.Count == 0)
                        continue;

                    int indiceInterseccion = entradas.Keys.ToArray()[rng.Next(entradas.Count)];
                    Coordenada puntoInterseccion = entradas[indiceInterseccion].Keys.ToArray()[rng.Next(entradas[indiceInterseccion].Count)];

                    añadirPalabra(palabra, puntoInterseccion.X, puntoInterseccion.Y, indiceInterseccion, esLibreParaNuevaPalabra(puntoInterseccion, palabra, indiceInterseccion), ++numPalabras);
                }
            }

            foreach (var entrada in casillas.Entradas)
            {
                Controls.Add(new BotonLetra(entrada.Value)
                {
                    Location = new System.Drawing.Point((entrada.Key.X - casillas.StartColumn) * 25 + 10, (entrada.Key.Y - casillas.StartRow) * 25 + 10)
                });
            }
        }

        private void añadirPalabra(string word, int x, int y, int indiceInicial, Orientacion orientacion, int numeroPalabra)
        {
            if (orientacion == Orientacion.Vertical)
            {
                verticales.Add(numeroPalabra, new KeyValuePair<string, string>(word, palabras[word]));
                y -= indiceInicial;
            }
            else
            {
                horizontales.Add(numeroPalabra, new KeyValuePair<string, string>(word, palabras[word]));
                x -= indiceInicial;
            }
            Console.WriteLine($"[{x}, {y}] ({(orientacion == Orientacion.Horizontal ? 'H' : 'V')}: {word}");

            palabras.Remove(word);

            for (int i = 0; i < word.Length; i++)
            {
                if (casillas[x, y] == null)
                    casillas[x, y] = new LetraEsperada { Letra = word[i], NumeroPalabra = new List<int>(new int[] { numeroPalabra }), LetraInicial = new List<int>() };
                else if (word[i] == casillas[x, y].Letra)
                    casillas[x, y].NumeroPalabra.Add(numeroPalabra);

                if (i == 0)
                    casillas[x, y].LetraInicial.Add(numeroPalabra);
                if (orientacion == Orientacion.Vertical)
                    y++;
                else
                    x++;
            }
        }

        private Orientacion esLibreParaNuevaPalabra(Coordenada coord, string word, int offset)
        {
            int tam = word.Length;
            int x = coord.X;
            int y = coord.Y;

            if ((casillas[x - 1, y] != null || casillas[x + 1, y] != null) && (casillas[x, y + 1] == null && casillas[x, y - 1] == null))
            {
                if (y - offset < casillas.StartRow || y - offset + tam - 1 > casillas.EndRow)
                {
                    if (Math.Max(y - offset + tam - 1, casillas.EndRow) - Math.Min(y - offset, casillas.StartRow) + 1 > 38)
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
                    if (casillas[x, i] != null && casillas[x, i].Letra != word[i - (y - offset)] || casillas[x - 1, i] != null || casillas[x + 1, i] != null) // ***
                        return Orientacion.Ninguno;                                                                                                         // +X+
                                                                                                                                                            // ***
                }

                return Orientacion.Vertical;
            }
            if ((casillas[x, y - 1] != null || casillas[x, y + 1] != null) && (casillas[x + 1, y] == null && casillas[x - 1, y] == null))
            {
                if (x - offset < casillas.StartColumn || x - offset + tam - 1 > casillas.EndColumn)
                {
                    if (Math.Max(x - offset + tam - 1, casillas.EndColumn) - Math.Min(x - offset, casillas.StartColumn) + 1 > 30)
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
                    if (casillas[i, y] != null && casillas[i, y].Letra != word[i - (x - offset)] || casillas[i, y - 1] != null || casillas[i, y + 1] != null) // *+*
                        return Orientacion.Ninguno;                                                         // *X*
                                                                                                            // *+*
                }

                return Orientacion.Horizontal;
            }

            return Orientacion.Ninguno;
        }
    }
}
