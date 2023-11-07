using Newtonsoft.Json;
using System.Configuration;
using System.Globalization;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace BattleshipsProto
{
    internal class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Config? Config { get; set; }
        public static ResourceManager? Resources { get; set; }

        static void Main(string[] args)
        {
            logger.Info($"Starting application");

            string json = File.ReadAllText("config.json"); // Carga la configuración desde el archivo JSON.
            Config = JsonConvert.DeserializeObject<Config>(json) ?? throw new InvalidDataException();

            Resources = new ResourceManager(Config.Language);
            CultureInfo cultura = CultureInfoFactory.CreateCultureInfo(Config.Language);
            Thread.CurrentThread.CurrentUICulture = cultura;

            Console.CursorVisible = false;
            Console.Title = "BattleShips";

            Game game = new();
            
            game.Run();

            logger.Info($"Ending application");
        }
    }
}