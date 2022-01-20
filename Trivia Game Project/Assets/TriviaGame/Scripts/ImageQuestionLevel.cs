using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;


namespace TriviaGame.Scripts
{

    public class ImageQuestionLevel : LevelController
    {
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<Sprite, string>[] _question;
        [SerializeField] private int _numberOfQuestionToShow = 12;
        [SerializeField] private int _nubmerOfAnswers = 4;
        private List<KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>> _generatedQuestion = new List<KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>>();

        private KeyValuePair<QuestionTemplate<Sprite, string>, List<string>> _currentQuestion;
        private int _currentQuestionIndex = 0;
        private List<AnswerOption> _answerOptions;

        protected override void OnEnable()
        {
            base.OnEnable();
            _answerOptions = new List<AnswerOption>();
            GenerateQuestions();
            ShowQuestion(_currentQuestionIndex);
        }

        protected override void GenerateQuestions()
        {
            var random = new System.Random();

            var randomList = _question.OrderBy(item => random.Next()).ToList();
            for (int i = 0; i < _numberOfQuestionToShow; i++)
            {
                List<string> answers = new List<string>();
                answers.Add(randomList[i].Answer);
                for (int j = answers.Count; j < _nubmerOfAnswers; j++)
                {
                    var num = i + j;
                    if (num >= randomList.Count)
                    {
                        num = 0;
                    }
                    answers.Add(randomList[num].Answer);
                }
                answers = answers.OrderBy(item => random.Next()).ToList();
                _generatedQuestion.Add(new KeyValuePair<QuestionTemplate<Sprite, string>, List<string>>(randomList[i], answers));
            }
        }

        private async void ShowQuestion(int questionIndex)
        {
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

        private async Task CreateAnswerOptions()
        {
            if (_nubmerOfAnswers == _answerOptions.Count)
            {
                return;
            }

            for (int i = 0; i < _nubmerOfAnswers; i++)
            {
                AnswerOption answerOption = Instantiate(_answerOptionPrefab, _toggleGroup.transform);
                _answerOptions.Add(answerOption);
                await Task.Yield();
            }
        }

        private async void CheckResult(int answerIndex)
        {
            var answer = _currentQuestion.Value[answerIndex];
            ShowResult(answer.Equals(_currentQuestion.Key.Answer));
            await Task.Delay(1000);
            ShowQuestion(++_currentQuestionIndex);
        }

    }
}
