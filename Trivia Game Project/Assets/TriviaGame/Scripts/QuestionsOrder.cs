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
    [SerializeField] private QuestionData<string, string> _populationQuestion;

    public QuestionData<string, string> TextQuestion => _textQuestion;
    public QuestionData<Sprite, string> ImageQuestion => _imageQuestion;
    public QuestionData<CombineTextImage, string> TextImageQuestion => _textImageQuestion;
    public QuestionData<string, bool> TrueFalseQuestion => _trueFalseQuestion;
    public QuestionData<string, string> PopulationQuestion => _populationQuestion;
}