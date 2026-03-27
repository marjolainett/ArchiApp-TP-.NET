namespace tutorials;

using System.Text.Json.Serialization;
using Newtonsoft.Json;
class Personne
{
    public Personne() : this("Anonymous", 0) { }
    public Personne(string name, int age)
    {
        Name = name;
        Age = age;
    }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Hello(bool isLowercase)
    {
        var response = $"hello {Name}, you are {Age}";
        if (isLowercase) return response;
        else return response.ToUpper();
    }
}

class Program
{
    static void Main()
    {
        var p = new Personne("Marjolaine", 23);
        Console.WriteLine(JsonConvert.SerializeObject(p, Formatting.Indented));
    }
}
