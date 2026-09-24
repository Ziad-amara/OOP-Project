using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public class PracticalExam : Exam
    {
        public PracticalExam(int time, int numberOfQuestions) :base(time, numberOfQuestions)
        {

        }

        public override void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            if(question is not MCQQuestion)
            {
                throw new ArgumentException("Practical Exam accepts only MCQQuestion.", nameof(question));
            }

            base.AddQuestion(question);
        }

        public override void ShowExam()
        {
            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is Question question)
                {
                    Console.WriteLine(question);

                    if (question.RightAnswer is not null)
                    {
                        Console.WriteLine($"Right Answer: {question.RightAnswer}");
                    }
                }
            }
        }
        public override string ToString()
        {
            return $"Practical {base.ToString()}";
        }

    }
}
