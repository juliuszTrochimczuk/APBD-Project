namespace Project
{
    public class FileController
    {
        private readonly string filePath;
        private string[] fileContent;

        public FileController(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                Console.WriteLine("New file has been created");
            }
            this.filePath = filePath;
            fileContent = File.ReadAllLines(filePath);
        }

        public int FileLinesCount() => fileContent.Length;

        public string GetFileLine(int line) => fileContent[line];

        public void SaveToFile(string newText)
        {
            File.WriteAllText(filePath, newText);
            fileContent = newText.Split('\n');
        }
    }
}
