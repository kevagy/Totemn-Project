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
    [SerializeField] private PresetsLevelController _presetsQuestionLevel;


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
    bool _hasQuestion = false;

    private Dictionary<SequenceQuestion, Action> _questionSetups;

    public QuestionsLevelController()
    {
        _questionSetups = new Dictionary<SequenceQuestion, Action>
        {
            {SequenceQuestion.Image, () => { SetupImage(_questionsOrder.ImageQuestion); }},
            {SequenceQuestion.Text, () => { SetupText(_questionsOrder.TextQuestion); }},
            {SequenceQuestion.TextImage, () => { SetupTextImage(_questionsOrder.TextImageQuestion); }},
            {SequenceQuestion.TrueFalse, () => { SetupTrueFalse(_questionsOrder.TrueFalseQuestion); }},
            {SequenceQuestion.CountryPopulationLow, () => { SetupPopulation(_questionsOrder.PopulationLowQuestion); }},
            {SequenceQuestion.CountryPopulationMid, () => { SetupPopulation(_questionsOrder.PopulationMidQuestion); }},
            {
                SequenceQuestion.CountryPopulationHigh,
                () => { SetupPopulation(_questionsOrder.PopulationHighQuestion); }
            },
            {SequenceQuestion.UrbanPopulation, () => { SetupPopulation(_questionsOrder.UrbanPopulationQuestion); }},
            {SequenceQuestion.BorderQuestions, () => { SetupPopulation(_questionsOrder.BordersQuestion); }},
        };
    }

    private void SetupTrueFalse(QuestionData<string, bool> data)
    {
        _trueFalseQuestionLevel.Question = data.Questions;
        var trueFalseQuestion = data.GetQuestion();
        _hasQuestion = trueFalseQuestion.hasQuestion;
        if (_hasQuestion)
        {
            _trueFalseQuestionLevel.gameObject.SetActive(true);
            _trueFalseQuestionLevel.ShowQuestion(trueFalseQuestion.question, ShowQuestion);
            _questionGameObject = _trueFalseQuestionLevel.gameObject;
        }
    }

    private void SetupTextImage(QuestionData<CombineTextImage, string> data)
    {
        _textImageQuestionLevel.Question = data.Questions;
        var textImageQuestion = data.GetQuestion();
        _hasQuestion = textImageQuestion.hasQuestion;
        if (_hasQuestion)
        {
            _textImageQuestionLevel.gameObject.SetActive(true);
            _textImageQuestionLevel.ShowQuestion(textImageQuestion.question, ShowQuestion);
            _questionGameObject = _textImageQuestionLevel.gameObject;
        }
    }

    private void SetupText(QuestionData<string, string> data)
    {
        _textQuestionLevel.Question = data.Questions;
        var textQuestion = data.GetQuestion();
        _hasQuestion = textQuestion.hasQuestion;
        if (_hasQuestion)
        {
            _textQuestionLevel.gameObject.SetActive(true);
            _textQuestionLevel.ShowQuestion(textQuestion.question, ShowQuestion);
            _questionGameObject = _textQuestionLevel.gameObject;
        }
    }


    private void SetupImage(QuestionData<Sprite, string> data)
    {
        _imageQuestionLevel.Question = data.Questions;
        var imageQuestion = data.GetQuestion();
        _hasQuestion = imageQuestion.hasQuestion;
        if (_hasQuestion)
        {
            _imageQuestionLevel.gameObject.SetActive(true);
            _imageQuestionLevel.ShowQuestion(imageQuestion.question, ShowQuestion);
            _questionGameObject = _imageQuestionLevel.gameObject;
        }
    }

    private void SetupPopulation(QuestionData<string, int> data)
    {
        _presetsQuestionLevel.Question = data.Questions;
        _presetsQuestionLevel.AnswerSprites = data.Answers;
        var presetsQuestion = data.GetQuestion();
        _hasQuestion = presetsQuestion.hasQuestion;
        if (_hasQuestion)
        {
            _presetsQuestionLevel.gameObject.SetActive(true);
            _presetsQuestionLevel.ShowQuestion(presetsQuestion.question, ShowQuestion);
            _questionGameObject = _presetsQuestionLevel.gameObject;
        }
    }

    private void Start()
    {
        int numberOfTotalQuestion = 0;
        _timerAndScorePanel?.SetActive(false);
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
        _questionGameObject?.SetActive(false);
        if (_questionShowed >= _questionInLevel)
        {
            EndOfQuestions();
            return;
        }

        _hasQuestion = false;

        while (!_hasQuestion)
        {
            _questionSetups[_questionDataShuffle[_index].Type]?.Invoke();
            _questionShowed++;
            _index++;
            if (_index >= _questionDataShuffle.Count) //TODO think how to improve
            {
                _index = 0;
                break;
            }
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
        EndOfQuestions();
    }

    void EndOfQuestions()
    {
        _questionGameObject?.SetActive(false);
        _mainMenu.ShowScore(_score);
        _questionShowed = 0;
    }
}