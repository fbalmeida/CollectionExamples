using System.Collections;
using System.Diagnostics;

var weekDays = new WeekDays(); 

var cart = new List<Product>()
{
    new Product() {Name="Milk", Price=7.89},
    new Product() {Name="Butter", Price=3.45},
};

void listCartForeach()
{
    foreach (var product in cart)
    {
        Console.WriteLine($"{product.Name} - Price ${product.Price}");
    };
}

void listCartFor()
{
    for (int i = 0; i < cart.Count; i++)
{
        Console.WriteLine($"Item {i} - {cart[i].Name} - Price ${cart[i].Price}");
    };
}

void ListWeekDaysEnumerator()
{
    var enumerator =  weekDays.GetEnumerator(); 
    while (enumerator.MoveNext())
    {
        var day =  enumerator.Current;
        Console.WriteLine(day);
    }
}

void ListWeekDays()
{
    foreach(string day in weekDays)
    {
        Console.WriteLine(day);
    }
}

var pares = EvenNumbers();
var count = 0;
foreach (var par in pares)
{
    count++;
    Console.WriteLine(par);
    if (count >= 7) break;
}


    IEnumerable<int> EvenNumbers()
{
    int i = 1;
    while(true)
    {
        Console.WriteLine($"Processando elemento {i}...");
        yield return i * 2;
        i++;
    }
}

class Product
{
    public string Name { get; set; }   
    public double Price { get; set; }   
};

class WeekDaysEnumerator : IEnumerator<string>
{
    private int position = -1;
    private string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

    public string Current => days[position];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        position++;
        return position < days.Length;
    }

    public void Reset()
    {
        position = -1;
    }
}

class WeekDays: IEnumerable<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        yield return "Sunday";
        yield return "Monday";
        yield return "Tuesday";
        yield return "Wednesday";
        yield return "Thursday";
        yield return "Friday";
        yield return "Saturday";
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}