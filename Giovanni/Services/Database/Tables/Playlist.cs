namespace Giovanni.Services.Database.Tables
{
    [TableName("playlists")]
    public class Playlist
    {
        [Column("id", 0, typeof(int))] public int ID;
        [Column("name", 1, typeof(string))] public string Name;


        public override string ToString() => $"Playlist; ID: {ID}, Name: {Name}";
    }
}