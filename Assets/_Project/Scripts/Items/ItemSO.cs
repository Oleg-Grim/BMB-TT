using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Items
{
    [CreateAssetMenu(menuName = "Gameplay/Create ItemSO", fileName = "ItemSO", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public ItemType Type;
        public Sprite ItemImage;
        public Sprite ShadowImage;
    }
}