using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public struct Move
    {
        public readonly int player;
        public readonly Position fromPosition;
        public readonly Position toPosition;

        public Move(int player, Position fromPosition, Position toPosition)
        {
            this.player = player;
            this.fromPosition = fromPosition;
            this.toPosition = toPosition;
        }
    }        
}
