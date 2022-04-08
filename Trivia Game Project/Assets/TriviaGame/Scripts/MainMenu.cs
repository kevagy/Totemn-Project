using TriviaGame.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private Button _playButton;
    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private QuestionsLevelController _questionsLevelController;

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
        _questionsLevelController.ShowQuestion();
        _mainMenuScreen.gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _mainMenuScreen.gameObject.SetActive(true);
    }

    public void ShowScore(float score)
    {
        _scorePanel.SetScore(score);
        _scorePanel.gameObject.SetActive(true);
    }

}