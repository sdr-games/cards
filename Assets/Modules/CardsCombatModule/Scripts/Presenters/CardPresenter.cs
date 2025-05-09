using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.CharacterCombatModule.Models;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class CardPresenter
    {
        public CardPresenter(Card card, CardView cardView, PlayerParamsModel playerParamsModel)
        {
            cardView.Initialize(card.Name, card.GetLocalizedDescription(playerParamsModel), card.Icon, card.Cost.ToString());
        }
    }
}
