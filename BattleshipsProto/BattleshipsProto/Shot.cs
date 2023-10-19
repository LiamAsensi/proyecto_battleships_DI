namespace BattleshipsProto
{
    internal class Shot
    {
        private Coords position;
        private bool hasHit;

        public Coords Position { get => position; set => position = value; }
        public bool HasHit { get => hasHit; set => hasHit = value; }

        public Shot(Coords position)
        {
            this.position = position;
            hasHit = false;
        }
    }
}
