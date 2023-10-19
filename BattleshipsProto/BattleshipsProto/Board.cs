namespace BattleshipsProto
{
    internal class Board
    {
        public const int MAX_COORD = 10;
        public const int MIN_COORD = 1;
        public List<Ship> ships;
        public List<Shot> shots;

        public Board()
        {
            ships = new();
            shots = new();
        }

        public bool AddShip(Ship ship)
        {
            if (ship.Fragments.Any(f => f.Position.x > MAX_COORD || f.Position.x < MIN_COORD ||
                f.Position.y > MAX_COORD || f.Position.y < MIN_COORD))
            {
                return false;
            }
            if (ships.Any(s => s.CheckCollision(ship))) 
            {
                return false; 
            }

            ships.Add(ship);

            return true;
        }

        public bool AddShot(Shot shot)
        {
            if (shot.Position.x > MAX_COORD || shot.Position.x < MIN_COORD ||
                shot.Position.y > MAX_COORD || shot.Position.y < MIN_COORD)
            {
                return false;
            }

            if (ships.Any(s => s.Fragments.Any(f => f.Position == shot.Position && f.Destroyed))) 
            {
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

            ships.ForEach(s => s.CheckStatus());

            shots.Add(shot);

            return true;
        }

        public bool CheckDefeatStatus()
        {
            return ships.All(s => s.Destroyed);
        }
    }
}
