namespace MusicHub
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            int producerId = int.Parse(Console.ReadLine());
           var result = ExportAlbumsInfo(context,producerId)
                .ToList();
            Console.WriteLine(String.Join("", result));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Producers.Include(producer=> producer.Albums).
                ThenInclude(album=>album.Songs).ThenInclude(song => song.Writer).
                FirstOrDefault(p=>p.Id == producerId).Albums.Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    Song = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        s.Price,
                        WriterName = s.Writer.Name
                    })
                   .OrderByDescending(s => s.SongName).ThenBy(s => s.WriterName),
                   AlbumPrice = a.Price
                   }).OrderByDescending(a =>a.AlbumPrice) ;

            StringBuilder sb = new StringBuilder();
            
          
            foreach (var album  in albums)
            {
                
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");
                int counter = 1;
                foreach (var song in album.Song)
                {
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                    counter++;
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }
            

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
