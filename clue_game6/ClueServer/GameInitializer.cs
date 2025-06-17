using System;
using System.Collections.Generic;

namespace ClueServer
{
    public static class GameInitializer
    {
        public static GameState CreateNewGame(List<string> playerNames)
        {
            int playerCount = playerNames.Count;

            var game = new GameState
            {
                TotalPlayers = playerCount,
                Players = new Player[playerCount],
                clue_map_point_net = GenerateMap(),
                CurrentTurn = 0 // ✅ 显式指定回合从 0 开始
            };

            // 初始坐标（与你客户端保持一致）
            int[] initialX = { 7, 17, 24, 0, 6, 19 };
            int[] initialY = { 0, 0, 7, 14, 23, 23 };

            for (int i = 0; i < playerCount; i++)
            {
                game.Players[i] = new Player
                {
                    id = i,
                    name = playerNames[i],
                    hands = new List<Card>(),
                    x = initialX[i],
                    y = initialY[i],
                    isTurn = (i == 0),

                    isInRoom = false,
                    isFinalRoom = false,
                    hasRolled = false,
                    hasSuggested = false,
                    clueBox = new string[3],
                    manBox = new bool[6],
                    weaponBox = new bool[6],
                    roomBox = new bool[9]
                };
            }
            
            game.InitializeCards();    
            game.distributeCards();     
            return game;
        }

        private static List<List<MyPoint>> GenerateMap()
        {
            var map = new List<List<MyPoint>>();
            for (int row = 0; row < 25; row++)
            {
                var rowList = new List<MyPoint>();
                for (int col = 0; col < 24; col++)
                {
                    rowList.Add(new MyPoint
                    {
                        X = 8 + col * 20,
                        Y = 8 + row * 16
                    });
                }
                map.Add(rowList);
            }
            return map;
        }
    }
}
