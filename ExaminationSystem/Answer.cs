namespace ExaminationSystem
{
    public class Answer : ICloneable
    {
        private int _answerId;
        private string _answerText = string.Empty;

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

        public Answer( int id , string text)
        {
            AnswerId = id;
            AnswerText = text;
        }

        public override string ToString()
        {
            return AnswerText;
        }

        public object Clone()
        {
            return new Answer(AnswerId, AnswerText);
        }
    }
}
