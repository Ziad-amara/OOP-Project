using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public abstract class Exam : ICloneable, IComparable
    {
        protected int _time;
        protected int _numberOfQuestions;
        protected Question?[] _questions = Array.Empty<Question?>();
        protected int[] _studentAnswers = Array.Empty<int>();

        public int Time
        {
            get
            {
                return _time;
            }

            set
            {
                if (value > 0)
                    _time = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Time must be greater than zero.");
            }
        }

        public int NumberOfQuestions
        {
            get
            {
                return _numberOfQuestions;
            }

            set
            {
                if (value > 0)
                    _numberOfQuestions = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Number of questions must be greater than zero.");
            }
        }

        public Question?[] Questions
        {
            get
            {
                return _questions;
            }

            protected set
            {
                _questions = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public int[] StudentAnswers
        {
            get
            {
                return _studentAnswers;
            }

            protected set
            {
                _studentAnswers = value ?? throw new ArgumentNullException(nameof(value));
            }
        }


        public Exam(int time, int numberOfQuestions)
        {
            Time = time;
            NumberOfQuestions = numberOfQuestions;


            _questions = new Question?[NumberOfQuestions];
            _studentAnswers = new int[NumberOfQuestions];

        }


        public virtual void AddQuestion(Question question)
        {

            ArgumentNullException.ThrowIfNull(question);

            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is null)
                {
                    _questions[i] = (Question)question.Clone();
                    return;
                }
            }

            throw new InvalidOperationException("The questions array is full.");

        }

        public void SetStudentAnswer(int questionIndex, int answerId)
        {
            if (answerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(answerId), "Answer ID must be greater than zero.");


            if (questionIndex < 0 || questionIndex >= _questions.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(questionIndex), "Question index is out of range");
            }

            Question? question = _questions[questionIndex];

            if (question is null)
                throw new InvalidOperationException("There is no question at this specified index.");

            bool answerExists = question.Answers.Any(a => a is not null && a.AnswerId == answerId);
            if (!answerExists)
                throw new ArgumentException("The specified answer does not belong to this question.", nameof(answerId));

            StudentAnswers[questionIndex] = answerId;

        }

        public abstract void ShowExam();

        public override string ToString()
        {
            return $"Exam - Time: {Time} minutes, Questions: {NumberOfQuestions}";

        }

        public object Clone()
        {
            Exam clone = (Exam)MemberwiseClone();

            clone._questions = new Question?[_questions.Length];

            for (int i = 0; i < _questions.Length; i++)
            {
                if (_questions[i] is not null)
                {
                    clone._questions[i] = (Question)_questions[i]!.Clone();
                }
            }

            clone._studentAnswers = (int[])_studentAnswers.Clone();

            return clone;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
                return 1;

            if (obj is not Exam other)
                throw new ArgumentException("Object is not an Exam.", nameof(obj));

            return NumberOfQuestions.CompareTo(other.NumberOfQuestions);
        }
    }
}
