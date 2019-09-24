using System.Collections.Generic;

namespace CrosswordGenerator
{
    public class LetraEsperada
    {
        public char Letra;
        public List<int> NumeroPalabra;
        public List<int> LetraInicial;

        public override string ToString()
        {
            return $"({NumeroPalabra}) {Letra}";
        }
    }
}
