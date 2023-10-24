namespace BattleshipsProto
{
    internal class Shot
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Coords position;
        private bool hasHit;

        public Coords Position { get => position; set => position = value; }
        public bool HasHit { get => hasHit; set => hasHit = value; }

        public Shot(Coords position)
        {
            this.position = position;
            hasHit = false;

            logger.Info($"Shot created: {ToString()}");
        }

        public override string? ToString()
        {
            return $"[Shot: Position={position}]";
        }
    }
}
