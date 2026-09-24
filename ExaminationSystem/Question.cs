using ExaminationSystem;
namespace ExaminationSystem
{
    /// <summary>
    /// Base class for every question type in the exam system. Holds the question's
    /// header, body, mark, its array of possible answers, and which answer is correct.
    /// Cannot be instantiated directly — concrete question types (<see cref="TrueFalseQuestion"/>,
    /// <see cref="MCQQuestion"/>) derive from it.
    /// </summary>
    public abstract class Question : ICloneable, IComparable
    {
        protected string _header = string.Empty;
        protected string _body = string.Empty;
        protected int _mark;
        protected Answer?[] _answers = Array.Empty<Answer?>();
        protected Answer? _rightAnswer;

        /// <summary>The question's short title/label. Cannot be blank.</summary>
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

        /// <summary>The question's main text/prompt. Cannot be blank.</summary>
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

        /// <summary>How many marks this question is worth. Must be greater than zero.</summary>
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
                    throw new ArgumentOutOfRangeException(nameof(value), "Question mark must be greater than zero.");
            }
        }

        /// <summary>
        /// The fixed-size array of possible answers for this question. Empty slots are <c>null</c>.
        /// </summary>
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

        /// <summary>
        /// The answer considered correct for this question. Can only be changed via
        /// <see cref="SetRightAnswer"/>, which validates the answer belongs to <see cref="Answers"/>.
        /// </summary>
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

        /// <summary>
        /// Creates a question with its header, body and mark set, but no answers yet.
        /// </summary>
        public Question(string header, string body, int mark)
        {
            Header = header;
            Body = body;
            Mark = mark;
        }

        /// <summary>
        /// Creates a question with its header, body, mark, answer list, and correct
        /// answer all set at once. Chains to <see cref="Question(string, string, int)"/>.
        /// </summary>
        /// <param name="answersList">The full set of possible answers.</param>
        /// <param name="rightAnswer">Must be one of the answers in <paramref name="answersList"/>.</param>
        public Question(string header, string body, int mark, Answer[] answersList, Answer rightAnswer) : this(header, body, mark)
        {
            Answers = answersList;
            SetRightAnswer(rightAnswer);
        }

        /// <summary>
        /// Adds an answer into the first free slot in <see cref="Answers"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The answers array is already full.</exception>
        public void AddAnswer(Answer answer)
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

        /// <summary>
        /// Removes the given answer (matched by <see cref="Answer.AnswerId"/>) from <see cref="Answers"/>.
        /// If it was the right answer, <see cref="RightAnswer"/> is cleared as well. Does nothing if not found.
        /// </summary>
        public void RemoveAnswer(Answer answer)
        {
            ArgumentNullException.ThrowIfNull(answer);

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

        /// <summary>
        /// Marks the given answer as the correct one for this question.
        /// </summary>
        /// <exception cref="ArgumentException">The answer does not exist among <see cref="Answers"/>.</exception>
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

        /// <summary>
        /// Checks whether the given answer ID matches <see cref="RightAnswer"/>.
        /// </summary>
        public bool IsCorrect(int answerId)
        {
            return _rightAnswer is not null && _rightAnswer.AnswerId == answerId;
        }

        /// <summary>
        /// Creates a deep copy of this question: the answer array and each answer
        /// inside it are cloned independently, and <see cref="RightAnswer"/> is
        /// re-pointed at the cloned instance rather than the original.
        /// </summary>
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

        /// <summary>
        /// Compares questions by <see cref="Mark"/>.
        /// </summary>
        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (obj is not Question other)
                throw new ArgumentException("Object is not a Question.", nameof(obj));

            return _mark.CompareTo(other._mark);
        }

        /// <inheritdoc/>
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