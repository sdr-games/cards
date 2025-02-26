using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Views;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class CardPresenter
    {
        public CardPresenter(Card card, CardView cardView, int siblingIndex)
        {
            cardView.Initialize(siblingIndex, card.Name, card.GetLocalizedDescription(), card.Icon, card.Cost.ToString());
        }
    }
}
