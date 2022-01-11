using System;
namespace TriviaGame.Scripts
{
    [Serializable]
    public class QuestionTemplate<Q,A>
    {
        public Q Question;
        public A Answer;
        public bool IsCorrect;
        public bool IsAnswerOption;
    }
}
