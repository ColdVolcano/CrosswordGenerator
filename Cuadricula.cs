using System.Collections.Generic;
using System.Linq;

namespace CrosswordGenerator
{
    public class Cuadricula<T> where T : class
    {
        private readonly SortedList<int, SortedList<int, T>> elementos = new SortedList<int, SortedList<int, T>>();

        public int ColumnaInicio { get { return elementos.Any() ? elementos.First().Key : 0; } }
        public int ColumnaFin { get { return elementos.Any() ? elementos.Last().Key : 0; } }
        public int FilaInicio { get { return elementos.Any() ? elementos.Values.Min(col => col.Keys.Min()) : 0; } }
        public int FilaFin { get { return elementos.Any() ? elementos.Values.Max(col => col.Keys.Max()) : 0; } }

        public T this[Coordenada xy]
        {
            get
            {
                return this[xy.X, xy.Y];
            }
            set
            {
                this[xy.X, xy.Y] = value;
            }
        }
        public T this[int x, int y]
        {
            get
            {
                if (elementos.ContainsKey(x) && elementos[x].ContainsKey(y))
                    return elementos[x][y];
                else
                    return null;
            }
            set
            {
                if (this[x, y] == value)
                    return;

                if (value == null)
                {
                    elementos[x].Remove(y);
                    if (elementos[x].Count == 0)
                        elementos.Remove(x);
                    return;
                }

                if (this[x, y] != null)
                {
                    elementos[x][y] = value;
                    return;
                }

                if (!elementos.ContainsKey(x))
                    elementos.Add(x, new SortedList<int, T>());
                elementos[x].Add(y, value);
            }
        }

        public void Desplazar(Coordenada despl)
        {
            var entradas = Entradas;
            elementos.Clear();
            foreach(var ent in entradas)
            {
                this[ent.Key.X - despl.X, ent.Key.Y - despl.Y] = ent.Value;
            }
        }

        public Dictionary<Coordenada, T> Entradas
        {
            get
            {
                Dictionary<Coordenada, T> entradas = new Dictionary<Coordenada, T>();
                foreach (var columna in elementos)
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

        public int Width { get { return elementos.Count == 0 ? 0 : ColumnaFin - ColumnaInicio + 1; } }
        public int Height { get { return elementos.Count == 0 ? 0 : FilaFin - FilaInicio + 1; } }
    }
}
