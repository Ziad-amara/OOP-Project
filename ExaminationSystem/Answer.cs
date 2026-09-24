namespace ExaminationSystem
{
    /// <summary>
    /// Represents a single answer choice that can be attached to a <see cref="Question"/>,
    /// identified by an ID and holding the answer's display text.
    /// </summary>
    public class Answer : ICloneable
    {
        private int _answerId;
        private string _answerText = string.Empty;

        /// <summary>
        /// The answer's unique identifier within its owning question. Must be greater than zero.
        /// </summary>
        public int AnswerId
        {
            get 
            { 
                return _answerId;
            }
            
            set 
            {
                if (value > 0)
                    _answerId = value;
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Answer ID must be greater than zero.");
            }
        }

        /// <summary>
        /// The human-readable text of the answer (e.g. "True", "Stack"). Cannot be blank.
        /// </summary>
        public string AnswerText
        {
            get
            {
                return _answerText; 
            }
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _answerText = value;
                else
                    throw new ArgumentException("Answer text cannot be null, empty, or whitespace.", nameof(value));
            }
        }

        /// <summary>
        /// Creates a new answer with the given ID and text.
        /// </summary>
        /// <param name="id">Unique identifier for this answer. Must be greater than zero.</param>
        /// <param name="text">The answer's display text. Cannot be blank.</param>
        public Answer( int id , string text)
        {
            AnswerId = id;
            AnswerText = text;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"the Answer's Id: {_answerId}, the answer's text: \"{_answerText}\"";
        }

        /// <summary>
        /// Creates an independent copy of this answer (same ID and text, distinct instance).
        /// </summary>
        public object Clone()
        {
            return new Answer(AnswerId, AnswerText);
        }
    }
}
