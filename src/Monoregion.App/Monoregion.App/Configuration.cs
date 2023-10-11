using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Monoregion.App
{
    public class Configuration
    {
        public string DbSourceFileName { get; set; }

        public string ServiceUri
        {
            get => Preferences.Get("serviceUri", "http://192.168.0.105");
            set => Preferences.Set("serviceUri", value);
        }

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
