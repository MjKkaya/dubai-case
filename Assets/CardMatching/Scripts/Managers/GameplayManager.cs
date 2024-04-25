using CardMatching.Events;
using CardMatching.Gameplay;
using CardMatching.GridBox;
using UnityEngine;


namespace CardMatching.Managers
{
    [RequireComponent(typeof(SelectedCardsMatchController))]
    public class GameplayManager : MonoBehaviour
    {
        private SelectedCardsMatchController _SelectedCardsMatchController;


        private void OnEnable()
        {
            _SelectedCardsMatchController = GetComponent<SelectedCardsMatchController>();
            GameEvents.CardFlipped += GameEvents_CardFlipped;
        }

        private void OnDisable()
        {
            GameEvents.CardFlipped -= GameEvents_CardFlipped;
        }

        private void GameEvents_CardFlipped(GridBoxCardItem gridBoxCardItem)
        {
            _SelectedCardsMatchController.AddSelectedCardItem(gridBoxCardItem);
        }
    }
}