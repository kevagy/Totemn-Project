using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TriviaGame.Scripts
{
    public class TextQuestionLevel : LevelController
    {
        [SerializeField] private TMPro.TMP_Text _questionText;
        [SerializeField] private QuestionTemplate<string, string>[] _question;

        private List<KeyValuePair<QuestionTemplate<string, string>, List<string>>> _generatedQuestion;
        private KeyValuePair<QuestionTemplate<string, string>, List<string>> _currentQuestion;

        protected override void OnEnable()
        {
            base.OnEnable();
            _answerOptions = new List<AnswerOption>();
            _generatedQuestion = GenerateQuestions<string, string>(_question);
            ShowQuestion(_currentQuestionIndex);
        }

        private async void ShowQuestion(int questionIndex)
        {
            if (questionIndex >= _generatedQuestion.Count)
            {
                FinishQuestion();
                return;
            }
            _currentQuestionIndex = questionIndex;
            _currentQuestion = _generatedQuestion[questionIndex];
            _questionText.text = _currentQuestion.Key.Question;
            await CreateAnswerOptions();
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(_currentQuestion.Value[i], questionIndex, _ => { CheckResult(answerIndex); });
                _answerOptions[i].SetToggleGroup(_toggleGroup);
            }
        }

        private async void CheckResult(int answerIndex)
        {
            var answer = _currentQuestion.Value[answerIndex];
            bool isCorrect = answer.Equals(_currentQuestion.Key.Answer);
            ShowResult(isCorrect);
            AddScore(isCorrect ? 10 : -1 * _currentQuestion.Key.ScorePerQ);
            await Task.Delay(_timeToShowResult);
            HideResult();
            ShowQuestion(++_currentQuestionIndex);
        }
    }
}