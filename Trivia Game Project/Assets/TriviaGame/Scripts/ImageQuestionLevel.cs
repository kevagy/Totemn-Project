using UnityEngine;
using UnityEngine.UI;
namespace TriviaGame.Scripts
{
    public class ImageQuestionLevel : LevelController
    {
        [SerializeField] private Sprite _questionImage;
        [SerializeField] private QuestionTemplate<Sprite, string>[] _question;
        [SerializeField] private ToggleGroup _toggleGroup;
        // [SerializeField] private 
    }
}