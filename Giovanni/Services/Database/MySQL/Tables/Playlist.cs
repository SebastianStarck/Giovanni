namespace Giovanni.Services.Database.MySQL.Tables
{
    [TableName("playlists")]
    public class Playlist
    {
        [Column(0, typeof(int))] public int ID;
        [Column(1, typeof(string))] public string Name;

        public override string ToString() => $"Playlist; ID: {ID}, Name: {Name}";
    }
}