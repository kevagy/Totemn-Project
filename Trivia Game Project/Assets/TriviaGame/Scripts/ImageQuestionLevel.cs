using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class ImageQuestionLevel : LevelController
    {
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<Sprite, string>[] _question;

        private List<KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>> _generatedQuestion = new List<KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>>();

        private KeyValuePair<QuestionTemplate<Sprite, string>, List<string>> _currentQuestion;

        protected override void OnEnable()
        {
            base.OnEnable();
            Debug.Log($"1 answerToggles {_answerOptions.Count}");
            _generatedQuestion = GenerateQuestions<Sprite, string>(_question);
            Debug.Log($"2 answerToggles {_answerOptions.Count}");
            _currentQuestionIndex = 0;
            Debug.Log($"3 answerToggles {_answerOptions.Count}");
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
            _questionImage.sprite = _currentQuestion.Key.Question;
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
            ShowResult(answer.Equals(_currentQuestion.Key.Answer));
            await Task.Delay(_timeToShowResult);
            HideResult();
            ShowQuestion(++_currentQuestionIndex);
        }

    }
}
