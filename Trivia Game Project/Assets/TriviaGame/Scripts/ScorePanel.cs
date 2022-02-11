using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _scoreText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private MainMenu _mainMenu;

    public void SetScore(float score)
    {
        _scoreText.SetText(score.ToString());
    }
    
    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseButtonClick);
    }


    private void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
        _mainMenu.Show();
    }

}
