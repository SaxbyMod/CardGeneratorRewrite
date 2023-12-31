using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

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

        // Generate CDFs
        foreach (string filename in Directory.GetFiles(assetsDir, "*.png"))
        {
            GenerateCdfFile(filename, editionId);
        }

        // Inform the user that CDF files have been generated
        Console.WriteLine("CDF files generated successfully.");

        // Wait for user input before closing the console window
        Console.ReadLine();
    }

    static void GenerateCdfFile(string cardImagePath, string editionId)
    {
        string filenameNoExtension = Path.GetFileNameWithoutExtension(cardImagePath);

        // Extract category and description from the filename
        ExtractCategoryAndDescription(filenameNoExtension, out string category, out string description);

        // If category is not provided, set it to "Uncategorized"
        if (string.IsNullOrEmpty(category))
        {
            category = "Uncategorized";
        }

        // If description is not provided, set it to an empty string
        if (string.IsNullOrEmpty(description))
        {
            description = "";
        }

        // Form the illustration path
        string illustrationPath = $"{editionId}/{filenameNoExtension}";

        // Get the rarity level
        string rarity = GetRarityFromFilename(Path.GetFileName(cardImagePath));

        // Get the drop weight
        int dropWeight = GetDropWeight(rarity);

        // Form the updated CDF data
        string cdfData = $"EditionID={editionId}" +
                         $"\nCardID={GenerateCardId(filenameNoExtension)}" +
                         $"\nRarityLevel={rarity}" +
                         $"\nName={GenerateCardName(filenameNoExtension)}" +
                         $"\nCategory={category}" +
                         $"\nDropWeight={dropWeight}" +
                         $"\nDescription={description}" +
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

        // Remove anything in parentheses or square brackets from cardId
        cardId = Regex.Replace(cardId, @"\([^\)]*\)|\[[^\]]*\]", "");

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

        // Remove anything in parentheses or square brackets from imageName
        imageName = Regex.Replace(imageName, @"\([^\)]*\)|\[[^\]]*\]", "");

        // Capitalize each word in imageName
        imageName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(imageName);

        return imageName;
    }

    static string GetRarityFromFilename(string filename)
    {
        string rarity = "";

        Regex regex = new Regex("_(com|common|unc|uncommon|rare|anc|ancient|leg|legendary).png$", RegexOptions.IgnoreCase);
        Match match = regex.Match(filename);

        if (match.Success)
        {
            rarity = match.Groups[1].Value.ToLower();
        }
        else
        {
            rarity = "common";
        }

        return rarity;
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
                weight = 2;
                break;

            default:
                weight = 10;
                break;
        }

        return weight;
    }

    static void ExtractCategoryAndDescription(string filename, out string category, out string description)
    {
        category = "";
        description = "";

        // Category
        Regex categoryRegex = new Regex(@"\((.*?)\)");
        Match categoryMatch = categoryRegex.Match(filename);
        if (categoryMatch.Success)
        {
            category = categoryMatch.Groups[1].Value;
            category = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(category);
        }

        // Description
        Regex descRegex = new Regex(@"\[(.*?)\]");
        Match descMatch = descRegex.Match(filename);
        if (descMatch.Success)
        {
            description = descMatch.Groups[1].Value;
            description = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(description);
        }
    }
}
