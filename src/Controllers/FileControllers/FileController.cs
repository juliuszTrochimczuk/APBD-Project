using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.FileControllers
{
    /// <summary>
    /// Abstract class resposible for controlling the file operation
    /// </summary>
    public abstract class FileController
    {
        protected string[] fileContent;

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
