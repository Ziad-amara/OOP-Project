using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem
{
    public abstract class Question : ICloneable, IComparable
    {
        protected string _header = string.Empty;
        protected string _body = string.Empty; 
        protected int _mark;
        protected Answer?[] _answers =Array.Empty<Answer?>();
        protected Answer? _rightAnswer;

        public string Header
        {

            get
            {
                return _header;
            }

            set
            {

                if (!string.IsNullOrWhiteSpace(value))
                    _header = value;
                else
                    throw new ArgumentException("Question header cann't be empty", nameof(value));
                
            }
        }

        public string Body
        {

            get
            {
                return _body;
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _body = value;
                else
                    throw new ArgumentException("Question body cann't be empty", nameof(value));
            }
        }

        public int Mark
        {

            get
            {
                return _mark;
            }

            set
            {

                if (value > 0)
                    _mark = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value),"Question mark must be greater than zero.");
           
            }
        }

        public Answer?[] Answers
        {
            get
            {
                return _answers;
            }

            protected set
            {
                _answers = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public Answer? RightAnswer
        {
            get
            {
                return _rightAnswer;
            }
            private set
            {
                _rightAnswer = value;
            }
        }

        public Question(string header , string body , int mark)
        {
            Header = header;
            Body = body;
            Mark = mark;
        }

        public Question(string header, string body, int mark , Answer[] answersList , Answer rightAnswer) : this (header,body,mark)
        {
            Answers = answersList;
            SetRightAnswer(rightAnswer);
        }

        public void AddAnswer (Answer answer)
        {

            ArgumentNullException.ThrowIfNull(answer);

            for (int i = 0; i < _answers.Length; i++)
            {
                if (_answers[i] is null)
                {
                    _answers[i] = answer;
                    return;
                }
            }

            throw new InvalidOperationException("The answers array is full.");

        }

        public void RemoveAnswer(Answer answer)
        {
            
            for (int i = 0; i < _answers.Length; i++)
            {

                Answer? currentAnswer = _answers[i];

                if (currentAnswer is not null && currentAnswer.AnswerId == answer.AnswerId)
                {
                    if (_rightAnswer?.AnswerId == answer.AnswerId)
                        _rightAnswer = null;

                    _answers[i] = null;
                    return;
                }

            }
        }

        public void SetRightAnswer(Answer answer)
        {
            ArgumentNullException.ThrowIfNull(answer);

            for (int i = 0; i < _answers.Length; i++)
            {
                Answer? currentAnswer = _answers[i];

                if (currentAnswer is not null && currentAnswer.AnswerId == answer.AnswerId)
                {
                    _rightAnswer = _answers[i];
                    return;
                }
            }

            throw new ArgumentException("The specified answer does not exist in this question.", nameof(answer));
        }

        public bool IsCorrect(int answerId)
        {
            return _rightAnswer is not null && _rightAnswer.AnswerId == answerId;
        }

        public object Clone()
        {
            Question clone = (Question)MemberwiseClone();

            Answer?[] clonedAnswers = new Answer?[_answers.Length];

            for (int i = 0; i < _answers.Length; i++)
            {
                clonedAnswers[i] = _answers[i] is null ? null : (Answer)_answers[i]!.Clone();
            }

            clone._answers = clonedAnswers;

            if (_rightAnswer is not null)
            {
                int idx = Array.FindIndex(_answers, a => a is not null && a.AnswerId == _rightAnswer.AnswerId);
                clone._rightAnswer = idx >= 0 ? clonedAnswers[idx] : null;
            }

            return clone;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is not Question other)
                throw new ArgumentException("Object is not a Question.", nameof(obj));

            return _mark.CompareTo(other._mark);
        }

        public override string ToString()
        {
            string result = $"{_header}: {_body}\n";

            for (int i = 0; i < Answers.Length; i++)
            {
                if (Answers[i] is not null)
                    result += $"{i + 1}) {Answers[i]}\n";
            }

            return result;
        }

       
    }
}
