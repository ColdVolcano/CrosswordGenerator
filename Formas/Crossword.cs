using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System;

namespace CrosswordGenerator.Formas
{
    public partial class Crossword : Form
    {
        private readonly Dictionary<string, string> palabras = new Dictionary<string, string>();
        private SortedList<int, SortedList<int, LetraEsperada>> letrasPosicionadas = new SortedList<int, SortedList<int, LetraEsperada>>();

        private readonly Dictionary<string, string> verticales = new Dictionary<string, string>();
        private readonly Dictionary<string, string> horizontales = new Dictionary<string, string>();
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
                    palabras.Add(word, def);
            }

            Random rng = new Random();
            int numPalabras = 0;

            switch (diff)
            {
                case Dificultad.Facil:
                    numPalabras = 10;
                    break;
                case Dificultad.Medio:
                    numPalabras = 20;
                    break;
                case Dificultad.Dificil:
                    numPalabras = 30;
                    break;
            }
            while(numPalabras > 0)
            {
                string palabra = palabras.Keys.ToArray()[rng.Next(palabras.Count)];
                bool vertical = rng.Next(2) == 0;

                if (!letrasPosicionadas.Any()) // Primera palabra
                {
                    añadirPalabra(palabra, 0, 0, 0, vertical, 1);
                }
                else
                {

                }

                if (vertical)
                {
                    verticales.Add(palabra, palabras[palabra]);
                }
                else
                {
                    horizontales.Add(palabra, palabras[palabra]);
                }

                palabras.Remove(palabra);

                numPalabras--;

            }

            MessageBox.Show("all");
        }

        private void añadirPalabra(string word, int x, int y, int indiceInicial, bool vertical, int numeroPalabra)
        {
            if (vertical)
                y -= indiceInicial;
            else
                x -= indiceInicial;
            for (int i = 0; i < word.Length; i++)
            {
                if (!letrasPosicionadas.ContainsKey(x))
                    letrasPosicionadas.Add(x, new SortedList<int, LetraEsperada>());
                letrasPosicionadas[x].Add(y, new LetraEsperada { Letra = word[i], NumeroPalabra = 1 });

                if (vertical)
                    y++;
                else
                    x++;
            }
        }

        private class LetraEsperada
        {
            public char Letra;
            public int NumeroPalabra;

            public override string ToString()
            {
                return $"({NumeroPalabra}) {Letra}";
            }
        }
    }
}
