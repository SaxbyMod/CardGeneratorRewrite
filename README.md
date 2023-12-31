# Card Generator Rewrite

This C# script is designed to format data for a card game, creating Card Definition Files (CDFs) based on the images in a specified directory.

## Usage

1. **Edition ID Input:**
   - The script begins by prompting the user to enter the edition ID. This is used to locate the directory containing card images.

2. **Directory Validation:**
   - It validates the path for the existence of the specified card directory. If the directory does not exist, it displays an error message and exits the program.

3. **CDF Generation Loop:**
   - For each PNG file in the specified directory, the script generates a corresponding CDF file.

4. **CDF Data Generation:**
   - For each card, it extracts information such as category, description, rarity, and drop weight from the filename and generates a CDF with this information.

5. **File Overwriting:**
   - The script overwrites any existing CDF files with the updated content.

6. **Success Message:**
   - After processing all files, it prints a success message.

7. **Console Wait:**
   - The program waits for user input before closing the console window.

## Methods Overview

1. **GenerateCdfFile:**
   - Takes a card image path and the edition ID as input.
   - Extracts information from the filename and generates CDF data.
   - Writes the CDF data to a file.

2. **GenerateCardId:**
   - Generates a card ID by converting the filename to lowercase, removing file extensions, parentheses, square brackets, and rarity suffixes, and replacing non-alphanumeric characters with underscores.

3. **GenerateCardName:**
   - Generates a card name by removing the rarity suffix, a prefix of consecutive numbers followed by an underscore, replacing underscores with spaces, and capitalizing each word.

4. **GetRarityFromFilename:**
   - Extracts the rarity level from the filename.

5. **GetDropWeight:**
   - Assigns a drop weight based on the rarity level.

6. **ExtractCategoryAndDescription:**
   - Extracts category and description from the filename, capitalizes each word, and assigns them to respective variables.

## Image Formatting

- Images are expected to follow the format: `n_a_m_e(Category Insert)[Description Insert]_Rarity.png`
- The script extracts information from this format to generate CDFs.

## Important Notes

- The script is designed to overwrite existing CDF files with updated content.
- It handles variations in the filenames and generates consistent CDFs.

## How to Use

1. **Run the Script:**
   - Execute the script in a C# environment.

2. **Enter Edition ID:**
   - Provide the edition ID when prompted.

3. **Review Output:**
   - Check the console output for success messages and any potential errors.

4. **CDF Files:**
   - Find generated CDF files in the same directory as the script.

5. **Adjustments:**
   - Modify methods as needed to accommodate specific filename variations or additional data requirements.

## Dependencies

- The script utilizes standard C# libraries for file handling, regular expressions, and console input/output.

## License

- Add a license statement if applicable.

## Additional Information

- Include any other relevant information, known issues, or potential improvements.

**Note:** Ensure to replace placeholder information with actual details specific to your use case.
