using System.Collections.Generic;
using CardMatching.GridBox;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.Gameplay
{
    public class CardMatchCommandInvoker : MonoBehaviour
    {
        private readonly Stack<ICommand> _undoStack = new ();
        private readonly Stack<ICommand> _redoStack = new ();
        private readonly List<CardMatchCommand> _cardMatchCommandList = new ();


        public void AddSelectedCardItem(GridBoxCardItem gridBoxCardItem)
        {
            CardMatchCommand cardMatchCommand = GetEmptySelectedCardPaid();
            cardMatchCommand.AddCard(gridBoxCardItem);
            ExecuteCommand(cardMatchCommand);
        }

        public void ResetList()
        {
            CustomDebug.Log($"CardMatchCommandController-ResetList:{_cardMatchCommandList.Count}");
            foreach (var item in _cardMatchCommandList)
            {
                item.Clear();
            }
        }

        private CardMatchCommand GetEmptySelectedCardPaid()
        {
            CardMatchCommand cardMatchCommand;
            //todo:we can use int for hold the last empty index number for reduce CPU time on the for loop!
            int count = _cardMatchCommandList.Count;
            for (int i = 0; i < count; i++)
            {
                cardMatchCommand = _cardMatchCommandList[i];
                if (!cardMatchCommand.IsFull())
                    return cardMatchCommand;
            }

            cardMatchCommand = new CardMatchCommand();
            _cardMatchCommandList.Add(cardMatchCommand);

            //CustomDebug.Log($"CardMatchCommandController-GetEmptySelectedCardPaid:{_cardMatchCommandList.Count}");

            return cardMatchCommand;
        }

        private void ExecuteCommand(CardMatchCommand cardMatchCommand)
        {
            if (cardMatchCommand.IsFull())
            {
                cardMatchCommand.Execute();
                _undoStack.Push(cardMatchCommand);

                // clear out the redo stack if we make a new move
                _redoStack.Clear();
            }
        }

        public void UndoCommand()
        {
            if (_undoStack.Count > 0)
            {
                ICommand activeCommand = _undoStack.Pop();
                _redoStack.Push(activeCommand);
                activeCommand.Undo();
            }
        }

        public void RedoCommand()
        {
            if (_redoStack.Count > 0)
            {
                ICommand activeCommand = _redoStack.Pop();
                _undoStack.Push(activeCommand);
                activeCommand.Undo();
            }
        }
    }
}