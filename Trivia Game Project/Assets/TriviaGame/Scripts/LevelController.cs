using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] protected AnswerOption _answerOptionPrefab;
        [SerializeField] private Image _answerResult;
        [SerializeField] private Sprite _correct;
        [SerializeField] private Sprite _incorrect;
        [SerializeField] protected ToggleGroup _toggleGroup;
        [SerializeField] protected int _numberOfQuestionToShow = 12;
        [SerializeField] protected int _nubmerOfAnswers = 4;

        [Tooltip("Need to be in ms")]
        [SerializeField] protected int _timeToShowResult = 500;

        [SerializeField] private TMPro.TMP_Text _timerText;

        [Tooltip("Time to end in seconds")]
        [SerializeField] private int _timeToEnd = 60;

        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private TMPro.TMP_Text _scoreText;

        protected List<AnswerOption> _answerOptions = new List<AnswerOption>();
        protected int _currentQuestionIndex = 0;
        protected float _score;

        private bool _isTimerRunning;
        protected virtual void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _answerResult.gameObject.SetActive(false);
            StartTimer();
        }

        protected List<KeyValuePair<QuestionTemplate<Q, A>, List<A>>> GenerateQuestions<Q, A>(QuestionTemplate<Q, A>[] question)
        {
            Debug.Log($"GenerateQuestions answerToggles {_answerOptions.Count}");
            var random = new System.Random();
            var randomList = question.OrderBy(item => random.Next()).ToList();
            Debug.Log($"GenerateQuestions answerToggles {_answerOptions.Count}");
            var generatedQuestion = new List<KeyValuePair<QuestionTemplate<Q, A>, List<A>>>();
            Debug.Log($"GenerateQuestions answerToggles {_answerOptions.Count}");
            for (int i = 0; i < _numberOfQuestionToShow; i++)
            {
                List<A> answers = new List<A>();
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
                generatedQuestion.Add(new KeyValuePair<QuestionTemplate<Q, A>, List<A>>(randomList[i], answers));
            }
            Debug.Log($"GenerateQuestions answerToggles {_answerOptions.Count}");
            return generatedQuestion;
        }

        protected async Task CreateAnswerOptions()
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


        protected virtual void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            _mainMenu.Show();
        }

        protected void ShowResult(bool isCorrect)
        {
            _answerResult.gameObject.SetActive(true);
            _answerResult.sprite = isCorrect ? _correct : _incorrect;
        }
        protected void HideResult()
        {
            _answerResult.gameObject.SetActive(false);
        }

        private async Task StartTimer()
        {
            int timer = _timeToEnd;
            _isTimerRunning = true;
            while (timer > 0 && _isTimerRunning)
            {
                await Task.Delay(1000);
                timer -= 1;
                TimeSpan dateTime = new TimeSpan(0, 0, timer);
                _timerText.text = $"{dateTime.Minutes.ToString("00")}:{dateTime.Seconds.ToString("00")}";
            }
            _isTimerRunning = false;
            FinishQuestion();
        }

        protected void AddScore(float score)
        {
            _score = Mathf.Clamp(_score + score, 0, float.MaxValue);
            _scoreText.text = _score.ToString();
        }

        protected void FinishQuestion()
        {
            Debug.Log($"FinishQuestion");
            _mainMenu.ShowScore(_score);
        }

    }
}
