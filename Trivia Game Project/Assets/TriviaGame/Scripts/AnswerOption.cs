using System;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOption : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    private bool _isTrueAnswer;
    
    private int _index;
    private Action<int> _onChoosedAnswer;
    private Action<bool> _onChoosedBoolAnswer;
    public void SetupAnswer(string text, int index, Action<int> onChoosedAnswer)
    {
        _text.text = text;
        _index = index;
        _onChoosedAnswer = onChoosedAnswer;
    }
    
    public void SetupAnswer(bool answer, int index, Action<bool> onChoosedAnswer)
    {
        _isTrueAnswer = answer;
        _index = index;
        _onChoosedBoolAnswer = onChoosedAnswer;
    }
    
    public void SetupAnswer(Sprite sprite, int index, Action<int> onChoosedAnswer)
    {
        _image.sprite = sprite;
        _index = index;
        _onChoosedAnswer = onChoosedAnswer;
    }

    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        _toggle.group = toggleGroup;
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            _onChoosedAnswer?.Invoke(_index);
            _onChoosedBoolAnswer?.Invoke(_isTrueAnswer);
        }
    }

}