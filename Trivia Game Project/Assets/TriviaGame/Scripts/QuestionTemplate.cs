using System;
using System.Collections.Generic;
using UnityEngine;

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
        TrueFalse,
        PresetAnswers
    }

    public interface IQuestionData
    {
        void SetTotalQuestionInLevel(int count);
        float Quantity { get; set; }
        int QuestionCount { get; }
        SequenceQuestion Type { get; }
    }


    [Serializable]
    public class QuestionData<Q, A> : IQuestionData
    {
        [SerializeField] private SequenceQuestion _type;
        public QuestionTemplate<Q, A>[] Questions;
        public List<Sprite> Answers;

        private int _quantity;
        private const int ToPersentage = 100;
        private int _questionToShow;

        public (bool hasQuestion, QuestionTemplate<Q, A> question) GetQuestion()
        {
            int random = UnityEngine.Random.Range(0, Questions.Length - 1);
            var hasQuestion = _questionToShow > 0;
            var question = hasQuestion ? Questions[random] : null;
            _questionToShow--;
            return (hasQuestion, question);
        }

        public void SetTotalQuestionInLevel(int count)
        {
            _questionToShow = (int)Math.Round(Quantity * count);
        }

        public float Quantity
        {
            set => _quantity = (int)Math.Round((double)(Questions.Length / value * ToPersentage));

            get => _quantity;
        }

        public int QuestionCount => Questions.Length;
        public SequenceQuestion Type => _type;
    }
}