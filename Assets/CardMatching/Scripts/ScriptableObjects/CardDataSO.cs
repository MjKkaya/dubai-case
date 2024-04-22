using System.Collections.Generic;
using UnityEngine;


namespace CardMatching.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardMatching/CardData")]
    public class CardDataSO : ScriptableObject
    {
        [SerializeField] private List<Sprite> _cardIconList;
        public List<Sprite> CardIconList => _cardIconList;
    }
}
