namespace ImportantFileManager
{
    static class ImportantFileManager
    {
        private const string ImportantFilesStorage = "important_files.txt";

        static void Main(string?[] args)
        {
            string? command = args[0];
            string? filePath = args[1];

            switch (command)
            {
                case "mark":
                    MarkFileAsImportant(filePath);
                    break;
                case "unmark":
                    UnmarkFileAsImportant(filePath);
                    break;
                case "find":
                    string? directory = args.Contains("--dir") ? args[Array.IndexOf(args, "--dir") + 1] : Directory.GetCurrentDirectory();
                    string? extension = args.Contains("--ext") ? args[Array.IndexOf(args, "--ext") + 1] : null;
                    string? nameContains = args.Contains("--name-contains") ? args[Array.IndexOf(args, "--name-contains") + 1] : null;
                    FindImportantFiles(directory, extension, nameContains);
                    break;
                default:
                    Console.WriteLine("Invalid command. Use mark, unmark, or find.");
                    break;
            }
        }

        static void MarkFileAsImportant(string? filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} does not exist.");
                return;
            }

            List<string?> importantFiles = GetImportantFiles();
            
            if (importantFiles.Contains(filePath))
            {
                Console.WriteLine($"File {filePath} is already marked as important.");
                return;
            }

            importantFiles.Add(filePath);
            SaveImportantFiles(importantFiles);
            Console.WriteLine($"File {filePath} marked as important.");
        }


        static void UnmarkFileAsImportant(string? filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} does not exist.");
                return;
            }

            List<string?> importantFiles = GetImportantFiles();
            
            if (!importantFiles.Contains(filePath))
            {
                Console.WriteLine($"File {filePath} is not marked as important.");
                return;
            }

            importantFiles.Remove(filePath);
            SaveImportantFiles(importantFiles);
            Console.WriteLine($"File {filePath} unmarked as important.");
        }

        
        static void FindImportantFiles(string? directory, string? extension, string? nameContains)
        {
            List<string?> importantFiles = GetImportantFiles();
            List<string?> matchingFiles = importantFiles.Where(file => file!.StartsWith(directory!) && (extension == null || file.EndsWith($".{extension}")) && (nameContains == null || Path.GetFileName(file).Contains(nameContains))).ToList();

            if (matchingFiles.Count == 0)
            {
                Console.WriteLine("No important files found matching the criteria.");
                return;
            }

            foreach (var file in matchingFiles)
            {
                Console.WriteLine(Path.GetFullPath(file!));
            }
        }

        static List<string?> GetImportantFiles()
        {
            if (!File.Exists(ImportantFilesStorage)) return new List<string?>();

            return File.ReadAllLines(ImportantFilesStorage).ToList()!;
        }

        static void SaveImportantFiles(List<string?> importantFiles)
        {
            File.WriteAllLines(ImportantFilesStorage, importantFiles!);
        }
    }
}

