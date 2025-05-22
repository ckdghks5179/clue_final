using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clue_game6
{


    public class Player
    {
        public string name;
        public int id;
        //public PictureBox player;

        public List<Card> hands; //가지고 있는 카드

        public int x; //열
        public int y; //행

        public bool isAlive = true;
        public bool isTurn = false;
        public bool isInRoom = false;
        public bool isFinalRoom = false;
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
                { 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 4, 1, 1, 0, 0, 1, 1, 1, 4, 1, 1, 1, 1, 0, 0, 1, 1, 1, 4, 1, 1},
                { 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 2, 1, 1, 1, 1},
                { 1, 1, 1, 1, 2, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 5, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
                { 1, 1, 1, 4, 1, 1, 1, 2, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 2, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 5, 1, 4, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0},
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
                { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1}
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

        public void initializeNote()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                PlayerNotes[i] = new PlayerNoteData();
            }
        }

        public void AdvanceTurn()
        {
            int total = Players.Length;
            do
            {
                CurrentTurn = (CurrentTurn + 1) % total;
            } while (!Players[CurrentTurn].isAlive);
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
            }//카드 할당 오류 수정

            
            Deck = Deck.OrderBy(x => rand.Next()).ToList();
        }

        public void distributeCards() //카드 분배
        {
            int playerCount = Players.Length;
            int cardCount = Deck.Count;
            int cardsPerPlayer = cardCount / playerCount; //나눠줘야 할 카드 수
            int  restCard = cardCount % playerCount; //나눠줘야 할 카드 수
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

    }


}
