using Newtonsoft.Json;
using System;
using System.Text;

namespace Monoregion.App.Entites
{
    public abstract class DataStatusObject : IEquatable<DataStatusObject>
    {
        private string _id;

        [JsonProperty("id")]
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //[JsonProperty("version")]
        //public string Version { get; set; } = Encoding.UTF8oop--.GetBytes(DateTime.UtcNow.B.ToString("yyyy-MM-dd HH-mm-ss.fff")).ToString();

        [JsonProperty("version")]
        public string Version { get; set; }
        //= Encoding.UTF8.GetString(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()));

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        public bool Equals(DataStatusObject other)
        {
            //if (other != null && Id == other.Id && UpdatedAt == other.UpdatedAt && Deleted == other.Deleted)
            //{
            //    return Version.SequenceEqual(other.Version);
            //}

            return other != null && Id == other.Id;
        }
    }
}
