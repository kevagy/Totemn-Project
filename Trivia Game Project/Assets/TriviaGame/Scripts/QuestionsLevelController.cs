using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TriviaGame.Scripts;
using UnityEngine;

public class QuestionsLevelController : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private QuestionsOrder _questionsOrder;
    [SerializeField] private TextQuestionLevel _textQuestionLevel;
    [SerializeField] private ImageQuestionLevel _imageQuestionLevel;
    [SerializeField] private TextImageQuestionLevel _textImageQuestionLevel;
    [SerializeField] private TrueFalseQuestionLevel _trueFalseQuestionLevel;

    [SerializeField] private int _questionInLevel = 10;
    [SerializeField] private GameObject _timerAndScorePanel;
    [SerializeField] private TMPro.TMP_Text _timerText;
    [SerializeField] private TMPro.TMP_Text _scoreText;

    [Tooltip("Time to end in seconds")] [SerializeField]
    private int _timeToEnd = 60;

    private int _questionShowed;

    private List<IQuestionData> _questionData = new List<IQuestionData>();

    private List<IQuestionData> _questionDataShuffle;
    private int _index = 0;

    private GameObject _questionGameObject;
    private bool _isTimerRunning;
    private float _score = 0;

    private void Start()
    {
        int numberOfTotalQuestion = 0;
        _timerAndScorePanel.SetActive(false);
        _questionData.Add(_questionsOrder.TextQuestion);
        _questionData.Add(_questionsOrder.ImageQuestion);
        _questionData.Add(_questionsOrder.TextImageQuestion);
        _questionData.Add(_questionsOrder.TrueFalseQuestion);
        _questionData.Add(_questionsOrder.BordersQuestion);
        _questionData.Add(_questionsOrder.UrbanPopulationQuestion);
        _questionData.Add(_questionsOrder.PopulationLowQuestion);
        _questionData.Add(_questionsOrder.PopulationMidQuestion);
        _questionData.Add(_questionsOrder.PopulationHighQuestion);

        foreach (var question in _questionData)
        {
            numberOfTotalQuestion += question.QuestionCount;
        }

        foreach (var question in _questionData)
        {
            question.Quantity = numberOfTotalQuestion;
            question.SetTotalQuestionInLevel(_questionInLevel);
        }

        var random = new System.Random();
        _questionDataShuffle = _questionData.OrderBy(item => random.Next()).ToList();
    }


    private void CalculateAndShowScore()
    {
        _score = 0;
        _score += _textQuestionLevel.Score;
        _score += _imageQuestionLevel.Score;
        _score += _textImageQuestionLevel.Score;
        _score += _trueFalseQuestionLevel.Score;
        _scoreText.text = _score.ToString();
    }

    public void ShowQuestion()
    {
        StartTimer().Forget();
        CalculateAndShowScore();
        if (_questionShowed >= _questionInLevel)
        {
            _mainMenu.ShowScore(_score);
            _questionShowed = 0;
            return;
        }

        _questionGameObject?.SetActive(false);
        bool hasQuestion = false;

        while (!hasQuestion)
        {
            switch (_questionDataShuffle[_index].Type)
            {
                case SequenceQuestion.Image:
                    _imageQuestionLevel.Question = _questionsOrder.ImageQuestion.Questions;
                    var imageQuestion = _questionsOrder.ImageQuestion.GetQuestion();
                    hasQuestion = imageQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _imageQuestionLevel.gameObject.SetActive(true);
                        _imageQuestionLevel.ShowQuestion(imageQuestion.question, ShowQuestion);
                        _questionGameObject = _imageQuestionLevel.gameObject;
                    }

                    break;

                case SequenceQuestion.Text:
                    _textQuestionLevel.Question = _questionsOrder.TextQuestion.Questions;
                    var textQuestion = _questionsOrder.TextQuestion.GetQuestion();
                    hasQuestion = textQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _textQuestionLevel.gameObject.SetActive(true);
                        _textQuestionLevel.ShowQuestion(textQuestion.question, ShowQuestion);
                        _questionGameObject = _textQuestionLevel.gameObject;
                    }

                    break;

                case SequenceQuestion.PresetAnswers:
                    //TODO Level controller 
                    break;

                case SequenceQuestion.TextImage:
                    _textImageQuestionLevel.Question = _questionsOrder.TextImageQuestion.Questions;
                    var textImageQuestion = _questionsOrder.TextImageQuestion.GetQuestion();
                    hasQuestion = textImageQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _textImageQuestionLevel.gameObject.SetActive(true);
                        _textImageQuestionLevel.ShowQuestion(textImageQuestion.question, ShowQuestion);
                        _questionGameObject = _textImageQuestionLevel.gameObject;
                    }

                    break;

                case SequenceQuestion.TrueFalse:
                    _trueFalseQuestionLevel.Question = _questionsOrder.TrueFalseQuestion.Questions;

                    var trueFalseQuestion = _questionsOrder.TrueFalseQuestion.GetQuestion();
                    hasQuestion = trueFalseQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _trueFalseQuestionLevel.gameObject.SetActive(true);
                        _trueFalseQuestionLevel.ShowQuestion(trueFalseQuestion.question, ShowQuestion);
                        _questionGameObject = _trueFalseQuestionLevel.gameObject;
                    }

                    break;
            }

            _questionShowed++;

            _index++;
            if (_index >= _questionDataShuffle.Count) //TODO think how to improve
            {
                _index = 0;
                break;
            }
        }

        async UniTask StartTimer()
        {
            if (_isTimerRunning)
            {
                return;
            }

            int timer = _timeToEnd;
            _isTimerRunning = true;
            _timerAndScorePanel.SetActive(_isTimerRunning);

            while (timer > 0 && _isTimerRunning)
            {
                await UniTask.Delay(1000);
                timer -= 1;
                TimeSpan dateTime = new TimeSpan(0, 0, timer);
                _timerText.text = $"{dateTime.Minutes.ToString("00")}:{dateTime.Seconds.ToString("00")}";
            }

            _isTimerRunning = false;
            _timerAndScorePanel.SetActive(_isTimerRunning);
            // FinishQuestion();
        }
    }
}