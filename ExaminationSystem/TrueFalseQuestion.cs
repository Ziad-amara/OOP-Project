using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, int mark , bool correctIsTrue) : base(header,body,mark)
        {
            Answers = new Answer?[2];

            Answer trueAnswer = new Answer(1, "True");
            Answer falseAnswer = new Answer(2, "False");

            AddAnswer(trueAnswer);
            AddAnswer(falseAnswer);

            SetRightAnswer(correctIsTrue ? trueAnswer : falseAnswer);
        }
    }
}
