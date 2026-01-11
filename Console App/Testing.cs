using System;
using System.Globalization;
public class Calculator{
    public static int Add(int addNumber1, int addNumber2)
    {
        return addNumber1 + addNumber2;
    }
    public static void Main(string[] args)
    {
        int num1=0;
        int num2=0;
        Console.WriteLine("Please enter number 1 = ");
        num1 = int.Parse(Console.ReadLine());
        Console.WriteLine("Please enter number 2 = ");
        num2 = int.Parse(Console.ReadLine());
 
            int result = Add(num1,num2);
            Console.WriteLine("The sum is: " + result);
    }
}