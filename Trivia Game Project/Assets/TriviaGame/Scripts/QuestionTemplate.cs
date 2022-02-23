using System;
namespace TriviaGame.Scripts
{
    [Serializable]
    public class QuestionTemplate<Q, A>
    {
        public Q Question;
        public A Answer;
        public bool IsAnswerOption;
        public float ScorePerQ = 10f;

    }

    [Serializable]
    public enum SequenceQuestion
    {
        Text,
        Image,
        TextImage,
        TrueFalse

    }


    [Serializable]
    public class QuestionData <Q, A>
    {
        public SequenceQuestion Type;
        public QuestionTemplate<Q, A>[] Questions;
    }

}