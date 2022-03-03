using System.Collections;
using System.Collections.Generic;
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

}