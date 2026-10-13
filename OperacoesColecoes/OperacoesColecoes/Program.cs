
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

Music music1 = new("Olha o Boi", "Ozzy Osbourne", 125);
Music music2 = new("Jamona", "Michael Jackson", 203);
Music music3 = new("The Man Comes Around", "Johnny Cash", 180);
Music music4 = new("Bebendo Vinho", "Ira!", 97);
Music music5 = new("Rota em Colisao", "Rogerio Skylab", 212);
Music music6 = new("Mr Crowley", "Ozzy Osbourne", 200);
Music music7 = new("I dont wanna stop", "Ozzy Osbourne", 212);
Music music8 = new("Crazy Train", "Ozzy Osbourne", 212);

Playlist plist = new Playlist("Pendrive");
plist.Add(music1);
plist.Add(music2);
plist.Add(music3);
plist.Add(music4);
plist.Add(music5);
plist.Add(music6);
plist.Add(music7);
ShowPlaylist(plist);

Playlist plistOzzy = new Playlist("Best of Ozzy");
plistOzzy.Add(music1);
plistOzzy.Add(music6);
plistOzzy.Add(music7);
plistOzzy.Add(music8);
ShowPlaylist(plistOzzy);

//ShowMostPlayed(plist, plistOzzy);

var player = new MusicPlayer();
player.AddQueue(music2);
player.AddQueue(music4);
player.AddQueue(plistOzzy);
ShowPlayerQueue(player);
ShowPlayerHistory(player);

player.NextQueue();

ShowPlayerQueue(player);
ShowPlayerHistory(player);

void ShowPlayerHistory(MusicPlayer player)
{
    Console.WriteLine("Player - History:");
    foreach(var music in player.History())
    {
        Console.WriteLine($"\t Song - {music.Title}");
    }
}

void ShowPlayerQueue(MusicPlayer player)
{
    Console.WriteLine("Player - Queue:");
    foreach (var music in player.Queue())
    {
        Console.WriteLine($"\t Song - {music.Title}");
    }
}

    void ShowMostPlayed(Playlist plist1, Playlist plist2)
{
    //Title (key), Count (value)
    Dictionary<Music, int> ranking = [];

    foreach(var music in plist1)
    {
        ranking.Add(music, 1);
    }

    foreach (var music in plist2)
    {
        if (ranking.TryGetValue(music, out int cont))
        {
            cont++;
            ranking[music] = cont;
        }
        else
        {
            ranking[music] = 1;
        }     
    }

    List<KeyValuePair<Music, int>> top = new(ranking);
    top.Sort(new ByCount());

    Console.WriteLine("\n The top 3 most played songs:");
    int count = 1;
    foreach (var par in top) 
    {
        Console.WriteLine($"\t - {par.Key.Title}");
        count++;
        if (count > 3) break;
    }
}

void ShowPlaylist(Playlist playlist)
{
    Console.WriteLine($"\n Now playing the list {playlist.Name}");
    foreach (var music in playlist)
    {
        Console.WriteLine($"\t {music.Title}");
    }
}

void RemoveByTitle(Playlist playlist, string title)
{
    var musicFound = playlist.ObtainTitle(title);

    if (musicFound != null)
    {
        Console.WriteLine("Deleting Music...");
        playlist.Remove(musicFound);
    }
    else
    {
        Console.WriteLine("Music not found!");
    };
}

void RandomMusic(Playlist playlist)
{
    var musicRandom = playlist.RandomMusic();
    if (musicRandom != null)
    {
        Console.WriteLine($"\n The random song is {musicRandom.Title}");
    }
    else
    {
        Console.WriteLine("\n The playlist is empty!");
    }
}

class ByCount: IComparer<KeyValuePair<Music, int>>
{
    public int Compare(KeyValuePair<Music, int> x, KeyValuePair<Music, int> y)
    {
        return y.Value.CompareTo(x.Value);
    }
}

class ByAuthor : IComparer<Music>
{
    public int Compare(Music? x, Music? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Author.CompareTo(y.Author);
    }
}

class ByTitle : IComparer<Music>
{
    public int Compare(Music? x, Music? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Title.CompareTo(y.Title);
    }
}

class Music : IComparable
{
    public Music(string title, string author, int time)
    {
        Title = title;
        Author = author;
        Time = time;
    }

    public string Title {  get; set; }  
    public string Author {  get; set; } 
    public int Time {  get; set; }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is Music otherMusic) return this.Time.CompareTo(otherMusic.Time);
        return -1;
    }
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is Music otherMusic)
            return this.Title.Equals(otherMusic.Title) && this.Author.Equals(otherMusic.Author);
        return false;

    }

    public override int GetHashCode()
    {
        return this.Title.GetHashCode() ^ this.Author.GetHashCode();
    }

}

class Playlist: ICollection<Music>
{
    public Playlist(string name)
    {
        Name = name;
    }

    public string Name {  get; set; }

    private HashSet<Music> set = [];
    private List<Music> list = [];

    public int Count => list.Count;

    public bool IsReadOnly => false;

    public IEnumerator<Music> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Music item)
    {
        if (set.Add(item)) list.Add(item);
    }

    public void Clear()
    {
        list.Clear(); 
    }

    public bool Contains(Music item)
    {
        return list.Contains(item); 
    }

    public Music? ObtainTitle(string title)
    {
        foreach (var music in list) 
        {
            if (music.Title == title) return music;
        }
        return null;
    }

    public Music? RandomMusic()
    {
        if (list.Count == 0) return null;
        var random = new Random();
        var randomNumber = random.Next(0, list.Count - 1);
        return list[randomNumber];          
    }

    public void OrderByTime()
    {
        list.Sort();
    }

    public void OrderByAuthor()
    {
        list.Sort(new ByAuthor());
    }

    public void OrderByTitle()
    {
        list.Sort(new ByTitle());
    }

    public void CopyTo(Music[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex); 
    }

    public bool Remove(Music item)
    {
        return list.Remove(item);
    }
}

class MusicPlayer
{
    private Queue<Music> queue = []; //FIFO
    private Stack<Music> stack = []; //LIFO
    public void AddQueue(Music music)
    {
        queue.Enqueue(music);
    }

    public void AddQueue(Playlist plist)
    {
        foreach (var music in plist)
            AddQueue(music);
    }

    public Music? NextQueue()
    {
        if (queue.Count == 0) return null;
        var music = queue.Dequeue();
        stack.Push(music);
        return music;
    }

    public Music? PreviousQueue()
    {
        if (queue.Count == 0) return null;
        return stack.Pop();
    }

    public IEnumerable<Music> Queue()
    {
        foreach(var music in queue)
        {
            yield return music;
        }
    }

    public IEnumerable<Music> History()
    {
        foreach (var music in stack)
        {
            yield return music;
        }
    }
}