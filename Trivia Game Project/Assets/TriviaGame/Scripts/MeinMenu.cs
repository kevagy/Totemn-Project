using UnityEngine;
using UnityEngine.UI;

public class MeinMenu : MonoBehaviour
{
   
   [SerializeField] private Button _playButton;
   [SerializeField] private GameObject _imageQuestion;

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
      _imageQuestion.SetActive(true);
      _playButton.gameObject.SetActive(false);
   }
}
