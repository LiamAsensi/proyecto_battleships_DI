using System.Text;

namespace BattleshipsProto
{
    internal class Ship
    {
        private Direction direction;
        private ShipFragment[] fragments;
        private bool destroyed;

        public Direction Direction { get => direction; set => direction = value; }
        public bool Destroyed { get => destroyed; set => destroyed = value; }
        internal ShipFragment[] Fragments { get => fragments; set => fragments = value; }

        public Ship(ShipSize size, Direction direction, Coords initialCoord)
        {
            this.direction = direction;
            fragments = new ShipFragment[(int)size];
            destroyed = false;

            AssignCoords(initialCoord);
        }

        private void AssignCoords(Coords coords)
        {
            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] = new ShipFragment(coords);
                coords = direction == Direction.Vertical ?
                    new Coords(coords.x, coords.y + 1) :
                    new Coords(coords.x + 1, coords.y);
            }
        }

        public void CheckStatus()
        {
            destroyed = fragments.ToList().All(f => f.Destroyed);
        }

        public bool CheckCollision(Ship ship)
        {
            for (int i = 0; i < ship.Fragments.Length; i++)
            {
                for (int j = 0; j < this.Fragments.Length; j++)
                {
                    if (ship.Fragments[i].Position == this.Fragments[j].Position)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string? ToString()
        {
            StringBuilder result = new();
            result.AppendLine($"[Ship: direction = {direction}; destroyed = {destroyed}; parts =");

            foreach (var frag in fragments)
            {
                result.AppendLine($"\t{frag.ToString()}");
            }

            result.Append("]");

            return result.ToString();
        }
    }

    public struct ShipFragment
    {
        private Coords position;
        private bool destroyed;

        public Coords Position { get => position; set => position = value; }
        public bool Destroyed { get => destroyed; set => destroyed = value; }

        public ShipFragment(Coords posicion)
        {
            this.position = posicion;
            destroyed = false;
        }

        public override string? ToString()
        {
            return $"[ShipFragment: x = {position.x}; y = {position.y}; destroyed = {destroyed}]";
        }
    }

    public enum ShipSize : int
    {
        Tiny = 2,
        Small = 3,
        Medium = 4,
        Large = 5
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }
}