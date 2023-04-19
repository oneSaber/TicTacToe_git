using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl
{
    public class ChessPlayerAI
    {
        public int AIMark;
        public int PlayerMark;
        public int AILevel;
        public bool GetAIChoose(GameChessBoard board,out int i, out int j)
        {
            i = 0;
            j = 0;
            if (AILevel == 1)
            {
                return LowLevelChoose(board,out i, out j);
            }
            else
            {
                tempBoard = GameChessBoardFactor.CreateTempBoard(board);
                CurrentDepth = board.GetEmptyPosition().Count;
                CurrentPlayer = AIMark;
                MinMaxSearch(CurrentDepth);
                i = bestMove.x;
                j = bestMove.y;
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

        #region HighLevelAI

        // ai 使用极小值 极大值算法选择最佳落子位置
        private GameChessBoard tempBoard;
        private Vector2Int bestMove = new();
        private int MAX_MUN = 1000;
        private int TryAIWeight = -1;
        private int TryHumanWight = 1;
        private int CurrentPlayer;
        private int CurrentDepth;
        private int MinMaxSearch(int depth)
        {
            int bestValue = 0;
            if (tempBoard.GetBoardResult(out var winner))
                return EvaluateMap();

            var emptyList = tempBoard.GetEmptyPosition();
            if (depth == 0) return EvaluateMap();

            if (CurrentPlayer == PlayerMark)
                bestValue = MAX_MUN;
            if (CurrentPlayer == AIMark)
                bestValue = -MAX_MUN;
            foreach (var position in emptyList)
            {
                MarkMap(position.Item1,position.Item2);
                var value = MinMaxSearch(depth - 1);
                UnMarkMap(position.Item1,position.Item2);
                if (CurrentPlayer == AIMark)
                {
                    if (value > bestValue)
                    {
                        bestValue = value;
                        if (depth == CurrentDepth)
                            bestMove = new Vector2Int(position.Item1, position.Item2);
                    }
                }else if (CurrentPlayer == PlayerMark)
                {
                    if (value < bestValue)
                    {
                        bestValue = value;
                        if (depth == CurrentDepth)
                            bestMove = new Vector2Int(position.Item1, position.Item2);
                    }
                }
            }
            
            return bestValue;
        }

        // 评估函数
        private int EvaluateMap()
        {
            if (tempBoard.GetBoardResult(out var winner))
            {
                if (winner == PlayerMark) return -MAX_MUN;
                if (winner == AIMark) return MAX_MUN;
            }

            var AITryBoard = GameChessBoardFactor.CreateTempBoard(tempBoard);
            int count = 0;
            // 把空格全部填成ai
            for(int i = 0; i <3 ;i++)
                for (int j = 0; j < 3; j++)
                {
                    if (tempBoard[i, j] == 0 || tempBoard[i, j] == AIMark)
                        AITryBoard[i, j] = TryAIWeight;
                    else AITryBoard[i, j] = TryHumanWight;
                }

            // 因为 角色标记分别为1和2,如果有连续3个格子是相同的标记，则综合应该是3或者6，所以用结果对3取余，如果能除净，表示
            for (var i = 0; i < 3; i++)
                count += (AITryBoard[i, 0] + AITryBoard[i, 1] + AITryBoard[i, 2]) / 3;
            for (var j = 0; j < 3; j++)
                count += (AITryBoard[0, j] + AITryBoard[1, j] + AITryBoard[2, j]) / 3;
            count += (AITryBoard[0, 0] + AITryBoard[1, 1] + AITryBoard[2, 2]) / 3;
            count += (AITryBoard[2, 0] + AITryBoard[1, 1] + AITryBoard[0, 2]) / 3;
            
            
            // 把空格全部填成人类
            // 把空格全部填成ai
            for(int i = 0; i <3 ;i++)
            for (int j = 0; j < 3; j++)
            {
                if (tempBoard[i, j] == 0 || tempBoard[i, j] == PlayerMark)
                    AITryBoard[i, j] = TryHumanWight;
                else AITryBoard[i, j] = TryAIWeight;
            }
            for (var i = 0; i < 3; i++)
                count += (AITryBoard[i, 0] + AITryBoard[i, 1] + AITryBoard[i, 2]) / 3;
            for (var j = 0; j < 3; j++)
                count += (AITryBoard[0, j] + AITryBoard[1, j] + AITryBoard[2, j]) / 3;
            count += (AITryBoard[0, 0] + AITryBoard[1, 1] + AITryBoard[2, 2]) / 3;
            count += (AITryBoard[2, 0] + AITryBoard[1, 1] + AITryBoard[0, 2]) / 3;
            
            return count;
        }

        private void MarkMap(int i,int j)
        {
            tempBoard[i, j] = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == AIMark ? PlayerMark : AIMark;
        }

        private void UnMarkMap(int i, int j)
        {
            tempBoard.SetPositionEmpty(i, j);
            CurrentPlayer = CurrentPlayer == AIMark ? PlayerMark : AIMark;
        }

        #endregion
        
    }

}
