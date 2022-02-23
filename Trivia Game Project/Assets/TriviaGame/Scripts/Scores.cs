using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    public TMPro.TMP_Text _scoreText;
    private int _currentScore;

    void Start()
    {
        _currentScore = 0;
        _scoreText.text = _currentScore.ToString();

    }



    public void Addscore()
    {
        _currentScore += 10;
        _scoreText.text = _currentScore.ToString();

    }

    public void Deductscore()
    {
        _currentScore = _currentScore > 0 ? _currentScore - 10 : 0;
        _scoreText.text = _currentScore.ToString();
    }



}
