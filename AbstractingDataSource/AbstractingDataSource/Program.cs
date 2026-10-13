
using var file = new FileStream("songs.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(file);

//var musics = GetSongs(stream);
//ListSongs(musics);
//Console.WriteLine("-------");


var songsByMetallicaLonger = GetSongs(stream)
      .Where(music => music.Author == "Metallica") 
      .Where(music => music.Time >= 400);
ListSongs(songsByMetallicaLonger);
Console.WriteLine("-------");

//var songsByColdplay = GetSongs(stream)
//      .FilterBy(music => music.Author == "Coldplay");
//ListSongs(songsByColdplay);
//Console.WriteLine("-------");

void ListSongs(IEnumerable<Music> musics)
{
    var cont = 1;
    foreach(var music in musics)
    {
        Console.WriteLine($"\t * {music.Author} - {music.Title}");
        cont++;
        if (cont > 10) break;
    }
}

IEnumerable<Music> GetSongs(StreamReader stream)
{
    var line = stream.ReadLine();
    while (line is not null)
    {
        var parts = line.Split(";");
        var music = new Music
        {
            Title = parts[0],
            Author = parts[1],
            Time = Convert.ToInt32(parts[2])
        };
        yield return music;
        line = stream.ReadLine();
    }
}    

static class MusicExtensions
{
    public static IEnumerable<T> FilterBy<T>(this IEnumerable<T> collection, Func<T, bool> condition)
    {
        foreach (var element in collection)
        {
            if (condition(element)) yield return element;
        }
    }
}

class Music
{
    public string Title { get; set; }
    public string Author {  get; set; } 
    public int Time { get; set; }
}