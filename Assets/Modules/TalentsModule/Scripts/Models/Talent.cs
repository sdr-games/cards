using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Talent
    {
        public bool IsBlocked { get; private set; }
        public bool IsActive { get; private set; }

        public event EventHandler<BlockChangedEventArgs> BlockChanged;
        public event EventHandler<ActiveChangedEventArgs> ActiveChanged;

        public void ChangeBlock()
        {
            IsBlocked = !IsBlocked;
            BlockChanged?.Invoke(this, new BlockChangedEventArgs(IsBlocked));
        }

        public void SetBlock(bool isBlock)
        {
            IsBlocked = isBlock;
            BlockChanged?.Invoke(this, new BlockChangedEventArgs(IsBlocked));
        }

        public void ChangeActive()
        {
            IsActive = !IsActive;
            ActiveChanged?.Invoke(this, new ActiveChangedEventArgs(IsActive));
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            ActiveChanged?.Invoke(this, new ActiveChangedEventArgs(IsActive));
        }
    }
}
