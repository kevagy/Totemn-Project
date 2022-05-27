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
        private System.Action _onAnswered;

        public QuestionTemplate<Sprite, string>[] Question
        {
            set { _question = value; }
            get { return _question; }
        }

        private Dictionary<QuestionTemplate<Sprite, string>, List<string>> _generatedQuestion =
            new Dictionary<QuestionTemplate<Sprite, string>, List<string>>();

        private KeyValuePair<QuestionTemplate<Sprite, string>, List<string>> _currentQuestion;

        protected override void OnEnable()
        {
            base.OnEnable();
            _generatedQuestion = GenerateQuestions<Sprite, string>(_question);
            _currentQuestionIndex = 0;
            // ShowQuestion(_currentQuestionIndex);
        }

        public async void ShowQuestion(QuestionTemplate<Sprite, string> question, System.Action onAnswered)
        {
            gameObject.SetActive(true);
            // if (_generatedQuestion.ContainsKey(question))
            // {
            //     FinishQuestion();
            //     return;
            // }

            _onAnswered = onAnswered;
            // _currentQuestionIndex = questionIndex;
            if (!_generatedQuestion.ContainsKey(question))
            {
                Hide();
                Debug.LogError($"Key was not found");
                return;
            }
            _currentQuestion = new KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>(question,
                _generatedQuestion[question]);

            _questionImage.sprite = question.Question;
            await CreateAnswerOptions();
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(_currentQuestion.Value[i], answerIndex,
                    _ => { CheckResult(answerIndex); });
                _answerOptions[i].SetToggleGroup(_toggleGroup);
            }
        }

        private async void ShowQuestion(int questionIndex)
        {
            if (questionIndex >= _generatedQuestion.Count)
            {
                FinishQuestion();
                return;
            }

            _currentQuestionIndex = questionIndex;
            // _currentQuestion = _generatedQuestion[questionIndex];
            _questionImage.sprite = _currentQuestion.Key.Question;
            await CreateAnswerOptions();
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(_currentQuestion.Value[i], questionIndex,
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
            await Task.Delay(_timeToShowResult);
            Hide();
            // ShowQuestion(++_currentQuestionIndex);
        }

        private void Hide()
        {
            HideResult();
            _onAnswered?.Invoke();
            gameObject.SetActive(false);
        }
    }
}