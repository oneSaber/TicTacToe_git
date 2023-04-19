using System.Collections.Generic;

namespace GameControl
{
    /// 棋盘类，一个 n * n 的int 矩阵，0表示没有落子，1表示 先手player1, 2 表示后手 player2
    /// i 表示横坐标，j 表示纵坐标
    public class GameChessBoard
    {
        private int[,] chessBoard;
        private int boardWidth;
        private int winCount;
        
        public GameChessBoard(int width, int win)
        {
            boardWidth = width;
            winCount = win;
            chessBoard = new int[boardWidth, boardWidth];
            for(int i = 0 ; i< boardWidth;i++)
            {
                for (int j = 0; j < boardWidth; j++)
                    chessBoard[i, j] = 0;
            }
        }

        public int this[int i, int j]
        {
            get => GetBoardPositionMark(i, j, out var mark) ? mark : -1;
            set => MarkPosition(i, j, value);
        }

        public bool SetPositionEmpty(int i, int j)
        {
            chessBoard[i, j] = 0;
            return true;
        }
        /// <summary>
        /// 标记棋盘的某个位置
        /// </summary>
        public bool MarkPosition(int i, int j, int mark)
        {
            if (mark != 1 && mark != 2)
                return false;
            if (i >= boardWidth || j >= boardWidth)
                return false;
            if (chessBoard == null)
                return false;
            if (chessBoard[i, j] != 0) return false;
            chessBoard[i, j] = mark;
            return true;
        }

        /// <summary>
        /// 获得棋盘上某个点的标记
        /// </summary>
        public bool GetBoardPositionMark(int i, int j, out int mark)
        {
            mark = -1;
            if (i >= boardWidth || j >= boardWidth)
                return false;
            if (chessBoard == null)
                return false;
            mark = chessBoard[i, j];
            return true;
        }

        // true 表示已经有结果了，0 表示平局，1表示 棋手1获胜，2表示棋手2获胜
        // false 表示还没有分出结果
        // 因为井字棋总共只有八种获胜情况，且每一次落子后都要调用这个函数进行检查，所以一旦某一种获胜情况满足，则表示该玩家获胜
        public bool GetBoardResult(out int winner)
        {
            winner = 0;
            var empty = false;
            // 检查棋盘是否还有空位
            for(int i = 0 ; i< boardWidth;i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    if (chessBoard[i, j] == 0)
                        empty = true;
                    else if (BeginCheckWin(i, j, winCount))
                    {
                        winner = chessBoard[i, j];
                        return true;
                    }
                }
            }

            return !empty;
        }

        private bool BeginCheckWin(int si, int sj, int needCount)
        {
            var startMark = chessBoard[si, sj];
            var upCount = 0;
            var downCount = 0;
            var leftCount = 0;
            var rightCount = 0;
            var leftUpCount = 0;
            var leftDownCount = 0;
            var rightUpCount = 0;
            var rightDownCount = 0;

            // 沿棋盘纵向搜索
            var offset = 1;
            while (sj + offset < boardWidth && chessBoard[si, sj + offset] == startMark)
            {
                upCount++;
                offset++;
            }

            offset = 1;
            while (sj - offset >=0 && chessBoard[si, sj - offset] == startMark)
            {
                downCount++;
                offset++;
            }

            if (upCount + downCount + 1 >= needCount) return true;

            // 沿棋盘横向搜索
            offset = 1;
            while (si + offset < boardWidth && chessBoard[si+ offset, sj] == startMark)
            {
                rightCount++;
                offset++;
            }

            offset = 1;
            while (si - offset>=0 && chessBoard[si - offset,sj] == startMark)
            {
                leftCount++;
                offset++;
            }

            if (leftCount + rightCount + 1 >= needCount) return true;
            
            // 沿棋盘左上到右下搜索
            offset = 1;
            while (si - offset >= 0 && sj + offset < boardWidth && chessBoard[si - offset, sj + offset] == startMark)
            {
                offset++;
                leftUpCount++;
            }

            offset = 1;
            while (si+offset<boardWidth && sj- offset>=0 && chessBoard[si+offset,sj-offset] == startMark)
            {
                offset++;
                rightDownCount++;
            }

            if (leftUpCount + rightDownCount + 1 >= needCount) return true;

            // 沿棋盘左下到右上搜索
            offset = 1;
            while (si - offset >= 0 && sj - offset >= 0 && chessBoard[si - offset, sj - offset] == startMark)
            {
                offset++;
                leftDownCount++;
            }

            offset = 1;
            while (si+offset < boardWidth && sj+ offset < boardWidth && chessBoard[si+offset,sj+offset] == startMark )
            {
                offset++;
                rightUpCount++;
            }

            if (leftDownCount + rightUpCount + 1 >= needCount) return true;
            return false;
        }

        public List<(int, int)> GetEmptyPosition()
        {
            var emptyPosition = new List<(int, int)>();
            for(int i = 0 ; i< boardWidth;i++)
            {
                for (int j = 0; j < boardWidth; j++)
                    if(chessBoard[i,j] == 0)
                        emptyPosition.Add((i,j));
            }

            return emptyPosition;
        }
        
        

        public GameChessBoard(GameChessBoard otherBoard)
        {
            boardWidth = otherBoard.boardWidth;
            winCount = otherBoard.winCount;
            chessBoard = new int[boardWidth, boardWidth];
            for(int i = 0 ; i< boardWidth;i++)
            {
                for (int j = 0; j < boardWidth; j++)
                    chessBoard[i, j] = otherBoard.chessBoard[i, j];
            }
        }
    }
    
    public static class GameChessBoardFactor
    {
        public static GameChessBoard CreateTempBoard(GameChessBoard board)
        {
            var newBoard = new GameChessBoard(board);
            return newBoard;
        }
    }
}