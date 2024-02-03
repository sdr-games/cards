using System.Collections.Generic;
using UnityEngine;

public class AIDefenceController : DefenceController
{
    public List<ObjectSelectController> Defence(int maxSelectedPartsCount)
    {
        List<ObjectSelectController> selectedParts = new List<ObjectSelectController>();
        for(int i = 0; i < maxSelectedPartsCount; i++)
        {
            ObjectSelectController objSelectController = null;
            if(Random.Range(0, 100) > 30)
            {
                int index = Random.Range(0, ObjectSelectableParts.Length - 1);
                objSelectController = ObjectSelectableParts[index];
            }
            selectedParts.Add(objSelectController);
        }
        return selectedParts;
    }

    public List<Card> MagicDefence(int maxSelectedCardsCount)
    {
        List<Card> cards = new List<Card>();
        for(int i = 0; i < maxSelectedCardsCount; i++)
        {
            int index = Random.Range(0, SelectableCards.Length - 1);
            cards.Add(SelectableCards[index]);
        }
        return cards;
    }
}
