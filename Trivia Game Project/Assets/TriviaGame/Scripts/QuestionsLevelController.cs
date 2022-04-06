using System.Collections.Generic;
using System.Linq;
using TriviaGame.Scripts;
using UnityEngine;

public class QuestionsLevelController : MonoBehaviour
{
    [SerializeField] private QuestionsOrder _questionsOrder;
    [SerializeField] private TextQuestionLevel _textQuestionLevel;
    [SerializeField] private ImageQuestionLevel _imageQuestionLevel;
    [SerializeField] private TextImageQuestionLevel _textImageQuestionLevel;
    [SerializeField] private TrueFalseQuestionLevel _trueFalseQuestionLevel;
    [SerializeField] private int _questionInLevel = 10;

    private List<IQuestionData> _questionData = new List<IQuestionData>();

    private List<IQuestionData> _questionDataShuffle;
    private int _index = 0;

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


    private void ShowQuestion()
    {
        bool hasQuestion = false;
        while (!hasQuestion)
        {
            switch (_questionDataShuffle[_index].Type)
            {
                case SequenceQuestion.Image:
                    var result = _questionsOrder.ImageQuestion.GetQuestion();
                    hasQuestion = result.hasQuestion;
                    if (hasQuestion)
                    {
                        _imageQuestionLevel.Question = result.question;
                    }
                    break;

                case SequenceQuestion.Text:
                    _textQuestionLevel.Question = _questionsOrder.TextQuestion.Questions;
                    break;

                case SequenceQuestion.PresetAnswers:
                    //TODO Level controller 
                    break;

                case SequenceQuestion.TextImage:
                    _textImageQuestionLevel.Question = _questionsOrder.TextImageQuestion.Questions;
                    break;

                case SequenceQuestion.TrueFalse:
                    _trueFalseQuestionLevel.Question = _questionsOrder.TrueFalseQuestion.Questions;
                    break;
            }


            _index++;
            if (_index >= _questionDataShuffle.Count)//TODO think how to improve
            {
                _index = 0;
                break;
            }
        }
    }
}