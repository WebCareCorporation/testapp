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

        public static Dictionary<string, Dictionary<string, string>> gameUserTurn = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, string> cardThrown = new Dictionary<string, string>();

        public GameHub()
        {
            if (!cards.Any())
            {
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
        }

        public void SendOfflineMessage(string sender, string receiver, string message)
        {

        }

        public void SendMessage(string groupName, string user, string message)
        {
            Clients.Group(groupName, Context.ConnectionId).sendMessage(user, message);
        }

        public void FirstTurnMessage(string groupName, string user)
        {
            Clients.Group(groupName, Context.ConnectionId).FirstTurn(user + " has Hukam A. His turn is first.");
        }

        public void ThrowCard(int card, string username, string groupname, string lastCard, string cardTurnType)
        {
            string cthrown = "";

            Dictionary<string, string> userturn = gameUserTurn[groupname];

            userturn[username] = "Done";

            gameUserTurn[groupname] = userturn;

            string throwncard = cards[card];
            string cardName = throwncard.Split('?')[1];
            string cardType = cardName.Split('-')[1];

            if (cardTurnType == cardType)
            {
                if (lastCard == "false")
                {
                    if (cardThrown.Where(x => x.Key == groupname).Any())
                    {
                        cthrown = cardThrown[groupname];

                        cthrown = cthrown + "?" + username + ":" + card;

                        cardThrown[groupname] = cthrown;
                    }
                    else
                    {
                        cthrown = username + ":" + card;
                        cardThrown[groupname] = cthrown;
                    }
                }
                else
                {
                    // If last card, remove user from turn list.
                    userturn.Remove(username);
                    gameUserTurn[groupname] = userturn;
                }


                Clients.Group(groupname).thrownCard(user, throwncard);




                List<KeyValuePair<string, string>> pending = userturn.Where(x => x.Value == "Pending").ToList();

                if (pending.Any())
                {
                    KeyValuePair<string, string> nextUser = pending.First();

                    string nextConnId = user[nextUser.Key];

                    Clients.Group(groupname).FirstTurn("Next turn : " + nextUser.Key);

                    Clients.Client(nextConnId).yourTurn(cardType);
                }
                else
                {
                    Clients.Group(groupname).turnComplete();

                    string largestCardUser = "";
                    int largestcard = GetLargestCard(groupname, out largestCardUser);

                    Dictionary<string, string> uTurn = new Dictionary<string, string>();
                    uTurn.Add(largestCardUser, "Pending");
                    
                    List<KeyValuePair<string, string>> Done = userturn.Where(x => x.Value == "Done").ToList();

                    int index = Done.FindIndex(x => x.Key == largestCardUser);

                    int length = Done.Count;

                    for (int i = index + 1; i < length; i++)
                    {

                        var us= Done.ElementAt(i);

                        uTurn.Add(us.Key, "Pending");
                    }

                    for (int i =0; i < index; i++)
                    { 
                        var us = Done.ElementAt(i);

                        uTurn.Add(us.Key, "Pending");
                    }

                    foreach (var d in Done)
                    {
                        userturn[d.Key] = "Pending";
                    }
                    gameUserTurn[groupname] = uTurn;
                }
            }
            else
            {
                Clients.Group(groupname).thokaGiven();


                // Get largest Card 
                string largestCardUser = "";
                int largestcard = GetLargestCard(groupname,out largestCardUser);

                cthrown = cardThrown[groupname];

                cthrown = cthrown + "?" + username + ":" + card;

            
              

                List<KeyValuePair<string, string>> Done = userturn.Where(x => x.Value == "Done").ToList();
                foreach (var d in Done)
                {
                    userturn[d.Key] = "Pending";
                }
                gameUserTurn[groupname] = userturn;
            }
        }





        public int GetLargestCard(string groupname,out string largestCardUser)
        {
            // Get largest Card
            int largestcard = 0;
            largestCardUser = "";
            string ct = cardThrown[groupname];
            string[] cts = ct.Split('?');
            foreach (string c in cts)
            {
                int cardNumber = Convert.ToInt32(c.Split(':')[1]);
                if (cardNumber == 1 || cardNumber == 14 || cardNumber == 27 || cardNumber == 40)
                {
                    largestcard = cardNumber;
                    largestCardUser = c.Split(':')[0];
                    break;
                }
                else
                {
                    if (cardNumber > largestcard)
                    {
                        largestcard = cardNumber;
                        largestCardUser = c.Split(':')[0];
                    }
                }
            }

            return largestcard;
        }

        public void InformTurn()
        {

        }

        public void Register(string name)
        {
            string connectionId = Context.ConnectionId;
            if (user.Any(x => x.Key == name))
            {
                user.Remove(name);
                string groupName = GRUSRs[name];
                GRUSRs.Remove(name);
                int grpCnt = groups[groupName];
                groups[groupName] = grpCnt - 1;
                //Clients.Client(Context.ConnectionId).sendCards(CRDUSRS[name]);
                // return;
            }
            user.Add(name, connectionId);
            Clients.Client(Context.ConnectionId).registered(name);
            JoinGame(name);
        }


        public void JoinGame(string name)
        {
            try
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
                Clients.All.informGroupName(groupName);
                if (groups[groupName] > 1)
                {
                    Clients.All.sendConfirm("started distri");
                    StartGame(grpCnt, groupName);
                }
            }
            catch (Exception ex)
            {
                Clients.All.sendConfirm("join game exception" + ex.Message);
            }
        }
        public void StartGame(int grpCnt, string groupName)
        {
            try
            {
                Dictionary<int, string> randomCards = DistCards();
                int count = 52 / grpCnt;
                IEnumerable<KeyValuePair<string, string>> allUsrs = GRUSRs.Where(x => x.Value == groupName);
                Clients.All.sendConfirm("distri done");
                int userCnt = 0;
                int start = 0;
                int end = 0;

                Dictionary<string, string> userTurn = new Dictionary<string, string>();

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
                    // CRDUSRS[u.Key] = cards;
                    Clients.Client(connId).sendCards(cards);

                    userCnt = userCnt + 1;


                    userTurn.Add(u.Key, "Active");
                }
                gameUserTurn.Add(groupName, userTurn);
            }
            catch (Exception ex)
            {
                Clients.All.sendConfirm("exception" + ex.Message);
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