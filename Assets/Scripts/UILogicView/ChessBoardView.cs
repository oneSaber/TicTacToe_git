using System;
using GameControl;
using UILogicView.Binder;
using UnityEngine;
using UnityEngine.UI;

namespace UILogicView
{
    [Serializable]
    public class ChessBoardView: UIView
    {
        public ChessBoardBinder Binder;
        

        private GlobalGameControl control;
        private GameSystem currentGameSystem;

        public void Init(GlobalGameControl control)
        {
            Binder = GetComponent<ChessBoardBinder>();
            this.control = control;
            Binder.ResultPanel.SetActive(false);
            Binder.IconPanel.SetActive(false);
            Binder.OperationPanel.SetActive(false);
            
            // InitBeginButton
            InitBeginPanel();
          
        }

        #region Init

        private void InitBeginPanel()
        {
            Binder.BeginPanel.SetActive(true);
            Binder.ChessPanel.SetActive(false);
            Binder.StartGameBtn.onClick.AddListener(() =>
            {
                var playerMaker = Binder.ChoosePlayerMaker.value+1;
                var aiLevel = Binder.ChooseAILevel.value+1;
                currentGameSystem = this.control.CreateNewGameSystem(playerMaker, aiLevel);
                StartGame();
            });
        }

        private void InitChessGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var index = (2 - i) * 3 + j;
                    var ci = i;
                    var cj = j;
                    var slot = Binder.ButtonGrid.GetChild(index);
                    slot.Find("Icon").gameObject.SetActive(false);
                    var button = slot.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        currentGameSystem.OnPlayerMark(ci,cj);
                        button.interactable = false;
                    });
                }
            }

            currentGameSystem.UpdateChessView += UpdateChessBoard;
        }

        #endregion

        private void StartGame()
        {
            Binder.BeginPanel.SetActive(false);
            Binder.ChessPanel.SetActive(true);
            // InitGrid
            InitChessGrid();
            Binder.IconPanel.SetActive(true);
            Binder.PlayerIcon.sprite = currentGameSystem.playerMark == 1 ? Binder.Player1Icon : Binder.Player2Icon;
            Binder.AIIcon.sprite = currentGameSystem.AIMark == 1 ? Binder.Player1Icon : Binder.Player2Icon;
            if(currentGameSystem.AIMark == 1) currentGameSystem.AiDrop();
            InitOperationPanel();
        }


        public void InitOperationPanel()
        {
            Binder.OperationPanel.SetActive(true);
            Binder.ReturnStartPanel.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.CreateStatView();
            });
            Binder.ReStartGame.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.StartNewGame();

            });
            Binder.Surrender.onClick.AddListener((() =>
            {
                currentGameSystem.PlayerSurrender();
            }));
        }
        private void UpdateChessBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var index = (2 - i) * 3 + j;
                    var slot = Binder.ButtonGrid.GetChild(index);
                    var img = slot.Find("Icon").GetComponent<Image>();
                    var mark = currentGameSystem.GetBoardMark(i, j);
                    if (mark != 0)
                    {
                        img.sprite = mark == 1 ? Binder.Player1Icon : Binder.Player2Icon;
                        img.gameObject.SetActive(true);
                    }
                }
            }

            if (currentGameSystem.finish)
            {
                // showFinishPanel
                ShowFinishPanel();
            }
        }

        private void ShowFinishPanel()
        {
            var winner = currentGameSystem.winner;
            Binder.WinWord.SetActive(winner == currentGameSystem.playerMark);
            Binder.FailureWord.SetActive(winner == currentGameSystem.AIMark);
            Binder.TieWord.SetActive(winner == 0);
            Binder.ResultPanel.SetActive(true);
            Binder.ResultRestart.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.StartNewGame();
            });
            Binder.ResultReturnStartPanel.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.CreateStatView();
            });
        }

        
    }
}