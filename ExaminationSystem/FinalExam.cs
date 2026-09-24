using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public class FinalExam : Exam
    {
        public FinalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions)
        {

        }

        public override void ShowExam()
        {
            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is not null)
                {
                    Console.WriteLine(_questions[i]);
                }
            }

            Console.WriteLine($"Grade: {CalculateGrade()}");
        }


        public int CalculateGrade()
        {
            int grade = 0;
            

            for (int i = 0; i < _questions.Length; i++)
            {
                if(_questions[i] is Question question)
                {
                    if(StudentAnswers[i] != 0)
                    {
                        if (question.IsCorrect(StudentAnswers[i]))
                            grade += question.Mark;

                    }
                }
            }

            return grade;
        }

        public override void AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);


            if(question is not MCQQuestion && question is not TrueFalseQuestion) 
            {
                throw new ArgumentException("Final Exam accepts only MCQQuestion or TrueFalseQuestion.", nameof(question));
            }

            base.AddQuestion(question);
        }

        public override string ToString()
        {
            return $"Final {base.ToString()}";
        }
    }
}
