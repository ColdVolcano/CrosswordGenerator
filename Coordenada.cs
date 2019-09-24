namespace CrosswordGenerator
{
    public struct Coordenada
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}