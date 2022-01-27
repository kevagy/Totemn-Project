using TriviaGame.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
   [SerializeField] private Button _playButton;
   [SerializeField] private LevelController _imageQuestion;
   [SerializeField] private ScorePanel _scorePanel;

   private LevelController _openedLevel;
   private void OnEnable()
   {
      _playButton.onClick.AddListener(OnPlayButtonClick);
   }

   private void OnDisable()
   {
      _playButton.onClick.RemoveListener(OnPlayButtonClick);
   }

   private void OnPlayButtonClick()
   {
      _imageQuestion.gameObject.SetActive(true);
      _playButton.gameObject.SetActive(false);
   }

   public void Show()
   {
      gameObject.SetActive(true);
      _openedLevel.gameObject.SetActive(false);
   }

   public void ShowScore(int score)
   {
      _scorePanel.SetScore(score);
      _scorePanel.gameObject.SetActive(true);
   }
   
}
