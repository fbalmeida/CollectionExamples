
using System.Net;
using System.Numerics;

using var file = new FileStream("songs.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(file);

//var musics = GetSongs(stream);
//ListSongs(musics);
//Console.WriteLine("-------");
//Statistics(stream);
//OperationsGroupingByArtist(stream);
//OperationsObtainElement(stream);
OperationsExistingVerification(stream);


void OperationsExistingVerification(StreamReader stream)
{
    var songs = GetSongs(stream).ToList();
    var artists = songs.GroupBy(m => m.Author)
                       .Where(g => g.Any(m => m.Time >= 480));
    foreach(var artist in artists)
    {
        Console.WriteLine($"\t - {artist.Key}");
    }
    Console.WriteLine("");
    Console.WriteLine("-----------");
    Console.WriteLine("");
    var reggae = songs
            .GroupBy(m => m.Author)
            .Where(g => g.Any(m => m.Genre.Contains("Reggae")));
    foreach (var artista in reggae)
    {
        Console.WriteLine($"\t - {artista.Key} has {artista.Count()} songs listed as Reggae");
    }
}

void ArtistWithMostSongs(StreamReader stream)
{
    var artists = GetSongs(stream)
    .GroupBy(m => m.Author)
    .Select(g => new { Author = g.Key, Songs = g, Total = g.Count() })
    .MaxBy(a => a.Total);
    Console.WriteLine($"The author with the most songs is {artists.Author}, with {artists.Total} songs.");
}

void OperationsObtainElement(StreamReader stream)
{
    var songs = GetSongs(stream).ToList();

    var firstsong = songs.First();
    Console.WriteLine($"The first song on the list is {firstsong.Title}");

    var longestsong = songs.MaxBy(m => m.Time);
    if (longestsong is not null )   
        Console.WriteLine($"The longest song on the list is {longestsong.Title} with {longestsong.Time} seconds.");

}

void OperationsGroupingByArtist(StreamReader stream)
{
    var authors = GetSongs(stream)
    .GroupBy(m => m.Author);

    Console.WriteLine($"\nListing songs by Artist:");
    foreach (var author in authors.Take(5))
    {
        Console.WriteLine($"Artist: {author.Key} with {author.Count()} songs.");
        foreach (var music in author)
        {
            Console.WriteLine($"\t - {music.Title}");
        }
    }
}

void Statistics(StreamReader stream)
{
    var songs = GetSongs(stream).ToList();

    Console.WriteLine($"\nThere are {songs.Count()} songs in  the collection.");
    Console.WriteLine($"\nThere are {songs.Count(m => m.Time >= 600)} songs over 10 minutes long in the collection.");
    Console.WriteLine($"\nThe song with the shortest duration is {songs.Min(m => m.Time)} seconds.");
    Console.WriteLine($"\nThe song with the longest duration is {songs.Max(m => m.Time)} seconds.");
    Console.WriteLine($"\nThe average duration is {songs.Average(m => m.Time)} seconds.");
    Console.WriteLine($"\nYou will take {songs.Sum(m => m.Time)/(3600*24)} days to listen to the whole collection!");

}

void OperationsProjection2(StreamReader stream)
{
    var genres = GetSongs(stream)
        .SelectMany(m => m.Genre)
        .Distinct()
        .OrderBy(g => g);

    foreach (var genre in genres)
    {
        Console.WriteLine(genre);
    }
}

void OperationsProjection(StreamReader stream)
{
    var authors = GetSongs(stream)
        .Select(m => m.Author)
        .Distinct()
        .OrderBy(a => a);

    foreach (var author in authors)
    {
        Console.WriteLine(author);
    }
}

void OperationsFilterOrder(StreamReader stream)
{
    var songsByMetallicaLonger = GetSongs(stream)
          .Where(music => music.Author == "Metallica")
          .OrderBy(music => music.Title)
          .Skip(5*2)
          .Take(5);
    
    ListSongs(songsByMetallicaLonger);
    Console.WriteLine("-------");
}

void ListSongs(IEnumerable<Music> musics)
{
    var cont = 1;
    foreach (var music in musics)
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
            Time = Convert.ToInt32(parts[2]),
            Genre = parts[3].Split(",").Select(g => g.Trim())
        };
        yield return music;
        line = stream.ReadLine();
    }
}

class Music
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Time { get; set; }
    public IEnumerable<string> Genre { get; set; }
}