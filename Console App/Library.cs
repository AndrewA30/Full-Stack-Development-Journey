using System;

class LibraryManagementSystem
{
    static void Main()
    {
        // Step 1: Create variables for up to 5 books
        string book1 = "";
        string book2 = "";
        string book3 = "";
        string book4 = "";
        string book5 = "";

        // Track borrowed books
        int borrowedCount = 0;
        bool book1Borrowed = false;
        bool book2Borrowed = false;
        bool book3Borrowed = false;
        bool book4Borrowed = false;
        bool book5Borrowed = false;

        bool running = true;

        while (running)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("Choose an action: add, remove, display, search, borrow, checkin, exit");
            string action = Console.ReadLine()?.ToLower();

            switch (action)
            {
                case "add":
                    Console.Write("Enter the book title to add: ");
                    string newBook = Console.ReadLine();

                    if (string.IsNullOrEmpty(book1)) book1 = newBook;
                    else if (string.IsNullOrEmpty(book2)) book2 = newBook;
                    else if (string.IsNullOrEmpty(book3)) book3 = newBook;
                    else if (string.IsNullOrEmpty(book4)) book4 = newBook;
                    else if (string.IsNullOrEmpty(book5)) book5 = newBook;
                    else Console.WriteLine("Library is full. Cannot add more books.");
                    break;

                case "remove":
                    Console.Write("Enter the book title to remove: ");
                    string removeBook = Console.ReadLine();

                    if (book1 == removeBook) { book1 = ""; book1Borrowed = false; }
                    else if (book2 == removeBook) { book2 = ""; book2Borrowed = false; }
                    else if (book3 == removeBook) { book3 = ""; book3Borrowed = false; }
                    else if (book4 == removeBook) { book4 = ""; book4Borrowed = false; }
                    else if (book5 == removeBook) { book5 = ""; book5Borrowed = false; }
                    else Console.WriteLine("Book not found in the library.");
                    break;

                case "display":
                    Console.WriteLine("\nBooks in the library:");
                    if (!string.IsNullOrEmpty(book1)) Console.WriteLine(book1 + (book1Borrowed ? " (Borrowed)" : ""));
                    if (!string.IsNullOrEmpty(book2)) Console.WriteLine(book2 + (book2Borrowed ? " (Borrowed)" : ""));
                    if (!string.IsNullOrEmpty(book3)) Console.WriteLine(book3 + (book3Borrowed ? " (Borrowed)" : ""));
                    if (!string.IsNullOrEmpty(book4)) Console.WriteLine(book4 + (book4Borrowed ? " (Borrowed)" : ""));
                    if (!string.IsNullOrEmpty(book5)) Console.WriteLine(book5 + (book5Borrowed ? " (Borrowed)" : ""));
                    break;

                case "search":
                    Console.Write("Enter the book title to search: ");
                    string searchBook = Console.ReadLine();

                    if (book1 == searchBook || book2 == searchBook || book3 == searchBook || book4 == searchBook || book5 == searchBook)
                        Console.WriteLine($"The book \"{searchBook}\" is available in the library.");
                    else
                        Console.WriteLine($"The book \"{searchBook}\" is not in the collection.");
                    break;

                case "borrow":
                    if (borrowedCount >= 3)
                    {
                        Console.WriteLine("You cannot borrow more than 3 books at a time.");
                        break;
                    }

                    Console.Write("Enter the book title to borrow: ");
                    string borrowBook = Console.ReadLine();

                    if (book1 == borrowBook && !book1Borrowed) { book1Borrowed = true; borrowedCount++; Console.WriteLine($"You borrowed \"{borrowBook}\"."); }
                    else if (book2 == borrowBook && !book2Borrowed) { book2Borrowed = true; borrowedCount++; Console.WriteLine($"You borrowed \"{borrowBook}\"."); }
                    else if (book3 == borrowBook && !book3Borrowed) { book3Borrowed = true; borrowedCount++; Console.WriteLine($"You borrowed \"{borrowBook}\"."); }
                    else if (book4 == borrowBook && !book4Borrowed) { book4Borrowed = true; borrowedCount++; Console.WriteLine($"You borrowed \"{borrowBook}\"."); }
                    else if (book5 == borrowBook && !book5Borrowed) { book5Borrowed = true; borrowedCount++; Console.WriteLine($"You borrowed \"{borrowBook}\"."); }
                    else Console.WriteLine("Book not found or already borrowed.");
                    break;

                case "checkin":
                    Console.Write("Enter the book title to check in: ");
                    string checkinBook = Console.ReadLine();

                    if (book1 == checkinBook && book1Borrowed) { book1Borrowed = false; borrowedCount--; Console.WriteLine($"You checked in \"{checkinBook}\"."); }
                    else if (book2 == checkinBook && book2Borrowed) { book2Borrowed = false; borrowedCount--; Console.WriteLine($"You checked in \"{checkinBook}\"."); }
                    else if (book3 == checkinBook && book3Borrowed) { book3Borrowed = false; borrowedCount--; Console.WriteLine($"You checked in \"{checkinBook}\"."); }
                    else if (book4 == checkinBook && book4Borrowed) { book4Borrowed = false; borrowedCount--; Console.WriteLine($"You checked in \"{checkinBook}\"."); }
                    else if (book5 == checkinBook && book5Borrowed) { book5Borrowed = false; borrowedCount--; Console.WriteLine($"You checked in \"{checkinBook}\"."); }
                    else Console.WriteLine("Book not found or not currently borrowed.");
                    break;

                case "exit":
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid action. Please type add, remove, display, search, borrow, checkin, or exit.");
                    break;
            }
        }
    }
}
