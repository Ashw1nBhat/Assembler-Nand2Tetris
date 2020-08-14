using System.Collections.Generic;
using System;


namespace Hack_Assembler
{
    class Parser
    {
        //takes the string array, removes the comments and whitespaces, then returns the list
        public List<string> RemoveComments(string[] rawData) 
        {
            int i;

            var noComments = new List<string>();  //Empty list to store the new strings

            foreach (string line in rawData)   //loop to iterate over the strings and check for comments
            { 
                if (line.Contains("//"))
                {
                    i = line.IndexOf('/');
                    noComments.Add(line.Substring(0, i));  //Removing the comment part
                }
                else
                    noComments.Add(line);
            }

            noComments.RemoveAll(item => string.IsNullOrEmpty(item));  //Removing all blank strings

            //loop for removing white spaces
            for (i = 0; i < noComments.Count; i++)
                noComments[i] = string.Join(" ", noComments[i].Split(default(string[]),StringSplitOptions.RemoveEmptyEntries));

            noComments = AddLabels(noComments);

            return noComments;
        }

        //Loops over all of the strings and adds the labels to a dictionary
        public List<string> AddLabels(List<string> rawData)
        {
            int labelCount = 0, i, len;
            string label, labelValue;

            for (i = 0; i < rawData.Count; i++)
            {
                if (rawData[i].StartsWith('('))
                {
                    label = rawData[i].Substring(1, rawData[i].Length - 2);  //label without the parenthesis
                    labelValue = Convert.ToString(i - labelCount, 2);  //value pointed by the label
                    len = 16 - labelValue.Length;
                    labelValue = new string('0', len) + labelValue;
                    SymbolTable.labels.Add(label, labelValue);  //adding the label to the symbol table after binary conversion
                    labelCount++;
                }
            }

            rawData.RemoveAll(item => item.Contains('('));  
        
            return rawData;
        }
    }
}
