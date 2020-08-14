using System;
using System.Collections.Generic;
using System.IO;


namespace Hack_Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            //set the path of the input file
            string inputPath = @"input_path";

            //getting the text from the file as an array of strings
            string[] rawData = System.IO.File.ReadAllLines(inputPath); 

            Parser p = new Parser();    
            var parsedList = new List<string>();
            parsedList = p.RemoveComments(rawData);

            Code c = new Code();
            var output = new List<string>();
            output = c.Cinstructions(c.Ainstructions(parsedList));

            //set the path of the output file
            string outputPath = @"output_path";

            //writing the text to a new file
            File.WriteAllLines(outputPath, output);
        }
    }
}
