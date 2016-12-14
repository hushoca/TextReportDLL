using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TextReport
{
    public class TXTReport
    {

        internal String     _fileName           = "";
        internal String     _filePath           = "";
        internal String     _reportBuffer       = "";
        internal int        _charlimit          = 0;
        internal char       _endChar            = ' ';
        internal Boolean    _removeEndChar      = true;

        /// <summary>
        /// Initialises the TXTReport object which will be used to create
        /// the report.
        /// </summary>
        /// <param name="FileName">Filename which will be created</param>
        public TXTReport(String FileName)
        {
            this._filePath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location).ToString();
            this._fileName = FileName;
        }

        /// <summary>
        /// Initialises the TXTReport object which will be used to create
        /// the report.
        /// </summary>
        /// <param name="FileName">Filename which will be created</param>
        /// <param name="FilePath">Path which the report will be in</param>
        public TXTReport( String FilePath,String FileName)
        {
            this._fileName = FileName;
            this._filePath = FilePath;
        }

        /// <summary>
        /// Determines how many characters can be in one line. When the limit is
        /// reached and end character is found it just creates a new line and continues.
        /// End character is space by default. Can be changed using setEndCharacter()
        /// </summary>
        /// <param name="CharCount">Character limit per line</param>
        public void setLineCharacterLimit(int CharCount)
        {
            this._charlimit = CharCount;
        }

        /// <summary>
        /// Determines which char will be the last char in the line. After this char
        /// jumps to the new line (if chararacter limit is reached.)
        /// </summary>
        /// <param name="chr">breaks line after this character (if charlimit is reached)</param>
        /// <param name="remove">if true removes the character before jumping to new line</param>
        public void setEndCharacter(char chr, Boolean remove) {
            this._endChar = chr;
            this._removeEndChar = remove;
        }

        /// <summary>
        /// Change the filename.
        /// </summary>
        /// <param name="FileName">New filename</param>
        public void setName(String FileName)
        {
            this._fileName = FileName;
        }

        /// <summary>
        /// Change the filepath.
        /// </summary>
        /// <param name="FilePath">New File path</param>
        public void setPath(String FilePath)
        {
            this._filePath = FilePath;
        }

        /// <summary>
        /// Returns the current file name.
        /// </summary>
        /// <returns>Filename</returns>
        public String getFileName() {
            return this._fileName;
        }

        /// <summary>
        /// Returns the current file path.
        /// </summary>
        /// <returns>File Path</returns>
        public String getPath() {
            return this._filePath;
        }

        /// <summary>
        /// Clears the entire report.
        /// </summary>
        public void clear()
        {
            this._reportBuffer = "";
        }

        /// <summary>
        /// Set the report buffer to String.
        /// </summary>
        /// <param name="text">String which will be equal to report</param>
        public void set(String text)
        {
            this._reportBuffer = text;
        }

        /// <summary>
        /// Return entire report as String.
        /// </summary>
        /// <returns>String (Full Report)</returns>
        public String toString()
        {
            return this._reportBuffer;
        }

        /// <summary>
        /// Appends String to the report.
        /// </summary>
        /// <param name="text">Text to be appended.</param>
        public void appendText(String text)
        {
            if (this._charlimit != 0)
            {
                int i = 1;
                foreach(Char chr in text)
                {
                    if (i >= _charlimit && chr == _endChar)
                    {
                        if (!_removeEndChar) this._reportBuffer += chr;
                        this._reportBuffer += "\n";
                        i = 1;
                    }
                    else
                    {
                        this._reportBuffer += chr;
                        i++;
                    }
                }
            }else
            {
                this._reportBuffer += text;
            }
        }

        /// <summary>
        /// Fills the next line with the chosen character.
        /// (When called, jumps to a new line fills the line then jumps to a new line again)
        /// </summary>
        /// <param name="chr">Character which will fill the line</param>
        public void fillLineWith(char chr) { 
            this._reportBuffer += "\n";
            for (int i = 0; i < _charlimit; i++) {
                this._reportBuffer += chr;
            }
            this._reportBuffer += "\n";
        }

        /// <summary>
        /// Starts a new line (Really unnecessary but why not...)
        /// </summary>
        public void breakLine()
        {
            this._reportBuffer += "\n";
        }

        /// <summary>
        /// Saves the report as a text file to previously chosen filepath
        /// with previously chosen filename.
        /// </summary>
        /// <returns>Failed = Returns null / Success = Returns full filepath</returns>
        public String save()
        {
            String path = "";

            // get the position of the last char
            int fpLength = _filePath.Length - 1;

            //if last char is not a \ then add a \ to the path
            path = (_filePath[fpLength] == '\\')? _filePath : _filePath + "\\";

            //combine path and filename and .txt
            String _final = path + _fileName + ".txt";

            //init streamwriter
            StreamWriter file = null;

            try
            {
               //attempt to write to the file
               file = new StreamWriter(_final);
            }catch(Exception e){
                //if exception caught return null
                return null;
            }
            //if successful

            //write the buffer to the file
            file.Write(_reportBuffer);

            //flush the file
            file.Flush();

            //close the file.
            file.Close();

            //return the file path
            return _final;
        }

        /// <summary>
        /// Adds a table to the report.
        /// </summary>
        /// <param name="tableArray">2D String array which will be turned into the table.</param>
        /// <param name="whiteSpaceCount">Number of spaces from left of the document</param>
        public void addTable(String[,] tableArray, int whiteSpaceCount) {
            String table = createTable(tableArray, whiteSpaceCount);
            _reportBuffer += table;
        }

        /// <summary>
        /// Add a table to the report.
        /// </summary>
        /// <param name="t">Table to be added to the report</param>
        /// <param name="whiteSpaceCount">Number of spaces from left of the document</param>
        public void addTable(Table t,int whiteSpaceCount)
        {
            if(t != null)
            {
                String table = createTable(t.to2DArray(), whiteSpaceCount);
                _reportBuffer += table;
            }
        }

        /// <summary>
        /// Yields an array which contains the maximum sizes of each column.
        /// </summary>
        /// <param name="array">2D String array which needs to be measured</param>
        /// <returns>Array containing max sizes</returns>
        internal static int[] getMaxCharacterSize(String[,] array)
        {
            //get the size of the rows and columns
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            //init a new int array
            int[] columnSize = new int[columns];

            for (int c = 0; c < columns; c++)//for each column do the following
            {
                //init a variable which will keep the largest item size
                int maxSize = 0;

                //go through each item in the column
                for (int r = 0; r < rows; r++)
                {
                    /*if the item is not null
                     *trying to .Length of a null throws an exception. (so this
                     *prevents an exception)
                     */
                    if (array[r, c] != null)
                    {
                        //get the item size
                        int itemSize = array[r, c].Length;

                        /*check if the item size is bigger than the previous items if so
                         * change the maxSize variable to this item's size
                         */
                        maxSize = (itemSize > maxSize) ? itemSize : maxSize;
                    }
                }

                //place the maxSize of the column into the array.
                columnSize[c] = maxSize;
            }

            //return the array which contains maxSize of each column.
            return columnSize;
        }

        /// <summary>
        /// Draws a line with the X size.
        /// </summary>
        /// <param name="length">Size of the line</param>
        internal static String drawLine(int length, int whiteSpaceCount)
        {
            String lineBuffer = "";
            //breakline and add 3 spaces.
            lineBuffer += "\n";
            for (int wS = 0; wS < whiteSpaceCount; wS++) lineBuffer += " ";

            //add - signs depending on the size
            for (int line = 0; line < length + 1; line++)
            {
                lineBuffer += "-";
            }

            //breakline.
            lineBuffer += "\n";
            return lineBuffer;
        }

        /// <summary>
        /// Creates a table from the 2D String array.
        /// </summary>
        /// <param name="array">2D String Array which will be turned into the table</param>
        internal static String createTable(String[,] array,int whiteSpaceCount)
        {
            String tableBuffer = "";

            //get the size of the array
            int[] columnSize = getMaxCharacterSize(array);

            //get the row and column count of the array
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            //init a new int variable
            int t = 0;

            ///get total size of characters in the array columns
            foreach (int i in columnSize) t += i;

            //for each column add 4 characters (spaces and | characters)
            //and then add the total character count previously calculated
            int totalCharacterPerLine = (columnSize.Length * 4) + t;

            //draw the top line of the table
            tableBuffer += drawLine(totalCharacterPerLine,whiteSpaceCount);

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)//for each row do the following:
            {
                //Write the begining
                for (int wS = 0; wS < whiteSpaceCount; wS++) tableBuffer += " ";
                tableBuffer += "|";

                for (int colIndex = 0; colIndex < columns; colIndex++)//for each item in the rows do the following:
                {
                    //check if the item is null. if not get its size, if its null set the size to 0.
                    int charSize = (array[rowIndex, colIndex] != null) ? array[rowIndex, colIndex].Length : 0;
                    /*get the column size of that column and 
                     *subtract the character size then add 2 
                     *for whitespaces
                     */
                    int tabCount = columnSize[colIndex] - charSize + 2;

                    //add the item to the cell
                    tableBuffer += " " + array[rowIndex, colIndex];

                    //fill the rest of the cell with whitespaces
                    for (int i = 0; i < tabCount; i++) tableBuffer += " ";

                    //end the cell
                    tableBuffer += "|";
                }

                //draw bottom line of the row
                tableBuffer += drawLine(totalCharacterPerLine, whiteSpaceCount);

            }

            return tableBuffer;
        }

    }
    
    public class Table
    {
        internal String[,] _table;
        internal int rPos = 0;
        internal int rSize = 0;
        internal int cSize = 0;

        /// <summary>
        /// Init a table with row and column size
        /// </summary>
        /// <param name="rowSize">Number of rows</param>
        /// <param name="columnSize">Number of columns</param>
        public Table(int rowSize, int columnSize)
        {
            this.rSize = rowSize;
            this.cSize = columnSize;
            this._table = new String[rowSize, columnSize];
        }

        /// <summary>
        /// Write to the next row.
        /// </summary>
        /// <param name="items">Cells to be writen</param>
        /// <returns>False if write fails, True if write is successful</returns>
        public Boolean addRow(String[] items)
        {
            int arrayLen;

            //check to make sure array is not null
            if (items != null) {
                //get the array length
                 arrayLen = items.Length;
            }else {
                //return false if array is null
                return false;
            }

            //check current row position is smaller than the row size
            //so we dont get an out of index exception
            if(this.rPos <  this.rSize)
            {

                //make sure array has less or equal items than to rows
                //to prevent out of index exception
                if (cSize >= arrayLen)
                {
                    //for each item in the array write it to a cell in the row
                    int i = 0;
                    foreach(String s in items)
                    {
                        _table[this.rPos, i] = s;
                        i++;
                    }

                    //increment row count (so that when addRow is called again
                    //it writes to the next row)
                    this.rPos++;
                    return true;

                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts table to a 2D String array.
        /// </summary>
        /// <returns>2D String Array</returns>
        public String[,] to2DArray()
        {
            return this._table;
        }
           
    }
}
