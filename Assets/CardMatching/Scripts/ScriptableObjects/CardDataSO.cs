using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace CardMatching.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardMatching/CardData")]
    public class CardDataSO : ScriptableObject
    {
        //todo: this is not useful!!!
        public static CardDataSO _instance;
        public static CardDataSO Instance => _instance;

        public float FlipAniamtionTime = 0.25f;
        public Ease FlipAnimationEase = Ease.OutCubic;

        [SerializeField] private Sprite _cardCoverSprite;
        public Sprite CardCoverSprite => _cardCoverSprite;

        [SerializeField] private List<Sprite> _cardIconList;
        public List<Sprite> CardIconList => _cardIconList;


        private void OnEnable()
        {
            _instance = this;
        }
    }
}
