namespace Project
{
    /// <summary>
    /// Class resposible for controlling the file operation
    /// </summary>
    public class FileController
    {
        private readonly string filePath;
        private string[] fileContent;

        /// <summary>
        /// Checks if the File Exists (if not then creates new file with approprate output to console). Reads from file to private variable
        /// </summary>
        /// <param name="filePath">Path to the file</param>
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

        /// <summary>
        /// Gives the length of the file content
        /// </summary>
        public int FileLinesCount() => fileContent.Length;

        /// <summary>
        /// Returns specific line in file
        /// </summary>
        /// <param name="line">index of the line in text</param>
        /// <param name="content">Content of that line</param>
        /// <returns>Returns bool that shows if asked line exists</returns>
        public bool GetFileLine(int line, out string content) 
        {
            try
            {
                content = fileContent[line];
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                content = null;
                return false;
            }
        }

        /// <summary>
        /// Saves text to file and overrides active content
        /// </summary>
        /// <param name="newText">Text that will be written to the file</param>
        public void SaveToFile(string newText)
        {
            File.WriteAllText(filePath, newText);
            fileContent = newText.Split('\n');
        }
    }
}
