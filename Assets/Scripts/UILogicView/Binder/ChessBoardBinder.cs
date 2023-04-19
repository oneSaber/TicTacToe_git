using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UILogicView.Binder
{
    public class ChessBoardBinder:MonoBehaviour
    {
        public Sprite Player1Icon;
        public Sprite Player2Icon;
        public GameObject ChessPanel;
        
        public GameObject BeginPanel;
        public Transform ButtonGrid;
        public Button StartGameBtn;
        public TMP_Dropdown ChoosePlayerMaker;
        public TMP_Dropdown ChooseAILevel;

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
        [Header("平局的提示语")]
        public GameObject TieWord;
        public Button ResultRestart;
        public Button ResultReturnStartPanel;
    }
}