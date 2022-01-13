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


        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _answerResult.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnCloseButtonClick()
        {
            //todo implement close function
        }

        protected void ShowResult(bool isCorrect)
        {
            _answerResult.sprite = isCorrect ? _correct : _incorrect;
        }
        
    }
}
