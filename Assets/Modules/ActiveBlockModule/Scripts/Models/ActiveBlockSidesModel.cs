using System;
using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Models
{
    [Serializable]
    public class ActiveBlockSidesModel
    {
        [field: SerializeField] public ActiveBlockSideModel TopSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModel RightSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModel BottomSide { get; private set; }
        [field: SerializeField] public ActiveBlockSideModel LeftSide { get; private set; }

        public List<ActiveBlockSideModel> GetSides()
        {
            return new List<ActiveBlockSideModel>() { TopSide, RightSide, BottomSide, LeftSide };
        }
    }
}
