namespace BookShop
{
    using Data;
    using Initializer;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
    using System.Linq;
    using BookShop.Models.Enums;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
           string input =Console.ReadLine();
            string result = GetAuthorNamesEndingIn(db, input);
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
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {

            var books = context.Books.Where(b => b.ReleaseDate.Value.Year != year).
                OrderBy(b => b.BookId).
                Select(b => b.Title).ToList();

            string result = string.Join(Environment.NewLine, books);
            return result;
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[]genres = input.Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(g =>g.ToLower()).ToArray();
            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => genres.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .ToList();
            string result = string.Join(Environment.NewLine,books.Select(b=> $"{b.Title}"));
            return result;
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
               .Where(b => b.ReleaseDate < parsedDate)
               .Select(b => new
               {
                   b.Title,
                   b.EditionType,
                   b.Price,
                   b.ReleaseDate
               })
               .OrderByDescending(b => b.ReleaseDate);

            string result = string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - " +
            $"${b.Price}"));
            return result;
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors.
                Where(a => a.FirstName.EndsWith(input)).
                AsEnumerable().
                Select(a => new {FullName = $"{a.FirstName} {a.LastName}"}).
                OrderBy(a => a.FullName).ToList();

            string result = string.Join(Environment.NewLine,authors.Select(a => $"{a.FullName}"));
            return result;
        }
    }

    
}


