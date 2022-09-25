namespace Giovanni.Services.Database.MySQL.Tables
{
    public class DiscordChannel
    {
        [Column(0, typeof(int), columnName: "id", isPivot: true)]
        public int ID;

        [Column(0, typeof(string), columnName: "discord_id", isPivot: true)]
        public string DiscordID;

        [Column(1, typeof(string))] public string Name;

        public DiscordChannel()
        {
        }

        public override string ToString() => $"Channel; ID: {ID}, Name: {Name}";
    }
}