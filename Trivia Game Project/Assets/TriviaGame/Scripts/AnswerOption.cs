using System;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOption : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;

    private int _index;
    private Action<int> _onChoosedAnswer;
    public void SetupAnswer(string text, int index, Action<int> onChoosedAnswer)
    {
        _text.text = text;
        _index = index;
        _onChoosedAnswer = onChoosedAnswer;
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
        }
    }
}