namespace Giovanni.Models
{
    public class MongoDBSettings
    {
        public static string Port = "mongodb://localhost:27017";

        public string ConnectionString { get; set; } = null!;

        public static string DB = "Discord";
    }
}