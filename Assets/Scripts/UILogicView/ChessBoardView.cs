using System;
using GameControl;
using UnityEngine;
using UnityEngine.UI;

namespace UILogicView
{
    [Serializable]
    public class ChessBoardView: UIView
    {
        #region UI组件
        public Sprite Player1Icon;
        public Sprite Player2Icon;
        public GameObject ChessPanel;
        
        public GameObject BeginPanel;
        public Transform ButtonGrid;
        public Button ChoosePlayer1;
        public Button ChoosePlayer2;

        public GameObject IconPanel;
        public Image PlayerIcon;
        public Image AIIcon;

        public GameObject OperationPanel;
        public Button ReturnStartPanel;
        public Button ReStartGame;
        public Button Surrender;

        public GameObject ResultPanel;
        public GameObject WinWord;
        public GameObject FailureWord;
        public Button ResultRestart;
        public Button ResultReturnStartPanel;
        

        #endregion
        
        private GlobalGameControl control;
        private GameSystem currentGameSystem;

        public void Init(GlobalGameControl control)
        {
            this.control = control;
            ResultPanel.SetActive(false);
            IconPanel.SetActive(false);
            OperationPanel.SetActive(false);
            
            // InitBeginButton
            InitBeginPanel();
          
        }

        #region Init

        private void InitBeginPanel()
        {
            BeginPanel.SetActive(true);
            ChessPanel.SetActive(false);
            ChoosePlayer1.onClick.AddListener(() =>
            {
                currentGameSystem = this.control.CreateNewGameSystem(1);
                StartGame();
            });    
            ChoosePlayer2.onClick.AddListener(() =>
            {
                currentGameSystem = this.control.CreateNewGameSystem(2);
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
                    var slot = ButtonGrid.GetChild(index);
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
            BeginPanel.SetActive(false);
            ChessPanel.SetActive(true);
            // InitGrid
            InitChessGrid();
            IconPanel.SetActive(true);
            PlayerIcon.sprite = currentGameSystem.playerMark == 1 ? Player1Icon : Player2Icon;
            AIIcon.sprite = currentGameSystem.AIMark == 1 ? Player1Icon : Player2Icon;
            if(currentGameSystem.AIMark == 1) currentGameSystem.AiDrop();
        }

        private void UpdateChessBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var index = (2 - i) * 3 + j;
                    var slot = ButtonGrid.GetChild(index);
                    var img = slot.Find("Icon").GetComponent<Image>();
                    var mark = currentGameSystem.GetBoardMark(i, j);
                    if (mark != 0)
                    {
                        img.sprite = mark == 1 ? Player1Icon : Player2Icon;
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
            WinWord.SetActive(winner == currentGameSystem.playerMark);
            FailureWord.SetActive(winner == currentGameSystem.AIMark);
            ResultPanel.SetActive(true);
            ResultRestart.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.StartNewGame();
            });
            ResultReturnStartPanel.onClick.AddListener(() =>
            {
                UIModule.DestroyPanel(viewName);
                control.CreateStatView();
            });
        }

        
    }
}