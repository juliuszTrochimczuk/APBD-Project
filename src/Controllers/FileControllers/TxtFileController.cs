namespace Controllers.FileControllers
{
    /// <summary>
    /// Class resposible for controlling the file operation from txt file type
    /// </summary>
    public class TxtFileController : FileController
    {
        private readonly string filePath;

        /// <summary>
        /// Check if file exists and if not create one, then read all the lines
        /// </summary>
        /// <param name="filePath">Path to the txt file</param>
        public TxtFileController(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                Console.WriteLine("New file has been created");
            }
            this.filePath = filePath;
            fileContent = File.ReadAllLines(filePath);
        }

        /// <inheritdoc/>
        public override int FileLinesCount() => fileContent.Length;

        /// <inheritdoc/>
        public override bool GetFileLine(int line, out string content)
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

        /// <inheritdoc/>
        public override void SaveToFile(string newText)
        {
            File.WriteAllText(filePath, newText);
            fileContent = newText.Split('\n');
        }
    }
}
