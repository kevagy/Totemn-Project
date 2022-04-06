using TriviaGame.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

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
        _questionsLevelController.gameObject.SetActive(true);
        _playButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _playButton.gameObject.SetActive(true);
        _questionsLevelController.gameObject.SetActive(false);
    }

    public void ShowScore(float score)
    {
        _scorePanel.SetScore(score);
        _scorePanel.gameObject.SetActive(true);
    }

}