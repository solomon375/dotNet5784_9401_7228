// See https://aka.ms/new-console-template for more information

namespace Stage0;
 partial class Program
{
     static void Main(string[] args)
    {
        Welcome7228();
        Welcome9401();
        Console.ReadKey();
    }

    static partial void Welcome9401();

    private static void Welcome7228()
    {
        Console.WriteLine("Enter your name: ");
        string username = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first console application", username);
    }
}
