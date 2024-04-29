## How to Use and Install Guide for the Script:

### Step 1: Installation
1. Download or clone the script from the provided source.
2. Ensure you have a C# compatible environment, such as Visual Studio or a compatible IDE installed on your system.

### Step 2: Setup
1. Once you have the script downloaded, navigate to the directory containing the script files.
2. Make sure you have the necessary assets, particularly the `cards.csv` file and card images, organized in the correct structure:
   - The `cards.csv` file containing card data should be placed in the same directory as the script.
   - Card images should be placed in a directory named according to the edition ID inside the `assets` directory. For example, if the edition ID is "example_edition", place card images in the directory `assets/example_edition`.

### Step 3: Execution
1. Open the script file (`Program.cs`) in your preferred C# development environment.
2. Build the project to ensure all dependencies are resolved.
3. Run the script by executing the program.

### Step 4: Input
1. When prompted, enter the `edition_id` as requested by the script.

### Step 5: Output
1. The script will generate CDF files for cards based on the provided CSV data and card images.
2. The generated CDF files will be placed in the same directory as the script.

### Step 6: Conclusion
1. Once the script has completed execution, you will see a message indicating that CDF files have been generated successfully.
2. You can now use the generated CDF files as required.
