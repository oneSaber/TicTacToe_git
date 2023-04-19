using UILogicView;
using UnityEngine;

namespace GameModel
{
    [CreateAssetMenu(fileName = "GamePrefabConfig", menuName = "ScriptableObjects/GamePrefabConfig", order = 1)]
    public class GamePrefabConfig:ScriptableObject
    {
        public Canvas canvas;
        public StartView startView;
        public ChessBoardView chessBoardView;
    }
}