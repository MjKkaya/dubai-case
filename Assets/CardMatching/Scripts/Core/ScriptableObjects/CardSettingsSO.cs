using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace CardMatching.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardSettings", menuName = "CardMatching/CardSettings")]
    public class CardSettingsSO : ScriptableObject
    {
        public float FlipAniamtionTime = 0.25f;
        public Ease FlipAnimationEase = Ease.OutCubic;

        [SerializeField] private Sprite _cardCoverSprite;
        public Sprite CardCoverSprite => _cardCoverSprite;

        [SerializeField] private List<Sprite> _cardIconList;
        public List<Sprite> CardIconList => _cardIconList;
    }
}
