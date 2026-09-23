using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public class MCQQuestion : Question
    {
        public MCQQuestion(string header, string body, int mark) : base(header, body, mark)
        {
            Answers = new Answer?[4];
        }
    }
}
