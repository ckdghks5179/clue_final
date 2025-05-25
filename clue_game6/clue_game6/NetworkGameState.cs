using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace clue_game6
{
    [DataContract]
    public class NetworkGameState
    {
        [DataMember] public int TotalPlayers { get; set; }
        [DataMember] public List<NetworkPlayer> Players { get; set; }
        [DataMember] public List<NetworkCard> openCard { get; set; }
    }

    [DataContract]
    public class NetworkPlayer
    {
        [DataMember] public int id;
        [DataMember] public string name;
        [DataMember] public List<NetworkCard> hands;
        [DataMember] public int x;
        [DataMember] public int y;
        [DataMember] public bool isTurn;
    }

    [DataContract]
    public class NetworkCard
    {
        [DataMember] public string type;
        [DataMember] public string name;
    }

}
