using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the edition_id: ");
        string editionId = Console.ReadLine();

        // Path validation
        string assetsDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), $"..//..//assets//{editionId}"));

        if (!Directory.Exists(assetsDir))
        {
            Console.WriteLine($"Error: Cards folder does not exist: {assetsDir}");
            Environment.Exit(1);
        }

        Console.WriteLine($"Generating CDF files for cards in: {assetsDir}");

        // Read internal CSV data and convert it to CardRecord objects
        var records = ReadCardsFromCsv("cards.csv");

        // Generate CDFs
        foreach (var record in records)
        {
            string cardImagePath = Path.Combine(assetsDir, $"{record.Filename.Replace(' ', '_').ToLower()}.png");
            if (Regex.IsMatch(cardImagePath, @"^[a-z0-9/._-]+\.png$"))
            {
                GenerateCdfFile(cardImagePath, editionId, record);
            }
            else
            {
                Console.WriteLine($"{cardImagePath} does not match required Regex");
            }
        }

        // Inform the user that CDF files have been generated
        Console.WriteLine("CDF files generated successfully.");

        // Wait for user input before closing the console window
        Console.ReadLine();
    }

    static void GenerateCdfFile(string cardImagePath, string editionId, CardRecord record)
    {
        string filenameNoExtension = Path.GetFileNameWithoutExtension(cardImagePath);

        // If category is not provided, set it to "Uncategorized"
        if (string.IsNullOrEmpty(record.Category))
        {
            record.Category = "Uncategorized";
        }

        // If description is not provided, set it to an empty string
        if (string.IsNullOrEmpty(record.Description))
        {
            record.Description = "";
        }

        // Form the illustration path
        string illustrationPath = $"{editionId}/{filenameNoExtension}";

        // Get the drop weight
        int dropWeight = GetDropWeight(record.Rarity);

        // Form the updated CDF data
        string cdfData = $"EditionID={editionId}" +
                         $"\nCardID={GenerateCardId(filenameNoExtension.Replace("___", ")").Replace("__", " ("))}" +
                         $"\nRarityLevel={record.Rarity}" +
                         $"\nName={GenerateCardName(filenameNoExtension.Replace("___", ")").Replace("__", " ("))}" +
                         $"\nCategory={record.Category}" +
                         $"\nDropWeight={dropWeight}" +
                         $"\nDescription={record.Description}" +
                         $"\nIllustrationPath={illustrationPath}";

        // Overwrite the existing CDF file with the updated content
        File.WriteAllText($"{filenameNoExtension}.cdf", cdfData);
    }

    // Other methods...

    static string GenerateCardId(string filename)
    {
        string cardId = filename.ToLower();

        // Remove the file extension
        cardId = Path.GetFileNameWithoutExtension(cardId);

        // Remove rarity suffix from cardId
        cardId = Regex.Replace(cardId, @"_(com|common|unc|uncommon|rare|anc|ancient|leg|legendary)$", "");

        // Replace spaces and other non-alphanumeric characters with underscores
        cardId = Regex.Replace(cardId, @"\W+", "_");

        return cardId;
    }

    static string GenerateCardName(string filename)
    {
        string imageName = Path.GetFileNameWithoutExtension(filename);

        // Remove the rarity suffix from imageName
        imageName = Regex.Replace(imageName, @"_(com|common|unc|uncommon|rare|anc|ancient|leg|legendary)$", "");

        // Remove a prefix of consecutive numbers followed by an underscore
        imageName = Regex.Replace(imageName, @"^\d+_", "");

        // Replace underscores with spaces
        imageName = imageName.Replace('_', ' ');

        // Capitalize each word in imageName
        imageName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(imageName);

        return imageName;
    }

    static int GetDropWeight(string rarity)
    {
        int weight = 0;

        switch (rarity)
        {
            case "com":
            case "common":
                weight = 10;
                break;

            case "unc":
            case "uncommon":
                weight = 5;
                break;

            case "rare":
                weight = 3;
                break;

            case "anc":
            case "ancient":
                weight = 2;
                break;

            case "leg":
            case "legendary":
                weight = 1;
                break;

            default:
                weight = 10;
                break;
        }

        return weight;
    }

    // Define a class to hold the CSV records
    public class CardRecord
    {
        [Name("filename")]
        public string Filename { get; set; }

        [Name("category")]
        public string Category { get; set; }

        [Name("description")]
        public string Description { get; set; }

        [Name("rarity")]
        public string Rarity { get; set; }
    }

    static List<CardRecord> ReadCardsFromCsv(string csvFilePath)
    {
        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<CardRecord>().ToList();
        }
    }
}
