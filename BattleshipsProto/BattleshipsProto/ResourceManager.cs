using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsProto
{
    internal class ResourceManager
    {
        private IResourceManagerFactory factory;
        private System.Resources.ResourceManager resourceManager;

        public ResourceManager(string language)
        {
            // Seleccionar la factoría adecuada según el idioma
            if (language == "en")
            {
                factory = new EnglishResourceManagerFactory();
            }
            else if (language == "es")
            {
                factory = new SpanishResourceManagerFactory();
            }
            else
            {
                throw new ArgumentException("Language not supported");
            }

            // Crear el resource manager usando la factoría
            resourceManager = factory.CreateResourceManager();
        }

        // Método que usa el resource manager para obtener una cadena de texto
        public string GetString(string name)
        {
            return resourceManager.GetString(name);
        }
    }
}
