using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Views;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class CardPresenter
    {
        public CardPresenter(Card card, CardView cardView)
        {
            cardView.Initialize(card.Name, card.Description, card.Icon, card.Cost.ToString());
        }
    }
}
