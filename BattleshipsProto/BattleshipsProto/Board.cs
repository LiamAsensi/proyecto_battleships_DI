namespace BattleshipsProto
{
    internal class Board
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int MAX_COORD = 10;
        public const int MIN_COORD = 1;
        public List<Ship> ships;
        public List<Shot> shots;

        public Board()
        {
            ships = new();
            shots = new();
            logger.Info("Board created");
        }

        public bool AddShip(Ship ship)
        {
            if (ship.Fragments.Any(f => f.Position.x > MAX_COORD || f.Position.x < MIN_COORD ||
                f.Position.y > MAX_COORD || f.Position.y < MIN_COORD))
            {
                logger.Error("Invalid coordinates! (Out of bounds)");
                return false;
            }
            if (ships.Any(s => s.CheckCollision(ship))) 
            {
                logger.Error("Invalid coordinates! (There's already another ship in there)");
                return false; 
            }

            ships.Add(ship);

            logger.Info("Ship added successfully");

            return true;
        }

        public bool AddShot(Shot shot)
        {
            if (shot.Position.x > MAX_COORD || shot.Position.x < MIN_COORD ||
                shot.Position.y > MAX_COORD || shot.Position.y < MIN_COORD)
            {
                logger.Error("Invalid coordinates! (Out of bounds)");
                return false;
            }

            if (ships.Any(s => s.Fragments.Any(f => f.Position == shot.Position && f.Destroyed))) 
            {
                logger.Error("Invalid coordinates! (There's already a shot in there)");
                return false;
            }
            else if (ships.Any(s => s.Fragments.Any(f => f.Position == shot.Position))) {
                shot.HasHit = true;
            }

            foreach (var ship in ships)
            {
                for (int i = 0; i < ship.Fragments.Length; i++)
                {
                    ship.Fragments[i].Destroyed = ship.Fragments[i].Position == shot.Position || ship.Fragments[i].Destroyed;
                }
            }

            logger.Info(string.Format("Fire! {0}", shot.HasHit ? "It hit!" : "It missed..."));

            ships.ForEach(s => s.CheckStatus());

            shots.Add(shot);

            logger.Info("Shot added successfully");

            return true;
        }

        public bool CheckDefeatStatus()
        {
            return ships.All(s => s.Destroyed);
        }
    }
}
