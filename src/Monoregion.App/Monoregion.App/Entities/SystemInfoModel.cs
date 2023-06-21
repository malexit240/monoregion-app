using System.ComponentModel.DataAnnotations;

namespace Monoregion.App.Entites
{
    public class SystemInfoModel
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
