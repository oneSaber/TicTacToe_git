# TicTacToe_git
 一个井字棋项目，简单难度采用随机取点，困难难度使用极大极小值算法选择结果

 ## 项目结构

 ### Assets
存放所有工程资源的文件
#### Assets/ArtWork
存档美术资源的文件夹
#### Assets/Resources
存放项目资源的文件夹，Configs存放内存放游戏的配置文件，Scenes文件夹存放项目的场景资源，UI文件夹存放用到的UI预制体
#### Assets/Scripts
存放项目脚本的文件夹

- GameControl 文件夹存放游戏的逻辑代码
- GameModel 文件存放游戏的配置代码
- UILogicView 存放游戏的UI控制代码

### 代码调用顺序

当游戏启动的时候，GlobalGameControl会首先被调用，这个脚本会创建开始菜单UI。

通过点击开始游戏，调用GlobalGameControl的StartNewGame接口，会创建棋盘界面，同时销毁开始菜单界面

棋盘界面首先需要选择先后手和AI难度，选择完成后，调用GlobalGameControl的 CreateNewGameSystem接口，创建一个对局系统：GameSystem

GameSystem 会创建一起GameChessBoard 棋盘实例，创建一个ChessPlayerAI实例，每一局游戏都会创建一个新的GameSystem实例，以及全新的棋盘和对局AI

当玩家点击棋盘的时候，会调用GameSystem的 OnPlayerMark 接口 对逻辑棋盘进行标记，，然后检查游戏是否结束，如果游戏没有结束，调用AI实例，计算AI的落点位置后
落子，并调用UI的回调刷新棋盘
