using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsProto
{
    internal static class CultureInfoFactory
    {
        private static readonly log4net.ILog logger = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static CultureInfo CreateCultureInfo(string language)
        {
            try
            {
                logger.Info("Selecting the language...");
                return new CultureInfo(language);
            }
            catch (CultureNotFoundException)
            {
                logger.Error("Invalid language. Using default language (English)");
                return new CultureInfo("en"); // Default to English (or another suitable default).
            }
        }
    }
}
