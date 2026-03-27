namespace tutorials;

using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
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
        // Part 1 & 2
        var p = new Personne("Marjolaine", 23);
        Console.WriteLine(JsonConvert.SerializeObject(p, Formatting.Indented));

        // Part 3
        var nb_iterations = 10;
        Console.WriteLine($"Comparison for {nb_iterations} iterations");

        var watch = System.Diagnostics.Stopwatch.StartNew();
        ResizeImgs(nb_iterations);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine($"Sequential time: {elapsedMs}ms");

        watch.Restart();
        ResizeImgsWithParallel(nb_iterations);
        watch.Stop();
        var elapsedMsParallel = watch.ElapsedMilliseconds;
        Console.WriteLine($"Parallel time: {elapsedMsParallel}ms");

        Console.WriteLine($"Speedup: {(double)elapsedMs / elapsedMsParallel:F2}");
    }

    private static void ResizeImgs(int nb_iterations)
    {
        for (int i = 0; i < nb_iterations; i++) ResizeImg(i);
    }

    private static void ResizeImgsWithParallel(int nb_iterations)
    {
        Parallel.For(0, nb_iterations, i => ResizeImg(i));
    }

    private static void ResizeImg(int i)
    {
        string inPath = $@"img/canard_{i}.png";
        string outPath = $@"img/resized/canard_{i}_resized.png";
        using (Image image = Image.Load(inPath))
        {
            int width = image.Width / 2;
            int height = image.Height / 2;
            image.Mutate(x => x.Resize(width, height));

            image.Save(outPath);
        }
    }

}
