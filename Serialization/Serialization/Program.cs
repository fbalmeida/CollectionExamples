using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

using var arquivo = new FileStream("songs.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);


/*
var songs = GetSongs(stream)
    .Where(song => song.Author.Equals("COLDPLAY", StringComparison.OrdinalIgnoreCase))
    .Take(20);
ListSongsTable(songs);
*/

var options = new JsonSerializerOptions
{
    WriteIndented=true
};
var artists = GetSongs(stream)
    .GroupBy(m => m.Author)
    .Select(g => new { Author = g.Key, Songs = g.OrderBy(m => m.Release), Total = g.Count() })
    .ToList();


var fileName = "artists.json";
using var jsonFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
JsonSerializer.Serialize(jsonFile, artists, options);

Console.WriteLine("Serializing complete.");


void ListSongsTable(IEnumerable<Music> songs)
{
    var title = "\nSongs from the File";
    Console.WriteLine(title);

    var columnTitle = "Title".PadRight(40);
    var columnAuthor = "Author".PadRight(30);
    var columnDuration = "Duration".PadRight(10);
    var columnRelease = "Released in".PadRight(15);
    Console.WriteLine($"{columnTitle}{columnAuthor}{columnDuration}{columnRelease}");
    var border = "=".PadRight(100, '-');
    Console.WriteLine(border);

    foreach (var music in songs)
    {
        var time = string.Format("{0, -10:F3}", music.Duration / 60.0);
        var line = $"{music.Title,-40}{music.Author,-30}{time}{music.Release,-15:dd/MM/yyyy}";
        Console.WriteLine(line);
    }
}

void ListSongs(IEnumerable<Music> musicas)
{
    Console.WriteLine("\nSongs from file:");
    foreach (var musica in musicas)
    {
        var linha = $"\t- {musica.Title} ({musica.Author}) - {musica.Duration}s [{musica.Release}]";
        Console.WriteLine(linha);
    }
}

IEnumerable<Music> GetSongs(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');

        int duration = 0;
        var match = Regex.Match(linha, @"(\d?\d):(\d\d)");
        if (match.Success)
        {
            var minutes = int.Parse(match.Groups[1].Value);
            var seconds = int.Parse(match.Groups[2].Value);
            duration = (minutes * 60) + seconds;
        }

        if (partes.Length == 5)
        {
            var musica = new Music
            {
                Title = string.IsNullOrWhiteSpace(partes[0]) ? "Title Not Found" : partes[0],
                Author = string.IsNullOrWhiteSpace(partes[1]) ? "Author Not Found" : partes[1],
                Duration = duration,
                Genre = partes[3].Split(',', StringSplitOptions.TrimEntries),
                Release = DateTime.ParseExact(partes[4], "dd/MM/yyyy", CultureInfo.InvariantCulture)
            };
            yield return musica;
        }

        linha = stream.ReadLine();
    }
}

class Music
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Duration { get; set; }
    public IEnumerable<string> Genre { get; set; }
    public DateTime Release { get; set; }
    public override string ToString()
    {
        return $"{Title} ({Author}) - {Duration}s [{Release}]";
    }
}