﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace clue_game6
{
    public class Player
    {
        public string name;
        public int id;

        public List<Card> hands; //가지고 있는 카드

        public int x; //열
        public int y; //행

        public bool isAlive = true;
        public bool isTurn = false;
        public bool isInRoom = false;
        public bool isFinalRoom = false;
        public bool hasRolled = false; //주사위 굴림 여부
        public bool hasSuggested = false; //추리 여부
        public bool usedSecretPass = false; //비밀통로 사용 여부
        public string[] clueBox = { "", "", "" }; //추리 저장 배열

        public bool[] manBox = new bool[6];
        public bool[] weaponBox = new bool[6];
        public bool[] roomBox = new bool[9];
    }


    public class Card
    {
        public string type; //player, weapon, room
        public string name;

        public Card(string type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public class PlayerNoteData
    {
        public bool[] SuspectChecks = new bool[6]; // Green ~ White
        public bool[] WeaponChecks = new bool[6];  // 촛대 ~ 렌치
        public bool[] RoomChecks = new bool[9];    // 9개 방
    }

    public class GameState
    {

        public int[,] clue_map = new int[,]
        {
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 4, 1, 1, 0, 0, 1, 1, 1, 4, 1, 1, 1, 1, 0, 0, 1, 1, 1, 4, 1, 1},
                { 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 2, 1, 1, 1, 1},
                { 1, 1, 1, 1, 2, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 5, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 4, 1, 1, 1, 2, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 2, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 5, 1, 4, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 2, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 2, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 1, 1, 0, 0, 2, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 2, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                { 1, 1, 1, 4, 1, 1, 1, 0, 0, 1, 1, 1, 4, 1, 1, 0, 0, 1, 2, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 4, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}


        };

        public List<Point> roomTiles = new List<Point> //문 좌표
        {

           new Point(8, 4), new Point(15, 4), new Point(19, 4), new Point(4, 5), new Point(9, 6),
            new Point(14, 6), new Point(18, 8), new Point(7, 11), new Point(22, 11), new Point(20, 13),
            new Point(6, 14), new Point(17, 15), new Point(11, 17),new Point(12, 17), new Point(5, 18),
            new Point(14, 19), new Point(18, 20)

        };

        public List<Point> finalRoomTiles = new List<Point> //최종추리방 문 좌표
        {

            new Point(12, 9), new Point(10, 12), new Point(14, 12), new Point(12, 15)
        };

        public Dictionary<string, string> secretPass = new Dictionary<string, string>
        {
            {"주방", "공부방" },
            {"공부방", "주방" },
            {"라운지", "온실" },
            {"온실", "라운지" }
        };

        public Dictionary<Point, Point> secretPassPoints = new Dictionary<Point, Point>
        {
            { new Point(4, 6), new Point(18, 21) }, // 주방 -> 공부방
            { new Point(18, 21), new Point(4, 6) }, // 공부방 -> 주방
            { new Point(5, 19), new Point(19, 5) }, // 라운지 -> 온실
            { new Point(19, 5), new Point(5, 19) } // 온실 -> 라운지
        };

        public int TotalPlayers { get; set; }
        public Player[] Players { get; set; }

        public Card[] answer = new Card[3]; //정답 카드
        public List<Card> Deck = new List<Card>(); //덱
        public List<Card> openCard = new List<Card>(); //나눠주고 남아서 공개할 카드들

        public int CurrentTurn { get; set; } = 0;
        public Point[,] clue_map_point { get; set; }
        public int CurrentX { get; set; } = 0;

        public int CurrentY { get; set; } = 0;

        public Dictionary<int, PlayerNoteData> PlayerNotes = new Dictionary<int, PlayerNoteData>();

        public List<string> gameLog = new List<string>(); //로그 저장소

        public void initializeNote()
        {
            for (int i = 0; i < Players.Length; i++)
                PlayerNotes[i] = new PlayerNoteData();
        }

        public void AdvanceTurn()
        {
            int total = Players.Length;
            do
            {
                CurrentTurn = (CurrentTurn + 1) % total;
            } while (!Players[CurrentTurn].isAlive);

            //누구의 턴입니다 메시지박스 출력
            MessageBox.Show($"Player {CurrentTurn + 1}의 턴입니다.");
        }

        public static int[,] GetInitialMap()
        {
            return new int[25, 24];
        }

        public void InitializeCards()
        {
            string[] types = { "mans", "weapon", "room" };
            string[] man = { "Green", "Mustard", "Peacock", "Plum", "Scarlett", "White" };
            string[] weapon = { "촛대", "파이프", "리볼버", "밧줄", "렌치", "단검" };
            string[] room = { "주방", "공부방", "무도회장", "온실", "식당", "당구장", "서재", "라운지", "홀" };

            List<string[]> listDeck = new List<string[]> { man, weapon, room };
            //정답 카드 생성, 봉투에 넣기
            Random rand = new Random();
            answer[0] = new Card(types[0], man[rand.Next(0, man.Length)]);
            answer[1] = new Card(types[1], weapon[rand.Next(0, weapon.Length)]);
            answer[2] = new Card(types[2], room[rand.Next(0, room.Length)]);

            //나머지 카드를 덱에 삽입
            for (int i = 0; i < types.Length; i++)
            {
                foreach (string temp in listDeck[i])
                {
                    if (temp != answer[i].Name)
                        Deck.Add(new Card(types[i], temp));
                }
            }

            Deck = Deck.OrderBy(x => rand.Next()).ToList();
        }

        public void distributeCards() //카드 분배
        {
            int playerCount = Players.Length;
            int cardCount = Deck.Count;
            int cardsPerPlayer = cardCount / playerCount; //나눠줘야 할 카드 수
            int restCard = cardCount % playerCount; //나눠줘야 할 카드 수
            for (int i = 0; i < playerCount; i++)
            {
                Players[i].hands = new List<Card>();
                for (int j = 0; j < cardsPerPlayer; j++)
                {
                    if (Deck.Count > 0)
                    {
                        Players[i].hands.Add(Deck[0]);
                        Deck.RemoveAt(0);
                    }
                }
            }

            //남은 카드를 공개 카드에 추가
            while (Deck.Count > 0)
            {
                openCard.Add(Deck[0]);
                Deck.RemoveAt(0);
            }

            //디버그 창에 카드 출력
            foreach (var player in Players)
            {
                Console.WriteLine($"Player {player.id + 1} hands:");
                foreach (var card in player.hands)
                {
                    Console.WriteLine($"- {card.Type}: {card.Name}");
                }
            }

            //공개 카드 출력
            Console.WriteLine("Open Cards:");
            foreach (var card in openCard)
            {
                Console.WriteLine($"- {card.Type}: {card.Name}");
            }
        }

        public void AddLog(string log)
        {
            gameLog.Add(log);

            // 모든 폼의 로그 텍스트박스에 출력 추가
            foreach (var form in PlayerChoose.AllPlayerForms)
            {
                form.textBox1.AppendText(log + "\r\n");
            }
        }

        public void SaveLogToFile(string filePath)
        {
            try
            {
                File.WriteAllLines(filePath, gameLog);
                MessageBox.Show($"로그가 저장되었습니다:\n{filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그 저장 실패: " + ex.Message);
            }
        }

        public string GetRoomNameByPosition(int x, int y)
        {
            Point pos = new Point(y, x); // Point(col, row)

            Dictionary<Point, string> roomNames = new Dictionary<Point, string>
            {
                { new Point(4, 5), "주방" },

                { new Point(8, 4), "무도회장" }, { new Point(15, 4), "무도회장" }, { new Point(9, 6), "무도회장" }, { new Point(14, 6), "무도회장" },

                { new Point(5, 18), "라운지" },

                { new Point(7, 11), "식당" }, { new Point(6, 14), "식당" },

                { new Point(19, 4), "온실"},

                { new Point(11, 17), "홀"}, { new Point(12, 17), "홀"}, { new Point(14, 19), "홀"},

                { new Point(22, 11), "당구장" }, { new Point(18, 8), "당구장" },

                { new Point(20, 23), "서재" }, { new Point(17, 15), "서재" },

                { new Point(18, 20), "공부방" },
            };

            if (roomNames.ContainsKey(pos))
                return roomNames[pos];
            else
                return "(알 수 없음)";
        }

        public bool GetSecretPass(Point currentPos, out Point targetPos)
        {
            if (secretPassPoints.ContainsKey(currentPos))
            {
                targetPos = secretPassPoints[currentPos];
                return true;
            }
            targetPos = Point.Empty;
            return false;
        }


    }


}
