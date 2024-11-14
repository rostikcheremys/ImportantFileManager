namespace ImportantFileManager
{
    static class ImportantFileManager
    {
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
            
            FileAttributes attributes = File.GetAttributes(filePath);
            
            if ((attributes & FileAttributes.Archive) != FileAttributes.Archive)
            {
                File.SetAttributes(filePath, attributes | FileAttributes.Archive);
                Console.WriteLine($"File {filePath} marked as important.");
            }
            else
            {
                Console.WriteLine($"File {filePath} is already marked as important.");
            }
        }

        static void UnmarkFileAsImportant(string? filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} does not exist.");
                return;
            }
            
            FileAttributes attributes = File.GetAttributes(filePath);
            
            if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                File.SetAttributes(filePath, attributes & ~FileAttributes.Archive);
                Console.WriteLine($"File {filePath} unmarked as important.");
            }
            else
            {
                Console.WriteLine($"File {filePath} is not marked as important.");
            }
        }

        static void FindImportantFiles(string? directory, string? extension, string? nameContains)
        {
            if (directory != null)
            {
                List<string> files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Where(file => 
                        (File.GetAttributes(file) & FileAttributes.Archive) == FileAttributes.Archive && (extension == null || file.EndsWith($".{extension}")) && 
                        (nameContains == null || Path.GetFileName(file).Contains(nameContains))).ToList();

                if (files.Count == 0)
                {
                    Console.WriteLine("No important files found matching the criteria.");
                }
                else
                {
                    foreach (var file in files)
                    {
                        Console.WriteLine(Path.GetFullPath(file));
                    }
                }
            }
        }
    }
}
