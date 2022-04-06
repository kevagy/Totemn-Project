using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class TrueFalseQuestionLevel : LevelController
    {
        [SerializeField] private TMPro.TMP_Text _questionText;
        [SerializeField] private Image _questionImage;
        [SerializeField] private QuestionTemplate<string, bool>[] _question;

        public QuestionTemplate<string, bool>[] Question
        {
            set { _question = value; }
            get { return _question; }
        }


    }
}