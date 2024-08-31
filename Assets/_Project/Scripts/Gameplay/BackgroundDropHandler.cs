using _Project.Scripts.Enums;
using _Project.Scripts.Items;
using _Project.Scripts.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Gameplay
{
    public class BackgroundDropHandler : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            var newItem = eventData.pointerDrag.GetComponent<Item>();
            
            SoundType.Wrong.Play();
            
            newItem.Return();
        }
    }
}
