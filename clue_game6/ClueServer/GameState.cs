using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ClueServer
{
    [DataContract]
    public class GameState
    {
        [DataMember] public int TotalPlayers { get; set; }

        [DataMember] public Player[] Players { get; set; } = Array.Empty<Player>();
        [DataMember] public int CurrentTurn { get; set; } = 0;// 当前轮次，从0开始计数

        [DataMember] public Card[] answer { get; set; } = new Card[3];

        [DataMember] public List<Card> Deck { get; set; } = new();

        [DataMember] public List<Card> openCard { get; set; } = new();

        [DataMember] public List<List<MyPoint>> clue_map_point_net { get; set; } = new();

        public void InitializeCards()
        {
            string[] types = { "mans", "weapon", "room" };
            string[] man = { "Green", "Mustard", "Peacock", "Plum", "Scarlett", "White" };
            string[] weapon = { "촛대", "파이프", "리볼버", "밧줄", "렌치", "단검" };
            string[] room = { "주방", "공부방", "무도회장", "온실", "식당", "당구장", "서재", "라운지", "홀" };

            List<string[]> listDeck = new List<string[]> { man, weapon, room };
            Random rand = new Random();

            answer[0] = new Card(types[0], man[rand.Next(man.Length)]);
            answer[1] = new Card(types[1], weapon[rand.Next(weapon.Length)]);
            answer[2] = new Card(types[2], room[rand.Next(room.Length)]);

            Deck.Clear();
            for (int i = 0; i < types.Length; i++)
            {
                foreach (var temp in listDeck[i])
                {
                    if (temp != answer[i].Name)
                        Deck.Add(new Card(types[i], temp));
                }
            }

            Deck = Deck.OrderBy(x => rand.Next()).ToList();
        }

        public void distributeCards()
        {
            int playerCount = Players.Length;
            int cardCount = Deck.Count;
            int cardsPerPlayer = cardCount / playerCount;

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

            while (Deck.Count > 0)
            {
                openCard.Add(Deck[0]);
                Deck.RemoveAt(0);
            }
        }
    }

    [DataContract]
    public class Player
    {
        [DataMember] public string name { get; set; } = "";
        [DataMember] public int id { get; set; }
        [DataMember] public List<Card> hands { get; set; } = new();
        [DataMember] public int x { get; set; }
        [DataMember] public int y { get; set; }
        [DataMember] public bool isTurn { get; set; }
        [DataMember] public bool isAlive { get; set; } = true;
        [DataMember] public bool isInRoom { get; set; } = false;
        [DataMember] public bool isFinalRoom { get; set; } = false;
        [DataMember] public bool hasSuggested { get; set; } = false;
        [DataMember] public bool hasRolled { get; set; } = false;
        [DataMember] public string[] clueBox { get; set; } = new string[3];

        [DataMember] public bool[] manBox { get; set; } = new bool[6];
        [DataMember] public bool[] weaponBox { get; set; } = new bool[6];
        [DataMember] public bool[] roomBox { get; set; } = new bool[9];
    }

    [DataContract]
    public class Card
    {
        [DataMember] public string type { get; set; } = "";
        [DataMember] public string name { get; set; } = "";

        public Card() { }

        public Card(string type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Type => type;
        public string Name => name;
    }

    [DataContract]
    public class MyPoint
    {
        [DataMember] public int X { get; set; }
        [DataMember] public int Y { get; set; }
    }
}
