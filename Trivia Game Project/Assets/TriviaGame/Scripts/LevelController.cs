using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame.Scripts
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] protected AnswerOption _answerOptionPrefab;
        [SerializeField] private Image _answerResult;
        [SerializeField] private Sprite _correct;
        [SerializeField] private Sprite _incorrect;
        [SerializeField] protected ToggleGroup _toggleGroup;
        [SerializeField] protected int _numberOfQuestionToShow = 12;
        [SerializeField] protected int _nubmerOfAnswers = 4;
        [Tooltip("Need to be in ms")]
        [SerializeField] protected int _timeToShowResult = 500;

        protected List<AnswerOption> _answerOptions;
        protected int _currentQuestionIndex = 0;

        protected virtual void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _answerResult.gameObject.SetActive(false);
        }
        
        protected List<KeyValuePair<QuestionTemplate<Q, A>, List<A>>>  GenerateQuestions<Q,A>(QuestionTemplate<Q, A>[] question)
        {
            var random = new System.Random();
            var randomList = question.OrderBy(item => random.Next()).ToList();
            var generatedQuestion = new List<KeyValuePair<QuestionTemplate<Q, A>, List<A>>>();
            for (int i = 0; i < _numberOfQuestionToShow; i++)
            {
                List<A> answers = new List<A>();
                answers.Add(randomList[i].Answer);
                for (int j = answers.Count; j < _nubmerOfAnswers; j++)
                {
                    var num = i + j;
                    if (num >= randomList.Count)
                    {
                        num = 0;
                    }
                    answers.Add(randomList[num].Answer);
                }
                answers = answers.OrderBy(item => random.Next()).ToList();
                generatedQuestion.Add(new KeyValuePair<QuestionTemplate<Q, A>, List<A>>(randomList[i], answers));
            }
            return generatedQuestion;
        }
        
        protected async Task CreateAnswerOptions()
        {
            if (_nubmerOfAnswers == _answerOptions.Count)
            {
                return;
            }

            for (int i = 0; i < _nubmerOfAnswers; i++)
            {
                AnswerOption answerOption = Instantiate(_answerOptionPrefab, _toggleGroup.transform);
                _answerOptions.Add(answerOption);
                await Task.Yield();
            }
        }
        

        protected virtual void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            //todo implement close function
        }

        protected void ShowResult(bool isCorrect)
        {
            _answerResult.gameObject.SetActive(true);
            _answerResult.sprite = isCorrect ? _correct : _incorrect;
        }
        protected void HideResult()
        {
            _answerResult.gameObject.SetActive(false);
        }
            

        /*
        protected virtual void GenerateQuestions()
        {
            throw new System.NotImplementedException();
        }*/
    }
}
