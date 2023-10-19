namespace BattleshipsProto
{
    internal class GUI
    {
        private Board playerBoard;
        private Board enemyBoard;

        public GUI(Board playerBoard, Board enemyBoard)
        {
            this.playerBoard = playerBoard;
            this.enemyBoard = enemyBoard;
        }

        public void DrawBoards()
        {
            DrawBoard(playerBoard, 1, 2, true);
            DrawBoard(enemyBoard, 20, 2, false);
        }

        private void DrawBoard(Board board, int x, int y, bool player)
        {
            Console.SetCursorPosition(x + 3, y - 1);
            Console.Write(player ? "JUGADOR" : "ENEMIGO");
            DrawBasicUI(x, y);

            DrawShots(board.shots, x, y);
            DrawShips(board.ships, x, y, player);
        }

        private void DrawShips(List<Ship> ships, int x, int y, bool player)
        {
            foreach (var ship in ships)
            {
                foreach (var frag in ship.Fragments)
                {
                    Console.ForegroundColor = ship.Destroyed ? ConsoleColor.Gray : frag.Destroyed ? ConsoleColor.DarkRed : ConsoleColor.Green;
                    Console.SetCursorPosition(frag.Position.x + x, frag.Position.y + y);
                    if (player || frag.Destroyed)
                    {
                        Console.Write("█");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private void DrawShots(List<Shot> shots, int  x, int y)
        {
            foreach(var shot in shots)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(shot.Position.x + x, shot.Position.y + y);
                Console.Write("¤");
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void DrawBasicUI(int x, int y)
        {
            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            Console.SetCursorPosition(x, y);
            Console.Write(" 123456789X");

            for (int i = 0, drawY = y + 1; i < 10; i++, drawY++)
            {
                Console.SetCursorPosition(x, drawY);
                Console.Write(letters[i]);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int j = 0; j < 10; j++)
                {
                    Console.Write("^");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void Clear()
        {
            Console.Clear();
            DrawBoards();
            Console.SetCursorPosition(0, 15);
        }

        public static Direction AskDirection()
        {
            ConsoleKeyInfo key;

            Console.SetCursorPosition(0, 15);
            Console.WriteLine("Dirección del barco: [H] Horizontal  [V] Vertical");

            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.H && key.Key != ConsoleKey.V);

            return key.Key == ConsoleKey.H ? Direction.Horizontal : Direction.Vertical;
        }

        public static Coords? AskCoordinates(string message)
        {
            string? answer;
            char[] coordsBoardX = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X' };
            char[] coordsBoardY = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            Console.SetCursorPosition(0, 15);
            Console.Write("                                            ");
            Console.SetCursorPosition(0, 15);
            Console.CursorVisible = true;
            Console.Write($"{message}: ");
            answer = Console.ReadLine()?.ToUpper();
            Console.CursorVisible = false;

            if (answer is null || answer.Length != 2)
            {
                return null;
            }

            if (!coordsBoardX.Contains(answer[1]) || !coordsBoardY.Contains(answer[0]))
            {
                return null;
            }

            return new Coords(Array.IndexOf(coordsBoardX, answer[1]) + 1, 
                Array.IndexOf(coordsBoardY, answer[0]) + 1);
        }
    }
}
