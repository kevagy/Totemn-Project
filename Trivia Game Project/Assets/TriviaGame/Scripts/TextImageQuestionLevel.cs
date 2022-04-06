using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    [System.Serializable]
    public class CombineTextImage
    {
        public string TextValue;
        public Sprite ImageValue;
    }

    public class TextImageQuestionLevel : LevelController
    {
        [SerializeField] private TMPro.TMP_Text _questionText;
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<CombineTextImage, string>[] _question;
        private System.Action _onAnswered;

        public QuestionTemplate<CombineTextImage, string>[] Question
        {
            set { _question = value; }
            get { return _question; }
        }
       
        private Dictionary<QuestionTemplate<CombineTextImage, string>, List<string>> _generatedQuestion = new Dictionary<QuestionTemplate<CombineTextImage, string>, List<string>>();
        private KeyValuePair<QuestionTemplate<CombineTextImage, string>, List<string>> _currentQuestion;

        protected override void OnEnable()
        {
            base.OnEnable();
            _generatedQuestion = GenerateQuestions<CombineTextImage, string>(_question);
            _currentQuestionIndex = 0;
            ShowQuestion(_currentQuestionIndex);
        }

        private async void ShowQuestion(QuestionTemplate<CombineTextImage, string> question, System.Action onAnswered)
        {
            if (_generatedQuestion.ContainsKey(question))
            {
                FinishQuestion();
                return;
            }

            _onAnswered = onAnswered;
            // _currentQuestionIndex = questionIndex;
            _currentQuestion =
                new KeyValuePair<QuestionTemplate<CombineTextImage, string>, List<CombineTextImage>>(question,
                    _generatedQuestion[question]);
            _questionImage.sprite = question.Question;
            await CreateAnswerOptions();
            for (int i = 0; i < _currentQuestion.Value.Count; i++)
            {
                int answerIndex = i;
                _answerOptions[i].SetupAnswer(_currentQuestion.Value[i], answerIndex, _ => { CheckResult(answerIndex); });
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
            _currentQuestion = _generatedQuestion[questionIndex];
            _questionText.text = _currentQuestion.Key.Question.TextValue;
            _questionImage.sprite = _currentQuestion.Key.Question.ImageValue;

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
