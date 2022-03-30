using System.Collections.Generic;
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
        
    }
}