using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Items
{
    public class ClickHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Vector3 _targetScale = new Vector3(1.01f, 1.01f,1.01f);
        [SerializeField] private float _scaleDuration = 0.1f;

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
            transform.DOScale(_targetScale, _scaleDuration).SetEase(Ease.Linear).OnComplete(()=> transform.DOScale(Vector3.one, _scaleDuration)).Restart();
        }
    }
}