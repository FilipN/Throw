using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Throw.Model;

namespace Throw.SessionData
{
    public class Code
    {
        public List<Line> Lines { get; set; } = new List<Line>();
        public Code(string codeContent)
        {
            string[] lines = codeContent.Split("\n");
            foreach (string item in lines)
            {
                Line line = new Line() { Content = item };
                Lines.Add(line);
            }
        }

        public bool addUser (string usr)
        {
            if (!Users.Contains(usr))
                Users.Add(usr);
            return true;
        }

        public bool changeLine(int lineNumber,string content)
        {
            //proveriti da li je novi red ubacen ili je slovo
            return true;
        }

        public string getCode()
        {
            string code = "";
            foreach (Line line in Lines)
            {
                code += line.Content;
            }
            return code;
        }

        public void addLine(string content)
        {
            int lastlinenum = Lines[Lines.Count - 1].NumberOfLine;
            Lines.Add(new Line { Content = content, LastModified = DateTime.Now, NumberOfLine = lastlinenum + 1 });
        }

        public void deleteLine(int linenum)
        {
            Lines.RemoveAt(linenum);
        }

        public List<string> Users { get; set; }
    }

    public class Line
    {
        public int NumberOfLine { get; set; }
        public string Content { get; set; }

        public DateTime LastModified { get; set; }

        public User UserModifed { get; set; }
    }
}
