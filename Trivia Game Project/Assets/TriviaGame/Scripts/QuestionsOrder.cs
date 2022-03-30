using TriviaGame.Scripts;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class QuestionsOrder : ScriptableObject
{
    [SerializeField] private QuestionData<string, string> _textQuestion;
    [SerializeField] private QuestionData<Sprite, string> _imageQuestion;
    [SerializeField] private QuestionData<CombineTextImage, string> _textImageQuestion;
    [SerializeField] private QuestionData<string, bool> _trueFalseQuestion;
    [SerializeField] private QuestionData<string, int> _bordersQuestion;
    [SerializeField] private QuestionData<string, int> _urbanPopulationQuestion;
    [SerializeField] private QuestionData<string, int> _populationLowQuestion;
    [SerializeField] private QuestionData<string, int> _populationMidQuestion;
    [SerializeField] private QuestionData<string, int> _populationHighQuestion;

    public QuestionData<string, string> TextQuestion => _textQuestion;
    public QuestionData<Sprite, string> ImageQuestion => _imageQuestion;
    public QuestionData<CombineTextImage, string> TextImageQuestion => _textImageQuestion;
    public QuestionData<string, bool> TrueFalseQuestion => _trueFalseQuestion;
    public QuestionData<string, int> BordersQuestion => _bordersQuestion;
    public QuestionData<string, int> UrbanPopulationQuestion => _urbanPopulationQuestion;
    public QuestionData<string, int> PopulationLowQuestion => _populationLowQuestion;
    public QuestionData<string, int> PopulationMidQuestion => _populationMidQuestion;
    public QuestionData<string, int> PopulationHighQuestion => _populationHighQuestion;
}
