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
        [SerializeField] private QuestionTemplate< CombineTextImage, string>[] _question;

        private List<KeyValuePair<QuestionTemplate<CombineTextImage, string>, List<string>>> _generatedQuestion = new List<KeyValuePair<QuestionTemplate<CombineTextImage, string>, List<string>>>();

        private KeyValuePair<QuestionTemplate<CombineTextImage, string>, List<string>> _currentQuestion;

        protected override void OnEnable()
        {
            base.OnEnable();
            _answerOptions = new List<AnswerOption>();
            _generatedQuestion = GenerateQuestions<CombineTextImage, string>(_question);
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
            _questionText.text = _currentQuestion.Key.Question.TextValue;
            //todo setup image for question
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
