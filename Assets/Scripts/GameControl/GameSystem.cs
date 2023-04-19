using System;

namespace GameControl
{
    // 控制一个单局游戏的系统，当开始一局游戏的时候，由这个系统初始化棋盘，重置下棋AI
    public class GameSystem
    {
        private GameChessBoard board;
        private ChessPlayerAI AI;

        public int playerMark;
        public int AIMark;

        public bool finish;
        public int winner;

        public Action UpdateChessView;
        /// <summary>
        /// 开始一局新游戏
        /// </summary>
        /// <param name="player">玩家选择是1，还是2，1先走</param>
        public void BeginNewGame(int player,int aiLevel)
        {
            board = new GameChessBoard(3, 3);
            AI = new ChessPlayerAI();
            AI.AILevel = aiLevel;
            finish = false;
            playerMark = player;
            AIMark = player == 1 ? 2 : 1;
            AI.AIMark = AIMark;
            AI.PlayerMark = playerMark;

        }

        public void AiDrop()
        {
            if(AI.GetAIChoose(board, out var i, out var j))
            {
                Drops(i,j,AIMark);
            }
        }

        /// <summary>
        /// UI 点击棋盘按钮的回调
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void OnPlayerMark(int i, int j)
        {
            Drops(i,j,playerMark);
            if (!finish)
            {
                if(AI.GetAIChoose(board, out var ai, out var aj))
                {
                    Drops(ai,aj,AIMark);
                }
            }
            
        }

        private void Drops(int i, int j, int mark)
        {
            if (board.MarkPosition(i, j, mark))
            {
                // 调用UI接口标记结果
                finish = board.GetBoardResult(out winner);
                UpdateChessView.Invoke();
                return;
            }

            // 发生异常，游戏失败
            finish = true;
        }

        public int GetBoardMark(int i, int j)
        {
            if (board.GetBoardPositionMark(i, j, out var mark))
                return mark;
            return 0;
        }

        public void PlayerSurrender()
        {
            finish = true;
            winner = AIMark;
            UpdateChessView.Invoke();
        }
    }

}
