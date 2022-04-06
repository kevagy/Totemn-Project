using System.Collections.Generic;
using System.Linq;
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
    private int _questionShowed;

    private List<IQuestionData> _questionData = new List<IQuestionData>();

    private List<IQuestionData> _questionDataShuffle;
    private int _index = 0;

    private GameObject _questionGameObject;

    private async void Start()
    {
        int numberOfTotalQuestion = 0;
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

    
    

    public void ShowQuestion()
    {
        if (_questionShowed >= _questionInLevel)
        {
            float score = 0;
            score += _textQuestionLevel.Score;
            score += _imageQuestionLevel.Score;
            score += _textImageQuestionLevel.Score;
            score += _trueFalseQuestionLevel.Score;
            _mainMenu.ShowScore(score);
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
                    var imageQuestion = _questionsOrder.ImageQuestion.GetQuestion();
                    hasQuestion = imageQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _imageQuestionLevel.ShowQuestion(imageQuestion.question, ShowQuestion);
                        _questionGameObject = _imageQuestionLevel.gameObject;
                    }

                    break;

                case SequenceQuestion.Text:
                    var textQuestion = _questionsOrder.TextQuestion.GetQuestion();
                    hasQuestion = textQuestion.hasQuestion;
                    if (hasQuestion)
                    {
                        _textQuestionLevel.ShowQuestion(textQuestion.question, ShowQuestion);
                        _questionGameObject = _textQuestionLevel.gameObject;
                    }

                    break;

                case SequenceQuestion.PresetAnswers:
                    //TODO Level controller 
                    break;

                case SequenceQuestion.TextImage:
                    var textImageQuestion = _questionsOrder.TextImageQuestion.GetQuestion();
                    hasQuestion = textImageQuestion.hasQuestion;
                    if (hasQuestion)
                    {
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
                        _trueFalseQuestionLevel.ShowQuestion(trueFalseQuestion.question, ShowQuestion);
                        _questionGameObject = _trueFalseQuestionLevel.gameObject;
                    }

                    break;
            }

            _questionGameObject?.SetActive(true);
            _questionShowed++;

            _index++;
            if (_index >= _questionDataShuffle.Count) //TODO think how to improve
            {
                _index = 0;
                break;
            }
        }
    }
}