using System.Collections.Generic;
using System.Linq;

namespace CrosswordGenerator
{
    public class Cuadricula<T> where T : class
    {
        private readonly SortedList<int, SortedList<int, T>> letrasPosicionadas = new SortedList<int, SortedList<int, T>>();

        public int StartColumn { get { return letrasPosicionadas.Any() ? letrasPosicionadas.First().Key : 0; } }
        public int EndColumn { get { return letrasPosicionadas.Any() ? letrasPosicionadas.Last().Key : 0; } }
        public int StartRow { get { return letrasPosicionadas.Any() ? letrasPosicionadas.Values.Min(col => col.Keys.Min()) : 0; } }
        public int EndRow { get { return letrasPosicionadas.Any() ? letrasPosicionadas.Values.Max(col => col.Keys.Max()) : 0; } }

        public T this[int x, int y]
        {
            get
            {
                if (letrasPosicionadas.ContainsKey(x) && letrasPosicionadas[x].ContainsKey(y))
                    return letrasPosicionadas[x][y];
                else
                    return null;
            }
            set
            {
                if (this[x, y] == value)
                    return;

                if (value == null)
                {
                    letrasPosicionadas[x].Remove(y);
                    if (letrasPosicionadas[x].Count == 0)
                        letrasPosicionadas.Remove(x);
                    return;
                }

                if (this[x, y] != null)
                {
                    letrasPosicionadas[x][y] = value;
                    return;
                }

                if (!letrasPosicionadas.ContainsKey(x))
                    letrasPosicionadas.Add(x, new SortedList<int, T>());
                letrasPosicionadas[x].Add(y, value);
            }
        }

        public Dictionary<Coordenada, T> Entradas
        {
            get
            {
                Dictionary<Coordenada, T> entradas = new Dictionary<Coordenada, T>();
                foreach (var columna in letrasPosicionadas)
                {
                    foreach (var entrada in columna.Value)
                    {
                        entradas.Add(new Coordenada
                        {
                            X = columna.Key,
                            Y = entrada.Key
                        }, entrada.Value);
                    }
                }
                return entradas;
            }
        }

        public int Width { get { return letrasPosicionadas.Count == 0 ? 0 : EndColumn - StartColumn + 1; } }
        public int Height { get { return letrasPosicionadas.Count == 0 ? 0 : EndRow - StartRow + 1; } }
    }
}
