using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame.Scripts
{

    public class QuestionPart : LevelController
    {
        [SerializeField]
        private QuestionsOrder _questionsOrder;
        [SerializeField]
        private TextQuestionLevel _textQuestionLevel;
        [SerializeField]
        private ImageQuestionLevel _imageQuestionLevel;
        [SerializeField]
        private TextImageQuestionLevel _textImageQuestionLevel;
        [SerializeField]
        private TrueFalseQuestionLevel _trueFalseQuestionLevel;


        // Start is called before the first frame update
        void Start()
        {

        }

    }

}
