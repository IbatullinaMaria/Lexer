using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer.TokenValue;

namespace Lexer
{
    public class Automata
    {
        public void ProcessLine(string line, int index)
        {
            switch (line[index])
            {
                case char ch when (ch == Global.whiteSpace):
                    ProcessSpace(line, index);
                    break;
                case char ch when Char.IsLetter(ch):                    
                    ProcessLetter(line, index);
                    break;
                case char val when Char.IsDigit(val):
                    ProcessDigit(line, index);
                    break;
                case char ch when Global.limiters.Contains(ch):
                    ProcessLimiter(line, index);
                    break;
                default:
                    Console.WriteLine("Unknown symbol.");
                    break;
            }

        }

        private Tuple<string, int> ReadToLimiter(string line, int index, string value) 
        {
            int i = index;
            string val = value;
            while ((i < line.Length)
                   && ((line[i] != Global.whiteSpace) && !Global.limiters.Contains(line[i])))
            {
                val += line[i].ToString();
                i++;
            }
            var pair = new Tuple<string, int>(val, i);
            return pair;
        }
        
        private void ProcessSpace(string line, int index)
        {            
            int i = index;
            bool wasProcessing = false;
            while ((i < line.Length) && (line[i] == ' '))
            {
                if (!wasProcessing) { wasProcessing = true; }
                i++;
            }
            if ((i < line.Length) && wasProcessing) { ProcessLine(line, i); }
        }

        private void ProcessLetter(string line, int index)
        {
            int i = index;
            string word = "";
            bool wasProcessing = false;
            while ((i < line.Length)
                   && ((line[i] != Global.whiteSpace) 
                       && !Global.limiters.Contains(line[i]) 
                       && !Global.mainLimiters.Contains(line[i])))
            {
                if (!wasProcessing) { wasProcessing = true; }
                word += line[i].ToString();
                i++;
            }
            if (Global.reservedWords.Contains(word) && (Global.multyCommentFlag == 0)) 
            {   
                int position = i - (word.Length - 1);
                Console.WriteLine("Keyword: " + word + ", pos number: " + position); 
            }
            else if (Global.boolValue.Contains(word) && (Global.multyCommentFlag == 0))
            {
                int position = i - (word.Length - 1);
                Console.WriteLine("Keyword: " + word + ", pos number: " + position);
            }
            else 
            {   
                if (Global.multyCommentFlag == 0)
                {
                    int position = i - (word.Length - 1); 
                    Console.WriteLine("Id: " + Keywords.Identificator + ", val: " + word + ", pos number: " + position);
                }
 
            }
            if ((i < line.Length) && wasProcessing) { ProcessLine(line, i); }
        }

        private void ProcessDigit(string line, int index)
        {
            int i = index;
            string number = "";
            bool isInteger = true;
            bool wasProcessing = false;
            while ((i < line.Length) 
                   && ((line[i] != Global.whiteSpace) && !Global.limiters.Contains(line[i]))
                   && isInteger)
            {
                switch (line[i])
                {
                    case char ch when (Char.IsLetter(ch)):
                        DetermineNumericSystem(line, i, number);
                        isInteger = false;
                        break;
                    case char ch when (Char.IsDigit(ch)):
                        number += line[i].ToString();
                        i++;
                        break;
                    case char ch when (ch == '.'):
                        ProcessDouble(line, i, number);
                        isInteger = false;
                        break;
                    default:
                        var answer = ReadToLimiter(line, i, number);
                        i = answer.Item2;
                        Console.WriteLine("Error in number: " + answer.Item1);
                        isInteger = false;
                        break;
                }
                if (!wasProcessing) { wasProcessing = true; }
            }
            if ((isInteger)&&(Global.multyCommentFlag == 0))
            {
                int position = i - (number.Length - 1);
                if (number.Length < 10 && (Global.multyCommentFlag == 0)) //проверка длины int
                {
                    Console.WriteLine("id: " + Keywords.Int + ", val: " + number + ", pos number: " + position);
                }
                else
                {
                    Console.WriteLine("Error in number: " + Keywords.Int + ", val: " + number + ", pos number: " + position);
                }
                
            }
            if ((i < line.Length) && wasProcessing && isInteger) { ProcessLine(line, i); }
        }

        private void ProcessDouble(string line, int index, string number)
        {
            int i = index;
            bool wasDot = false;
            bool isDouble = true;
            bool withExponent = false;
            while ((i < line.Length)
                   && ((line[i] != Global.whiteSpace) && !Global.limiters.Contains(line[i]))
                   && isDouble)
            {
                switch (line[i])
                {
                    case char ch when (Char.IsDigit(ch)):
                        number += ch.ToString();
                        i++;
                        break;
                    case '.':
                        if (!wasDot)
                        {
                            wasDot = true;
                            number += line[i].ToString();
                            i++;
                        }
                        else
                        {
                            var dotAnswer = ReadToLimiter(line, i, number);
                            i = dotAnswer.Item2;
                            int poSition = i - (dotAnswer.Item1.Length - 1);
                            Console.WriteLine("Error in double: " + dotAnswer.Item1 + ", pos number: " + poSition);
                            isDouble = false;
                        }
                        break;
                    case char ch when (ch.ToString() == NumericSystem.WithExponent.GetStringValue()):
                        withExponent = true;
                        ProcessNumberWithExponent(line, i, number);
                        isDouble = false;
                        break;
                    default:
                        var answer = ReadToLimiter(line, i, number);
                        i = answer.Item2;
                        number = answer.Item1;
                        int position = i - (number.Length - 1);
                        Console.WriteLine("Error in double: " + number + ", pos number: " + position);
                        isDouble = false;
                        break;
                }
            }
            if ((number[number.Length - 1] != '.') && (isDouble)) 
            {   
                int position=i - (number.Length - 1);
                Console.WriteLine("Id: " + Keywords.Double + ", val: " + number + ", pos number: " + position); 
            }
            else if ((number[number.Length - 1] == '.') && (!withExponent))
            {   
                int position = i - (number.Length - 1);
                Console.WriteLine("Error in double: " + number + ", pos number: " + position); 
            }
            if ((i < line.Length) && !withExponent) { ProcessLine(line, i); }
        }
        
        private void DetermineNumericSystem(string line, int index, string number)
        {
            switch (line[index].ToString())
            {
                case string str when (str == NumericSystem.Hexadecimal.GetStringValue()):
                    ProcessNondecimalNumber(line, index, number, NumericSystem.Hexadecimal);
                    break;
                case string str when (str == NumericSystem.Byte.GetStringValue()):
                    ProcessNondecimalNumber(line, index, number, NumericSystem.Byte);
                    break;
                case string str when (str == NumericSystem.Octal.GetStringValue()):
                    ProcessNondecimalNumber(line, index, number, NumericSystem.Octal);
                    break;
                case string str when (str == NumericSystem.WithExponent.GetStringValue()):
                    ProcessNumberWithExponent(line, index, number);
                    break;
                default:
                    var answer = ReadToLimiter(line, index, number);
                    int position = index - (number.Length - 1);
                    Console.WriteLine("Error in determine NumSys: " + answer.Item1 + ", pos number: " + position);
                    if ((answer.Item2 < line.Length)) { ProcessLine(line, answer.Item2); }
                    break;
            }
            
        }

        private void ProcessNondecimalNumber(string line, int index, string number, NumericSystem numSys)
        {
            int i = index;
            bool isNumber = true;
            if ((number.Length == 1) && (number[0] == '0'))
            {
                number += line[i].ToString();
                i++;
                while ((i < line.Length)
                       && ((line[i] != Global.whiteSpace) && !Global.limiters.Contains(line[i]))
                       && isNumber)
                {
                    bool isRequiredDigit = false;
                    switch (numSys)
                    {
                        case NumericSystem ns when 
                            ((ns == NumericSystem.Hexadecimal) || (ns == NumericSystem.Octal)):
                            isRequiredDigit = Char.IsDigit(line[i]);
                            break;
                        case NumericSystem.Byte:
                            isRequiredDigit = Global.byteDigit.Contains(line[i]);
                            break;
                    }
                    if (isRequiredDigit || (line[i] == Global.separator))
                    {
                        number += line[i].ToString();
                        i++;
                    }
                    else
                    {
                        var answer = ReadToLimiter(line, i, number);
                        i = answer.Item2;
                        int position = i - (number.Length - 1);
                        Console.WriteLine("Error in " + numSys +": " + answer.Item1 + ", pos number: " + position);
                        isNumber = false;
                    }
                }
            }
            else
            {
                var answer = ReadToLimiter(line, index, number);
                index = answer.Item2;
                int position = i - (number.Length - 1);
                Console.WriteLine("Error in " + numSys + ": " + answer.Item1 + ", pos number: " + position);
            }
            if (!Char.IsLetter(number[number.Length - 1]) && isNumber)
            {   
                int position = i - (number.Length - 1); 
                Console.WriteLine("Id: " + numSys + ", val: " + number + ", pos number: " + position); 
            }
            if ((i < line.Length)) { ProcessLine(line, i); }
        }
        
        private void ProcessNumberWithExponent(string line, int index, string number)
        {
            int i = index;
            bool isNumber = true;
            number += line[i].ToString();
            i++;
            if ((number.Length >= 1) && (number[0] != '0') && ((line[i] == '+') || (line[i] == '-')))
            {
                number += line[i].ToString();
                i++;
                while ((i < line.Length)
                       && ((line[i] != Global.whiteSpace) && !Global.limiters.Contains(line[i]))
                       && isNumber)
                {
                    if (Char.IsDigit(line[i]) || (line[i] == Global.separator))
                    {
                        number += line[i].ToString();
                        i++;
                    }
                    else
                    {
                        var answer = ReadToLimiter(line, i, number);
                        i = answer.Item2;
                        int position = i - (number.Length - 1);
                        Console.WriteLine("Error in " + NumericSystem.WithExponent +": " + answer.Item1 + ", pos number: " + position);
                        isNumber = false;
                    }
                }
            }
            else
            {
                var answer = ReadToLimiter(line, index, number);
                index = answer.Item2;
                int position = i - (number.Length - 1);
                Console.WriteLine("Error in number " + NumericSystem.WithExponent + ": " + answer.Item1 + ", pos number: " + position);
            }
            if (!Char.IsLetter(number[number.Length - 1]) && isNumber)
            {   
                int position = i - (number.Length - 1); 
                Console.WriteLine("Id: number " + NumericSystem.WithExponent + ", val: " + number + ", pos number: " + position); 
            }
            if ((i < line.Length)) { ProcessLine(line, i); }
        }
        
        public void ProcessLimiter(string line, int index)
        { 
            int i = index;
            string possibleToken = "";
            string newString = "";
            if (i + 1 < line.Length)
            {
                possibleToken = line[i].ToString() + line[i + 1].ToString();
            }
            bool complex = false;
            switch (possibleToken)
            {
                case string str when (str == Limiters.GreaterOrEqual.GetSimpleTokenString()):
                    int position = i - (str.Length - 1);
                    Console.WriteLine("Id: " + Limiters.GreaterOrEqual + ", val: " + str + ", pos number: " + position);
                    i++;
                    complex = true;
                    break;
                case string str when (str == Limiters.LessOrEqual.GetSimpleTokenString()):
                    position = i - (str.Length - 1);
                    Console.WriteLine("Id: " + Limiters.LessOrEqual + ", val: " + str + ", pos number: " + position);
                    i++;
                    complex = true;
                    break;
                case string str when (str == Limiters.Equal.GetSimpleTokenString()):
                    position = i - (str.Length - 1);
                    Console.WriteLine("Id: " + Limiters.Equal + ", val: " + str + ", pos number: " + position);
                    i++;
                    complex = true;
                    break;
                case string str when (str == Limiters.NotEqual.GetSimpleTokenString()):
                    position = i - (str.Length - 1); 
                    Console.WriteLine("Id: " + Limiters.NotEqual + ", val: " + str + ", pos number: " + position);
                    i++;
                    complex = true;
                    break;
            }
            if (!complex)
            {
                switch (line[i].ToString())
                {
                    case string ch when (ch == Limiters.LeftBracket.GetSimpleTokenString()):
                        int position = i - (ch.Length - 1);
                        Console.WriteLine("Id: " + Limiters.LeftBracket + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.RightBracket.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.RightBracket + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.CommentOneLineLimiter.GetSimpleTokenString()):
                        position = line.Length;
                        break;
                    case string ch when (ch == Limiters.CommentMultyLineLimiter.GetSimpleTokenString()):
                        i++;
                        if (Global.multyCommentFlag == 0)
                        {
                            while ((line[i] != '$') && (i < (line.Length - 1)))
                            {
                                i++;
                                if ((i == (line.Length - 1)) && (line[i] != '$'))    //если не нашли закрывающий $
                                { Global.multyCommentFlag = 1; };
                            }
                        }
                        if ((line[i-1] == '$') && (Global.multyCommentFlag == 1))
                        { 
                            Global.multyCommentFlag = 0; } //закрыли многострочный коммент
                        
                        break;

                    case string ch when (ch == Limiters.StringLimiter.GetSimpleTokenString()):
                        i++;
                        if (Global.stringFlag == 0)
                        {
                            while ((line[i] != '@') && (i < (line.Length - 1)))
                            {
                                newString += line[i];
                                i++;
                                if ((i == (line.Length - 1)) && (line[i] != '@'))    //если не нашли закрывающий @
                                { Global.stringFlag = 1; };
                                
                            }
                        }
                        if ((line[i - 1] == '@') && (Global.stringFlag == 1))
                        {
                            Global.stringFlag = 0;
                        }
                        position = i - (newString.Length-1);
                        Console.WriteLine("Id: " + Keywords.String + ", val: " + newString + ", pos number: " + position);
                        break;
                        
                    case string ch when (ch == Limiters.LeftBrace.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.LeftBrace + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.RightBrace.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.RightBrace + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.LeftParenthesis.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.LeftParenthesis + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.RightParenthesis.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.RightParenthesis + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Greater.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Greater + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Less.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Less + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Plus.GetSimpleTokenString()):
                        position = i+1; 
                        Console.WriteLine("Id: " + Limiters.Plus + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Minus.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Minus + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Assignment.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Assignment + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Not.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Not + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Multiply.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Multiply + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Divide.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Divide + ", val: " + ch + ", pos number: " + position);
                        break;
                    case string ch when (ch == Limiters.Mod.GetSimpleTokenString()):
                        position = i - (ch.Length - 1); 
                        Console.WriteLine("Id: " + Limiters.Mod + ", val: " + ch + ", pos number: " + position);
                        break;
                }
            }
            i++;
            if ((i < line.Length)&&(line[i-1]!='#')) { ProcessLine(line, i); }
        }
    }
}