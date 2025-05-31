using System;
using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Models
{
    [Serializable]
    public class ActiveBlockSidesModelC
    {
        [field: SerializeField] public ActiveBlockSideModelC TopSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModelC RightSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModelC BottomSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModelC LeftSide { get; private set; }

        public List<ActiveBlockSideModelC> GetSides()
        {
            return new List<ActiveBlockSideModelC>() { TopSide, RightSide, BottomSide, LeftSide };
        }
    }
}
