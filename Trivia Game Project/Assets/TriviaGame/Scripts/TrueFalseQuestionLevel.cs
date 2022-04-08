using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class TrueFalseQuestionLevel : LevelController
    {
        [SerializeField] private TMPro.TMP_Text _questionText;
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<string, bool>[] _question;
        [SerializeField] protected new List<AnswerOption> _answerOptions = new List<AnswerOption>();

        private Dictionary<QuestionTemplate<string, bool>, List<bool>> _generatedQuestion =
            new Dictionary<QuestionTemplate<string, bool>, List<bool>>();

        private KeyValuePair<QuestionTemplate<string, bool>, List<bool>> _currentQuestion;
        private System.Action _onAnswered;

        public QuestionTemplate<string, bool>[] Question
        {
            set { _question = value; }
            get { return _question; }
        }

        public async void ShowQuestion(QuestionTemplate<string, bool> question, System.Action onAnswered)
        {
            _onAnswered = onAnswered;
            _currentQuestion =
                new KeyValuePair<QuestionTemplate<string, bool>, List<bool>>(question,
                    new List<bool> {true, false});
            _questionText.text = question.Question;
            // await CreateAnswerOptions();
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(_currentQuestion.Value[i], answerIndex,
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