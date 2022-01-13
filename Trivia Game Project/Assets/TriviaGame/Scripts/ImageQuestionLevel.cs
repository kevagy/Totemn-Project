using UnityEngine;
using UnityEngine.UI;
namespace TriviaGame.Scripts
{
    public class ImageQuestionLevel : LevelController
    {
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<Sprite, string>[] _question;
   
        
        
    }
}
