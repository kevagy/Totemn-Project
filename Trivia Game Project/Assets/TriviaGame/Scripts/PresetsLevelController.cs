using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class PresetsLevelController : LevelController
    {
        [SerializeField] private TMPro.TMP_Text _questionText;
        [SerializeField] private Image _questionImage;

        [SerializeField] private QuestionTemplate<string, int>[] _question;
        // [SerializeField] private new List<AnswerOption> _answerOptions = new List<AnswerOption>();

        private Dictionary<QuestionTemplate<string, int>, List<int>> _generatedQuestion =
            new Dictionary<QuestionTemplate<string, int>, List<int>>();

        private KeyValuePair<QuestionTemplate<string, int>, List<int>> _currentQuestion;
        private System.Action _onAnswered;

        public QuestionTemplate<string, int>[] Question
        {
            set { _question = value; }
            get { return _question; }
        }

        public List<Sprite> AnswerSprites;

        public void ShowQuestion(QuestionTemplate<string, int> question, System.Action onAnswered)
        {
            _onAnswered = onAnswered;
            var answers = new List<int>();
            for (int i = 0; i < _answerOptions.Count; i++)
            {
                answers.Add(i);
            }

            _currentQuestion =
                new KeyValuePair<QuestionTemplate<string, int>, List<int>>(question, answers);
            _questionText.text = question.Question;
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(AnswerSprites[i], answerIndex,
                    _ => { CheckResult(answerIndex); });
                _answerOptions[i].SetToggleGroup(_toggleGroup);
            }
        }

        private async void CheckResult(int answerIndex)
        {
            var answer = _currentQuestion.Value[answerIndex];
            bool isCorrect = answer.Equals(_currentQuestion.Key.Answer);
            ShowResult(isCorrect);
            AddScore(isCorrect ? 10 : -1 * _currentQuestion.Key.ScorePerQ);
            await UniTask.Delay(_timeToShowResult);
            HideResult();
            _onAnswered?.Invoke();
        }
    }
}