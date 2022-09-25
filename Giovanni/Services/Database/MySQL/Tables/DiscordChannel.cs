namespace Giovanni.Services.Database.MySQL.Tables
{
    public class DiscordChannel
    {
        [Column("id", 0, typeof(string))] public string ID;
        [Column("name", 1, typeof(string))] public string Name;
        
        public DiscordChannel(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public override string ToString() => $"Channel; ID: {ID}, Name: {Name}";
    }
}