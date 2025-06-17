using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using ClueServer;
using System.Reflection.Metadata;

class Program
{
    static List<string> connectedPlayers = new List<string>();
    static List<NetworkStream> clientStreams = new List<NetworkStream>();
    static List<TcpClient> connectedClients = new List<TcpClient>();
    static object lockObj = new object();
    static int maxPlayers = 6;
    static GameState? game;// 

    static void Main(string[] args)
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        Console.WriteLine("서버 IP 주소:");
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                Console.WriteLine($" - {ip}:{5000}");
            }
        }

        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("서버 시작됨. 클라이언트 접속 대기 중...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();

            lock (lockObj)
            {
                if (connectedPlayers.Count >= maxPlayers)
                {
                    Console.WriteLine("접속 인원 초과: 연결 거부됨");

                    NetworkStream tempStream = client.GetStream();
                    byte[] fullMsg = Encoding.UTF8.GetBytes("❌ 인원이 가득 찼습니다.");
                    tempStream.Write(fullMsg, 0, fullMsg.Length);
                    client.Close();
                    continue;
                }
            }

            Console.WriteLine("클라이언트 연결됨.");
            Thread t = new Thread(() => HandleClient(client));
            t.Start();
        }

    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[2048];
        string playerName = "";

        try
        {
            while (true)
            {
                
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                string[] messages = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var msg in messages)
                {
                    string trimmed = msg.Trim(); 
                    string[] parts = trimmed.Split('|');
                    string command = parts[0];

                        switch (command)
                    {
                        case "NICKNAME":
                            playerName = parts[1];
                            connectedPlayers.Add(playerName);
                            clientStreams.Add(stream);
                            connectedClients.Add(client);
                            Console.WriteLine($"플레이어 입장: {playerName}");
                            BroadcastMessage($"📢 {playerName}님이 입장하였습니다!");
                            BroadcastPlayerList();
                            BroadcastPlayerCount();
                            // 인원이 가득 차면 자동으로 게임 시작
                            if (connectedPlayers.Count == maxPlayers && game == null)
                            {
                                Console.WriteLine("🟢 인원 충족됨. 게임 시작 전송...");
 
                                game = GameInitializer.CreateNewGame(connectedPlayers);
                                game.InitializeCards();       // 카드 초기화
                                game.distributeCards();     // 카드 분배

                                var serializer = new DataContractJsonSerializer(typeof(GameState));
                                string json;
                                using (var ms = new MemoryStream())
                                {
                                    
                                    serializer.WriteObject(ms, game);
                                    json = Encoding.UTF8.GetString(ms.ToArray());
                                }
                                //BroadcastRaw($"GAME_START|{json}\n");
                                BroadcastCommand("GAME_START", json);
                            }


                            break;

                        case "MAX_PLAYERS":
                            if (connectedPlayers.Count == 1 && int.TryParse(parts[1], out int newMax))
                            {
                                maxPlayers = newMax;
                                Console.WriteLine($"🛠️ 최대 접속 인원 설정됨: {maxPlayers}");
                                
                            }
                            BroadcastPlayerCount();
                            
                            
                            break;

                        case "CHAT":
                            string from = parts[1];
                            string text = parts.Length > 2 ? parts[2] : "";
                            Console.WriteLine($"💬 {from}: {text}");
                            BroadcastMessage($"💬 {from}: {text}");
                            break;

                        case "GAME_START":
                            Console.WriteLine("클라이언트가 GAME_START 요청함 (서버 무시 또는 처리)");
                            break;
                        case "END_TURN":
                            if (game == null)
                            {
                                Console.WriteLine("⚠️  game이 초기화되지 않음. END_TURN 무시");
                                break;
                            }
                            lock (lockObj)
                            {
                                //Console.WriteLine($"[DEBUG] Before Change: Turn = {game.CurrentTurn}, Hash = {game.GetHashCode()}");

                                game.CurrentTurn = (game.CurrentTurn + 1) % game.TotalPlayers;

                                //Console.WriteLine($"[DEBUG] After Change: Turn = {game.CurrentTurn}, Hash = {game.GetHashCode()}");

                                BroadcastRaw($"TURN|{game.CurrentTurn}");
                                //Console.WriteLine($"[Server] Broadcast TURN|{game.CurrentTurn} 发出完成");
                                int count = clientStreams.Count;
                                //Console.WriteLine($"[Server] 向 {count} 个客户端广播 TURN");
                            }
                            break;
                        case "MOVE":
                            if (parts.Length >= 4 &&
                                int.TryParse(parts[1], out int moveId) &&
                                int.TryParse(parts[2], out int x) &&
                                int.TryParse(parts[3], out int y))
                            {
                                Console.WriteLine($"📍 MOVE from Player {moveId} to ({x},{y})");
                                BroadcastRaw($"MOVE|{moveId}|{x}|{y}");
                            }
                            break;
                        case "SUGGEST":
                            string suggestionText = parts[1]; // eg. Player1: Green가 Hall에서 Rope로 죽였다.
                           
                            BroadcastCommand("SUGGEST", suggestionText);
                           // Console.WriteLine($"[DEBUG] SUGGEST 广播内容: {suggestionText}");
                            break;
                        case "FINAL_SUGGEST":

                            if (parts.Length == 5)
                            {
                                string id = parts[1];
                                string man = parts[2];
                                string weapon = parts[3];
                                string room = parts[4];

                                BroadcastRaw($"FINAL_SUGGEST|{id}|{man}|{weapon}|{room}");

                                if (game != null)
                                {
                                    bool isCorrect =
                                        game.answer[0].name == man &&
                                        game.answer[1].name == weapon &&
                                        game.answer[2].name == room;

                                    if (isCorrect)
                                    {
                                        BroadcastRaw($"PLAYER_WIN|{id}");
                                    }
                                    else
                                    {
                                        BroadcastRaw($"PLAYER_FAIL|{id}");
                                    }
                                }
                            }
                            break;

                        case "SUGGEST_NO_REPLY":
                            if (parts.Length == 2)
                            {
                                string to = parts[1];
                                BroadcastRaw($"SUGGEST_NO_REPLY|{to}");
                            }
                            break;
                        case "SUGGEST_REPLY":
                            // SUGGEST_REPLY|fromId |to|type|name
                            if (parts.Length == 5)
                            {
                                string fromId = parts[1];
                                string to = parts[2];
                                string type = parts[3];
                                string name = parts[4];

                                Console.WriteLine($"[SERVER] {fromId} -> {to} 에게 카드 '{name}' 공개");

                                int targetIndex = int.Parse(to);
                                string fullMessage = $"SUGGEST_REPLY|{fromId}|{to}|{type}|{name}\n";
                                try
                                {
                                    lock (lockObj)
                                    {
                                        clientStreams[targetIndex].Write(Encoding.UTF8.GetBytes(fullMessage));
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine($"[SERVER] ❌ SUGGEST_REPLY 전송 실패 (to {to})");
                                }

                                lock (lockObj)
                                {
                                    for (int i = 0; i < clientStreams.Count; i++)
                                    {
                                        if (i != targetIndex)
                                        {
                                            string notice = $"SUGGEST_REPLY_NOTICE|{fromId}|{to}\n";
                                            try { clientStreams[i].Write(Encoding.UTF8.GetBytes(notice)); } catch { }
                                        }
                                    }
                                }
                                BroadcastRaw($"SUGGEST_REPLY_LOG|{fromId}|{to}");
                                //BroadcastRaw($"SUGGEST_NO_REPLY|{to}");
                            }
                            break;
                        case "UPDATE_UI":
                            if (parts.Length == 2)
                            {
                                string who = parts[1];
                                BroadcastRaw($"UPDATE_UI|{who}");
                            }
                            break;
                        default:
                            Console.WriteLine($"⚠️ 알 수 없는 명령: {msg}");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("에러 발생: " + ex.Message);
        }
        finally
        {
            lock (lockObj)
            {
                connectedPlayers.Remove(playerName);
                clientStreams.Remove(stream);
                connectedClients.Remove(client);
            }

            try { stream.Close(); } catch { }
            try { client.Close(); } catch { }

            BroadcastMessage($"📤 {playerName}님이 퇴장하였습니다!");
            BroadcastPlayerList();
            BroadcastPlayerCount();
        }
    }

    static void BroadcastMessage(string msg)
    {
        BroadcastRaw(msg + "\n");
    }

    static void BroadcastRaw(string fullMsg)
    {
        if (!fullMsg.EndsWith("\n"))
            fullMsg += "\n";
        byte[] data = Encoding.UTF8.GetBytes(fullMsg);
        lock (lockObj)
        {
            foreach (var s in clientStreams)
            {
                try { s.Write(data, 0, data.Length); } catch { }
            }
        }
    }
    static void BroadcastCommand(string command, string data)
    {
        string message = $"{command}|{data}\n";
        BroadcastRaw(message);
    }
    static void BroadcastPlayerList()
    {
        string joined = string.Join(",", connectedPlayers);
        //BroadcastRaw($"PLAYER_LIST|{joined}\n");
        BroadcastCommand("PLAYER_LIST", joined);
        Console.WriteLine("PLAYER_LIST 전송됨.");
    }

    static void BroadcastPlayerCount()
    {
        string countOnly = $"{connectedPlayers.Count}/{maxPlayers}";
        BroadcastCommand("PLAYER_COUNT", countOnly);  
        Console.WriteLine("PLAYER_COUNT 전송됨.");
    }
}

