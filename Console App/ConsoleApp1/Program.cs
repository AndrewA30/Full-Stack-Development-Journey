// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Pastel;

Console.WriteLine("Hello, World .NET! oi".Pastel(Color.LightGreen));

public class Calculator
{
    public static int addNumber1=5;
    public static int addNumber2=10;

    public static int Add()
    {
        return addNumber1 + addNumber2;
    }
    public static void Main(string[] args)
    {
            int result = Add();
            Console.WriteLine("The sum is: " + result);
    }
}

