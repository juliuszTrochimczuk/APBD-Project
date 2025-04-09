namespace Controllers
{
    /// <summary>
    /// Class resposible for controlling the file operation from txt file type
    /// </summary>
    public class TxtFileController : FileController
    {
        /// <inheritdoc/>
        public TxtFileController(string filePath) : base(filePath) { }

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
