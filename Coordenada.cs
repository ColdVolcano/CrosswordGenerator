namespace CrosswordGenerator
{
    public struct Coordenada
    {
        public Coordenada(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}