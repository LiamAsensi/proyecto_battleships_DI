namespace BattleshipsProto
{
    internal class Game
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            Console.WriteLine(playerBoard.CheckDefeatStatus() ? 
                Program.Resources.GetString("defeat_mes"): Program.Resources.GetString("win_mes"));
            logger.Info(string.Format("{0}", 
                playerBoard.CheckDefeatStatus() ? "The enemy won" : "The player won"));
        }

        private void InitialTurn()
        {
            logger.Info("Starting the initial turn");
            int shipNum = 1;
            sizes.ToList().ForEach(s => AddShip(s, shipNum++));
            AddEnemyShips();
            userInterface.Clear();
            logger.Info("Ending the initial turn");
        }

        private void AddShip(ShipSize size, int shipNum)
        {
            Coords? crds;
            bool added;

            logger.Info("Asking for direction");
            Direction dir = GUI.AskDirection();
            userInterface.Clear();

            do
            {
                do
                {
                    logger.Info("Asking for coordinates");
                    crds = GUI.AskCoordinates(string.Format(Program.Resources.GetString("ask_coords_ship_mes"), shipNum));

                    if (crds is null)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.Error.WriteLine(Program.Resources.GetString("format_coords_error"));
                        logger.Error("Invalid format of coordinates");
                    }
                } while (crds is null);

                userInterface.Clear();

                Ship ship = new(size, dir, (Coords)crds);

                added = playerBoard.AddShip(ship);

                if (!added)
                {
                    Console.SetCursorPosition(0, 16);
                    Console.Error.WriteLine(Program.Resources.GetString("invalid_coords_error"));
                }
            } while (!added);

            userInterface.Clear();
        }

        private void AddEnemyShips()
        {
            logger.Info("Adding the enemy ships...");
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
                    logger.Info("Asking for coordinates");
                    crds = GUI.AskCoordinates(Program.Resources.GetString("ask_coords_shot_mes"));

                    if (crds is null)
                    {
                        Console.SetCursorPosition(0, 16);
                        Console.Error.WriteLine(Program.Resources.GetString("format_coords_error"));
                        logger.Error("Invalid format of coordinates");
                    }
                } while (crds is null);

                userInterface.Clear();

                shot = new((Coords)crds);

                logger.Info("Adding player shot...");
                added = enemyBoard.AddShot(shot);

                if (!added)
                {
                    Console.SetCursorPosition(0, 16);
                    Console.Error.WriteLine(Program.Resources.GetString("invalid_coords_error"));
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

                logger.Info("Adding enemy shot...");
                added = playerBoard.AddShot(shot);
            } while (!added);
            
            userInterface.Clear();

            return playerBoard.ships.Any(sp => sp.Fragments.Any(f => f.Position == shot.Position));
        }
    }
}
