using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;

namespace WebApplication1
{
    public class GameHub : Hub
    {

        /// <summary>
        /// Key : Name
        /// Value : Connection ID
        /// </summary>
        public static Dictionary<string, string> user = new Dictionary<string, string>();

        public static Dictionary<int, string> cards = new Dictionary<int, string>();

        public static Dictionary<string, int> groups = new Dictionary<string, int>();

        /// <summary>
        /// Key : User Name
        /// Value : Game Name
        /// </summary>
        public static Dictionary<string, string> GRUSRs = new Dictionary<string, string>();

        public static Dictionary<string, string> CRDUSRS = new Dictionary<string, string>();
        public static Dictionary<string, Dictionary<int, string>> games = new Dictionary<string, Dictionary<int, string>>();

        public GameHub()
        {
            cards.Clear();
            cards.Add(1, "1?card-hukam-a");
            cards.Add(2, "2?card-hukam-2");
            cards.Add(3, "3?card-hukam-3");
            cards.Add(4, "4?card-hukam-4");
            cards.Add(5, "5?card-hukam-5");
            cards.Add(6, "6?card-hukam-6");
            cards.Add(7, "7?card-hukam-7");
            cards.Add(8, "8?card-hukam-8");
            cards.Add(9, "9?card-hukam-9");
            cards.Add(10, "10?card-hukam-10");
            cards.Add(11, "11?card-hukam-j");
            cards.Add(12, "12?card-hukam-q");
            cards.Add(13, "13?card-hukam-k");


            cards.Add(14, "14?card-heart-a");
            cards.Add(15, "15?card-heart-2");
            cards.Add(16, "16?card-heart-3");
            cards.Add(17, "17?card-heart-4");
            cards.Add(18, "18?card-heart-5");
            cards.Add(19, "19?card-heart-6");
            cards.Add(20, "20?card-heart-7");
            cards.Add(21, "21?card-heart-8");
            cards.Add(22, "22?card-heart-9");
            cards.Add(23, "23?card-heart-10");
            cards.Add(24, "24?card-heart-j");
            cards.Add(25, "25?card-heart-q");
            cards.Add(26, "26?card-heart-k");

            cards.Add(27, "27?card-chidi-a");
            cards.Add(28, "28?card-chidi-2");
            cards.Add(29, "29?card-chidi-3");
            cards.Add(30, "30?card-chidi-4");
            cards.Add(31, "31?card-chidi-5");
            cards.Add(32, "32?card-chidi-6");
            cards.Add(33, "33?card-chidi-7");
            cards.Add(34, "34?card-chidi-8");
            cards.Add(35, "35?card-chidi-9");
            cards.Add(36, "36?card-chidi-10");
            cards.Add(37, "37?card-chidi-j");
            cards.Add(38, "38?card-chidi-q");
            cards.Add(39, "39?card-chidi-k");

            cards.Add(40, "40?card-itt-a");
            cards.Add(41, "41?card-itt-2");
            cards.Add(42, "42?card-itt-3");
            cards.Add(43, "43?card-itt-4");
            cards.Add(44, "44?card-itt-5");
            cards.Add(45, "45?card-itt-6");
            cards.Add(46, "46?card-itt-7");
            cards.Add(47, "47?card-itt-8");
            cards.Add(48, "48?card-itt-9");
            cards.Add(49, "49?card-itt-10");
            cards.Add(50, "50?card-itt-j");
            cards.Add(51, "51?card-itt-q");
            cards.Add(52, "52?card-itt-k");
        }

        public void SendOfflineMessage(string sender, string receiver, string message)
        {

        }

        public void Register(string name)
        {
            string connectionId = Context.ConnectionId;
            if (user.Any(x => x.Key == name))
            {
                Clients.Client(Context.ConnectionId).sendCards(CRDUSRS[name]);
                return;
            }
            user.Add(name, connectionId);
            JoinGame(name);
        }


        public void JoinGame(string name)
        {
            string groupName = "";
            int grpCnt = 0;
            string connectionId = Context.ConnectionId;
            KeyValuePair<string, int> grp = groups.Where(x => x.Value < 6).FirstOrDefault();

            if (grp.Key != null)
            {
                groupName = grp.Key;
                grpCnt = grp.Value;
            }
            else
            {
                groupName = Guid.NewGuid().ToString().Substring(0, 5);

                groups.Add(groupName, 0);
                grpCnt = 0;
            }

            /// Add to groups for message
            Groups.Add(Context.ConnectionId, groupName);

            // Add to dicotionary
            GRUSRs.Add(name, groupName);

            groups[groupName] = grpCnt + 1;
            grpCnt = groups[groupName];
            if (groups[groupName] > 1)
            {
                StartGame(grpCnt, groupName);
            }

        }
        public void StartGame(int grpCnt, string groupName)
        {
            Dictionary<int, string> randomCards = DistCards();
            int count = 52 / grpCnt;
            IEnumerable<KeyValuePair<string, string>> allUsrs = GRUSRs.Where(x => x.Value == groupName);

            int userCnt = 0;
            int start = 0;
            int end = 0;
            foreach (KeyValuePair<string, string> u in allUsrs)
            {
                string cards = "";
                string connId = user[u.Key];
                if (userCnt != 0)
                {
                    start = count * userCnt;
                    end = count * (userCnt + 1);

                }
                else
                {
                    start = 0;
                    end = count;
                }

                for (int i = start; i < end; i++)
                {
                    KeyValuePair<int, string> pair = randomCards.ElementAt(i);
                    cards = cards + pair.Value + ";";
                }
                cards = cards.Substring(0, cards.Length - 1);
                CRDUSRS[u.Key] = cards;
                Clients.Client(connId).sendCards(cards);

                userCnt = userCnt + 1;
            }
        }

        public Dictionary<int, string> DistCards()
        {
            Dictionary<int, string> dividelist = new Dictionary<int, string>(cards);
            Dictionary<int, string> selected = new Dictionary<int, string>();

            Random random = new Random();
            int r = 0;
            while (dividelist.Count > 0)
            {
                KeyValuePair<int, string> pair = dividelist.ElementAt(r);
                selected.Add(pair.Key, pair.Value);
                
                dividelist.Remove(pair.Key);
                r = random.Next(dividelist.Count);
            }

            return selected;
        }
    }
}