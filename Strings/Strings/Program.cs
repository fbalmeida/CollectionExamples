using System.Globalization;

using var arquivo = new FileStream("songs.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var songs = GetSongs(stream)
    .Where(song => song.Author.Equals("COLDPLAY", StringComparison.OrdinalIgnoreCase))
    .Take(20);
ListSongsTable(songs);


void ComparingStrings(StreamReader stream)
{
    var musics = GetSongs(stream)
        .Where(music => music.Author.Equals("COLDPLAY", StringComparison.OrdinalIgnoreCase))
        //.Where(m => m.Artista.ToUpper() == "COLDPLAY")
        .Take(20);

    // métodos que utilizam StringComparison
    "Coldplay".Equals("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".StartsWith("cold", StringComparison.OrdinalIgnoreCase);
    "Coldplay".EndsWith("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".IndexOf("coldplay", StringComparison.OrdinalIgnoreCase);
    "Coldplay".Contains("OLD", StringComparison.OrdinalIgnoreCase);
    "Coldplay".Replace("cold", "warm", StringComparison.OrdinalIgnoreCase);

    ListSongsTable(musics);
}

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

    foreach(var music in songs)
    {
        var time = string.Format("{0, -10:F3}", music.Duration / 60.0);
        var line = $"{music.Title,-40}{music.Author,-30}{time}{music.Release,-15:dd/MM/yyyy}";
        Console.WriteLine(line);  
    }
}


void AlterSongTitle(StreamReader stream)
{
    var songs = GetSongs(stream)
    .Where(m => m.Title.StartsWith('T'))
    .FirstOrDefault();

    if (songs is not null)
    {
        Console.WriteLine($"Song: {songs.Title} ({songs.Author})");
        songs.Title = songs.Title.Replace("The ", "");
        Console.WriteLine($"Song: {songs.Title} ({songs.Author})");
    }

}

void ValidatePassword()
{
    var password = "12345678Ab@";
    // at least 8 characters
    // one upper case character
    // one lower case character
    // at least one number
    // at least one symbol

    var totalChar = password.Length;
    var totalUpperChar = password.Count(c => char.IsUpper(c));
    var totalLowerChar = password.Count(c => char.IsLower(c));
    var totalNumbers = password.Count(c => char.IsDigit(c));
    var totalSymbols = password.Count(c => !char.IsLetterOrDigit(c));

    if (totalChar < 8 ||
        totalUpperChar == 0 ||
        totalLowerChar == 0 ||
        totalNumbers == 0 ||
        totalSymbols == 0)
    {
        Console.WriteLine("Weak Password.");
    }
    else
    {
        Console.WriteLine("Strong Password!");
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
        if (partes.Length == 5)
        {
            var musica = new Music
            {
                Title = string.IsNullOrWhiteSpace(partes[0]) ? "Title Not Found" : partes[0],
                Author = string.IsNullOrWhiteSpace(partes[1]) ? "Author Not Found": partes[1],
                Duration = int.TryParse(partes[2], out int duration) ? duration : 0,
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