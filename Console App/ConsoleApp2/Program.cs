// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<string> Tags { get; set; }

}

public class Program
{
    public static void Main()
    {
        // Json Deserialization Example
        string json = @"
        {
            'Name': 'Laptop',
            'Price': 999.99,
            'Tags': ['Electronics', 'Computers']
        }";
        Product product = JsonConvert.DeserializeObject<Product>(json);
        Console.WriteLine($"Product Name: {product.Name}");
        Console.WriteLine($"Price: {product.Price}");
        Console.WriteLine("Tags: " + string.Join(", ", product.Tags));

        // Json Serialization Example
        Product newProduct = new Product
        {
            Name = "Smartphone",
            Price = 699.99M,
            Tags = new List<string> { "Electronics", "Mobile" }
        };
        string serializedJson = JsonConvert.SerializeObject(newProduct, Formatting.Indented);
        Console.WriteLine("Serialized JSON:");
        Console.WriteLine(serializedJson);

    }
}