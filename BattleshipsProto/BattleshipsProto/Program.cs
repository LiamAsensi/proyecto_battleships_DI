[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

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