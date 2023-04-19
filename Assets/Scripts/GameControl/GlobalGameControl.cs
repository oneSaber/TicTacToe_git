using System;
using GameModel;
using UILogicView;
using UnityEngine;

namespace GameControl
{
    // 全局控制器
    public class GlobalGameControl: MonoBehaviour
    {
        public GamePrefabConfig config;
        public Canvas Canvas;
        public void Start()
        {
            UIModule.Init();
            // 创建一个空的Canvas
            Canvas = Instantiate(config.canvas);
            Canvas.worldCamera = Camera.main;
            CreateStatView();
        }

        public void CreateStatView()
        {
            // 创建Start界面
            var startView = Instantiate(config.startView, Canvas.transform, false);
            startView.Init(this);
            UIModule.AddNewView(startView);
        }
        public void StartNewGame()
        {
            var chessBoardView = Instantiate(config.chessBoardView, Canvas.transform, false);
            chessBoardView.Init(this);
            UIModule.AddNewView(chessBoardView);
        }

        public GameSystem CreateNewGameSystem(int player)
        {
            var system = new GameSystem();
            system.BeginNewGame(player,1);
            return system;
        }
    }
}