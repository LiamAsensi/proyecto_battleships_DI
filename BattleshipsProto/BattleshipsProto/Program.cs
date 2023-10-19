namespace BattleshipsProto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "BattleShips";

            Game game = new();
            
            game.Run();
        }
    }
}