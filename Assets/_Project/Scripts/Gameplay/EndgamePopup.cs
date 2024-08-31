using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay
{
    public class EndgamePopup : MonoBehaviour
    {
        [SerializeField] private Image _overlay;
        [SerializeField] private Button _replayButton;

        private void OnEnable()
        {
            RackManager.OnFinished += FinishSequence;
        }

        private void OnDisable()
        {
            RackManager.OnFinished -= FinishSequence;
        }

        private void Awake()
        {
            Play();
        
            _replayButton.onClick.AddListener(Play);
        }

        private void Play()
        {
            _replayButton.gameObject.SetActive(false);
            _overlay.DOFade(0, 0.01f);
            RackManager.OnRestart?.Invoke();
        }

        private void FinishSequence()
        {
            _overlay.DOFade(1f, 2f).OnComplete(() => _replayButton.gameObject.SetActive(true));
        }
    }
}
