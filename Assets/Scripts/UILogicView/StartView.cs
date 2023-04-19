using System;
using GameControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UILogicView
{
    public class StartView: UIView
    {
        public Button BeginGame;
        public Button ExitGame;

        public void Init(GlobalGameControl control)
        {
            BeginGame.onClick.AddListener(()=>{
                control.StartNewGame();
                UIModule.DestroyPanel(viewName);
            });
            // 退出游戏
            ExitGame.onClick.AddListener(()=>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else 
                Application.Quit();
#endif
            });
        }
        
        
    }
}