using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// Abstract class resposible for controlling the file operation
    /// </summary>
    public abstract class FileController
    {
        protected readonly string filePath;
        protected string[] fileContent;

        /// <summary>
        /// Checks if the File Exists (if not then creates new file with approprate output to console). Reads from file to private variable
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        protected FileController(string filePath)
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
        public abstract int FileLinesCount();

        /// <summary>
        /// Returns specific line in file
        /// </summary>
        /// <param name="line">index of the line in text</param>
        /// <param name="content">Content of that line</param>
        /// <returns>Returns bool that shows if asked line exists</returns>
        public abstract bool GetFileLine(int line, out string content);

        /// <summary>
        /// Saves text to file and overrides active content
        /// </summary>
        /// <param name="newText">Text that will be written to the file</param>
        public abstract void SaveToFile(string newText);
    }
}
