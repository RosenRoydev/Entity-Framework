namespace BookShop
{
    using Data;
    using Initializer;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
    using System.Linq;
    using BookShop.Models.Enums;
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(RemoveBooks(db));

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
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books.
                Where(b => b.Title.ToLower().Contains(input.ToLower())).
                OrderBy(b=>b.Title).
                Select(b=>new { b.Title });
            string result = string.Join(Environment.NewLine, books.Select(b=> b.Title));
            return result;
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower())).
                Select(b => new
                {
                    BookName = b.Title,
                    AuthorNames = b.Author.FirstName + " " + b.Author.LastName

                });
            string result = string.Join(Environment.NewLine, books.Select(b => $"{b.BookName} ({b.AuthorNames})"));
            return result;
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books.Where(b => b.Title.Length > lengthCheck).Count();
           var result = books;
            return result;
           
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuhtorName = string.Join(" ", a.FirstName, a.LastName),
                    TotalBooks = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBooks).ToList();

            return string.Join(Environment.NewLine,
                authors.Select(a => $"{a.AuhtorName} - {a.TotalBooks}"));
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories.Select(b => new
            {
                b.Name,
                Profit = b.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
            }).OrderByDescending(b => b.Profit).ThenBy(b => b.Name);
            string result = string.Join(Environment.NewLine, books.Select(b => $"{b.Name} ${b.Profit:f2}"));
            return result;
                
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories.Select(b => new
            {
                CategoryName = b.Name,
                bookNames = b.CategoryBooks.OrderByDescending(b => b.Book.ReleaseDate).
                Select(b => new { b.Book.Title ,
                   b.Book.ReleaseDate.Value.Year} ).Take(3).ToList(),
                Year = b.CategoryBooks.Select(b => b.Book.ReleaseDate.Value.Year)
            }).OrderBy(b => b.CategoryName) ;
            StringBuilder sb = new();
            foreach (var book in books)
            {
                sb.AppendLine($"--{book.CategoryName}");
                foreach (var bookName in book.bookNames)
                {
                    sb.AppendLine($"{bookName.Title} ({bookName.Year})");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate!.Value.Year < 2010).ToList();
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            context.ChangeTracker.Clear();
            var books = context.Books.Where(b => b.Copies < 4200).ToList();
           context.RemoveRange(books);
           return context.SaveChanges();
            
        }
    }

    
}


