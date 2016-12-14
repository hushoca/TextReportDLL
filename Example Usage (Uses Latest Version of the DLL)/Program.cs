using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextReport;

namespace DLLTestApplication
{
    class Program
    {


        static void Main(string[] args)
        {

            //2D array
            String[,] table = {
                { "Title", "Author", "Version", "Date" },
                {"A walk in the woods","George Woodland","1.0","12 Dec 2014" },
                {"A death wish","Ricky Martin","2.6","01 Jan 1991" }
            };

            //create a new table with 6 rows and 3 columns
            Table t = new Table(6, 3);

            t.addRow(new String[]{"Name","Surname","Date of Birth"});
            t.addRow(new String[] {"George", "Belkow", "01/08/1925" });

            //will be ignored because its larger then its supposed to be
            t.addRow(new String[] {"Emma", "Sparrow", "08/09/2011","Test" }); 
            t.addRow(new String[] { "Ryan", "Apple","25/02/1881" }); 

            //create a report with name text
            //will be saved as executable_directory\Test.txt
            TXTReport rep = new TXTReport("Test");

            //set character limit to 80
            rep.setLineCharacterLimit(80);

            //set the end character to space and dont remove them
            rep.setEndCharacter(' ',false);

            //add text
            rep.appendText("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque molestie orci justo, vel cursus tortor mattis vel. Praesent scelerisque est urna, ac ullamcorper turpis rhoncus nec. Etiam dapibus sollicitudin orci, sit amet porttitor ex varius at. Nulla at felis mi. Maecenas interdum lorem risus, non dapibus justo cursus quis. Donec quis dui sollicitudin, maximus turpis sed, efficitur erat. Integer feugiat varius sollicitudin. Fusce laoreet, diam quis placerat iaculis, arcu sapien feugiat mi, quis volutpat nunc diam ac lorem. Quisque suscipit erat vel nulla posuere aliquam. Nullam auctor, felis vel feugiat sollicitudin, augue lacus convallis orci, sed interdum magna augue id diam. Pellentesque efficitur turpis pharetra, fringilla nulla ac, sagittis dolor. Nullam bibendum ultrices molestie. Donec hendrerit turpis nec rhoncus rutrum.");

            //fill the next line with "#"
            rep.fillLineWith('#');

            //add the table from 2D array
            rep.addTable(table,1);

            //start a new line
            rep.breakLine();

            //add text
            rep.appendText("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque molestie orci justo, vel cursus tortor mattis vel. Praesent scelerisque est urna, ac ullamcorper turpis rhoncus nec. Etiam dapibus sollicitudin orci, sit amet porttitor ex varius at. Nulla at felis mi. Maecenas interdum lorem risus, non dapibus justo cursus quis. Donec quis dui sollicitudin, maximus turpis sed, efficitur erat. Integer feugiat varius sollicitudin. Fusce laoreet, diam quis placerat iaculis, arcu sapien feugiat mi, quis volutpat nunc diam ac lorem. Quisque suscipit erat vel nulla posuere aliquam. Nullam auctor, felis vel feugiat sollicitudin, augue lacus convallis orci, sed interdum magna augue id diam. Pellentesque efficitur turpis pharetra, fringilla nulla ac, sagittis dolor. Nullam bibendum ultrices molestie. Donec hendrerit turpis nec rhoncus rutrum.");
            
            //add table from Table object
            rep.addTable(t,8);

            //save the file and print the returned value (null if failed
            //and file path if success.
            Console.WriteLine(rep.save());

            while (true) { }
        }
    }
}
