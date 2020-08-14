using System;
using System.Collections.Generic;


namespace Hack_Assembler
{
    class Code
    {
        //Method to convert A-intructions to their equivalent binary form
        public List<string> Ainstructions(List<string> rawData)
        {
            int count = 16;    //Keeps count of the value of new variables
            string variable,newVar,actualVar;
            int len;
            bool isNumber;

            for (int i=0;i<rawData.Count;i++)
            {
                variable = rawData[i].Substring(1, rawData[i].Length - 1);  //temp string to store instruction w/o @

                if (rawData[i].StartsWith('@') && SymbolTable.labels.ContainsKey(variable))
                {
                    rawData[i] = SymbolTable.labels[variable];
                }
                else if(rawData[i].StartsWith('@'))
                {
                    isNumber = int.TryParse(variable, out int _);  //bool to check whether the variable is a number
                    if (isNumber)
                    {
                        rawData[i] = Convert.ToString(Convert.ToInt32(variable, 10), 2);  //Converting to binary
                        len = 16 - rawData[i].Length;
                        rawData[i] = new string('0', len) + rawData[i];   //adding the remaining zeros 
                    }
                    else
                    {
                        newVar = Convert.ToString(count,2);  //converting the value of the variable to binary
                        len = 16 - newVar.Length;
                        actualVar = new string('0', len) + newVar;  //adding remaining zeros
                        rawData[i] = actualVar;
                        SymbolTable.labels.Add(variable, actualVar);  //adding variable to the symbol table
                        count++;
                    }
                }
            }
            return rawData;
        }

        //Method to convert C-instructions to their equivalent binary form
        public List<string> Cinstructions(List<string> rawData)
        {
            string dest, comp, jump;  //temp variables to store dest,comp and jump values
            string[] destCompJump, CompJump, destComp, destJump; //temp arrays to store temp,comp,jump values

            for (int i=0;i<rawData.Count;i++)
            {
                if(rawData[i].Contains('=') && rawData[i].Contains(';'))   //comp,dest and jump are all present
                {
                    destCompJump = rawData[i].Split('=');  //temp array to store dest,comp and jump
                    CompJump = destCompJump[1].Split(';');  //temp array to store comp and jump

                    //getting the values of dest,comp and jump from symbol table
                    dest = SymbolTable.dest[destCompJump[0]];  
                    comp = SymbolTable.comp[CompJump[0]];
                    jump = SymbolTable.jump[CompJump[1]];

                    //combining the values
                    rawData[i] = "111" + comp + dest + jump;
                }
                else if(rawData[i].Contains('='))  //comp and dest are present
                {
                    destComp = rawData[i].Split('=');

                    //getting the values of dest and comp from symbol table
                    dest = SymbolTable.dest[destComp[0]];
                    comp = SymbolTable.comp[destComp[1]];

                    //combining the values
                    rawData[i] = "111" + comp + dest + "000";
                }
                else if(rawData[i].Contains(';'))  //dest and jump are present
                {
                    destJump = rawData[i].Split(';');  //temp array to store comp and jump

                    //getting the values of dest and jump from symbol table
                    dest = SymbolTable.comp[destJump[0]];
                    jump = SymbolTable.jump[destJump[1]];

                    //combining the values
                    rawData[i] = "111" + dest + "000" + jump;
                }
            }
            return rawData;
        }

    }
}
