using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Monoregion.App
{
    public class Configuration
    {
        public string DbSourceFileName { get; set; }

        public string ServiceUri { get; set; }

        private static Configuration _instance;
        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                    _instance = LoadConfigurationFromEmbededResources();

                return _instance;
            }
        }

        private static Configuration LoadConfigurationFromEmbededResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Monoregion.App.configuration.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<Configuration>(reader.ReadToEnd());
            }
        }
    }
}
