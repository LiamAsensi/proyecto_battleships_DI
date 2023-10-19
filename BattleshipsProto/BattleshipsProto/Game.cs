namespace BattleshipsProto
{
    internal class Game
    {
        private Board playerBoard;
        private Board enemyBoard;
        private GUI userInterface;
        private bool gameEnded;
        private readonly ShipSize[] sizes = new ShipSize[] { ShipSize.Large, ShipSize.Medium, ShipSize.Medium, ShipSize.Small, ShipSize.Tiny };

        public Game() 
        { 
            playerBoard = new Board();
            enemyBoard = new Board();
            userInterface = new GUI(playerBoard, enemyBoard);
            gameEnded = false;
        }

        public void Run()
        {
            bool userHasHit = false;
            bool enemyHasHit = false;

            userInterface.Clear();
            InitialTurn();

            do
            {
                if (!enemyHasHit)
                {
                    userHasHit = AddShot();
                    gameEnded = playerBoard.CheckDefeatStatus() || enemyBoard.CheckDefeatStatus();
                }

                if (!gameEnded && !userHasHit)
                {
                    Thread.Sleep(750);
                    enemyHasHit = AddEnemyShot();
                    gameEnded = playerBoard.CheckDefeatStatus() || enemyBoard.CheckDefeatStatus();
                }
            } while (!gameEnded);

            Console.SetCursorPosition(0, 15);
            Console.WriteLine(playerBoard.CheckDefeatStatus() ? "Has perdido..." : "¡Has ganado!");
        }

        private void InitialTurn()
        {
            int shipNum = 1;
            sizes.ToList().ForEach(s => AddShip(s, shipNum++));
            AddEnemyShips();
            userInterface.Clear();
        }

        private void AddShip(ShipSize size, int shipNum)
        {
            Coords? crds;
            bool added;

            Direction dir = GUI.AskDirection();
            userInterface.Clear();

            do
            {
                do
                {
                    crds = GUI.AskCoordinates($"Coordenadas del barco {shipNum}");

                    if (crds is null)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.Error.WriteLine("Coordenadas inválidas");
                    }
                } while (crds is null);

                userInterface.Clear();

                Ship ship = new(size, dir, (Coords)crds);

                added = playerBoard.AddShip(ship);

                if (!added)
                {
                    Console.SetCursorPosition(0, 16);
                    Console.Error.WriteLine("La posición es inválida");
                }
            } while (!added);

            userInterface.Clear();
        }

        private void AddEnemyShips()
        {
            var rand = new Random();

            foreach (var size in sizes)
            {
                bool added;

                do
                {
                    Direction dir = rand.Next(0, 2) == 0 ? Direction.Horizontal : Direction.Vertical;
                    Ship ship = new (size, dir, new Coords(rand.Next(0, 10), rand.Next(0, 10)));

                    added = enemyBoard.AddShip(ship);
                } while (!added);
            }
        }

        private bool AddShot()
        {
            Coords? crds = new();
            Shot shot;
            bool added = false;

            do
            {
                do
                {
                    crds = GUI.AskCoordinates($"Coordenadas del disparo");

                    if (crds is null)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.Error.WriteLine("Coordenadas inválidas");
                    }
                } while (crds is null);

                userInterface.Clear();

                shot = new((Coords)crds);

                added = enemyBoard.AddShot(shot);

                if (!added)
                {
                    Console.SetCursorPosition(0, 16);
                    Console.Error.WriteLine("La posición es inválida");
                }
            } while (!added);

            userInterface.Clear();

            return enemyBoard.ships.Any(sp => sp.Fragments.Any(f => f.Position == shot.Position));
        }

        private bool AddEnemyShot()
        {
            Shot shot;
            var rand = new Random();
            bool added;

            do
            {
                shot = new(new Coords(rand.Next(0, 10), rand.Next(0, 10)));

                added = playerBoard.AddShot(shot);
            } while (!added);

            userInterface.Clear();

            return playerBoard.ships.Any(sp => sp.Fragments.Any(f => f.Position == shot.Position));
        }
    }
}
