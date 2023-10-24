[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace BattleshipsProto
{
    internal class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            logger.Info($"Starting application");

            Console.CursorVisible = false;
            Console.Title = "BattleShips";

            Game game = new();
            
            game.Run();

            logger.Info($"Ending application");
        }
    }
}