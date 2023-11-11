namespace BookShop
{
    using Data;
    using Initializer;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
    using System.Linq;
    using BookShop.Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
           
            string result = GetBooksByPrice(db);
            Console.WriteLine(result);
        }


        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            if (Enum.TryParse(typeof(AgeRestriction), command, ignoreCase: true, out object ageRestriction))
            {
                var books = context.Books
                    .Where(b => b.AgeRestriction == (AgeRestriction)ageRestriction).Select(b =>b.Title).
                    OrderBy(b=>b)
                    .ToList();

                string result = string.Join(Environment.NewLine, books);
                return result;
            }
            else
            {
                return "Invalid age restriction.";
            }
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books.
                Where(b=>b.EditionType == EditionType.Gold && b.Copies < 5000).
                OrderBy(b => b.BookId).
                Select(b=>b.Title)
                .ToArray();

            string result = string.Join(Environment.NewLine, goldenBooks);
            return result;
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books.Where(b=>b.Price > 40).Select(b=> new
            {
                b.Title,
                b.Price,
            }).OrderByDescending(b => b.Price).ToList();
            string result = string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:f2}"));
            return result;
        }
    }

    
}


