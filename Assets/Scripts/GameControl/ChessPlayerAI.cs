using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl
{
    public class ChessPlayerAI
    {
        public int AILevel;
        public bool GetAIChoose(GameChessBoard board,out int i, out int j)
        {
            i = 0;
            j = 0;
            if (AILevel == 1)
            {
                return LowLevelChoose(board,out i, out j);
            }
            return true;
        }

        private bool LowLevelChoose(GameChessBoard board,out int i, out int j)
        {
            // 随机选点，从剩余可选项中随机选择一个
            i = 0;
            j = 0;
            var emptyPositions = board.GetEmptyPosition();
            if (emptyPositions.Count == 0) return false;
            else
            {
                var index = Random.Range(0, emptyPositions.Count);
                i = emptyPositions[index].Item1;
                j = emptyPositions[index].Item2;
                return true;
            }
        }
    }

}
