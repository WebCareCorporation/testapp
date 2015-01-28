using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections;
using System.Timers;

namespace WebApplication1
{
    public class GameHub : Hub
    {
        public static Dictionary<string, string> uniqueIds = new Dictionary<string, string>();
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

        public static Dictionary<string, string> userToRemove = new Dictionary<string, string>();

        public static Dictionary<string, string> gameInPrgress = new Dictionary<string, string>();

        public static Dictionary<string, string> askTimeOut = new Dictionary<string, string>();

        public static Dictionary<string, string> userDiscQueue = new Dictionary<string, string>();
        public static Dictionary<string, string> msgQueue = new Dictionary<string, string>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void RegisterUser(string uniqueId, string userName)
        {
            if (user.Any(x => x.Key == userName))
            {
                Clients.Client(Context.ConnectionId).registerConfirm("false");
            }
            else
            {
                if (uniqueIds.Any(x => x.Key == uniqueId))
                {
                    string oldName = uniqueIds[uniqueId];
                    uniqueIds.Remove(uniqueId);
                    user.Remove(oldName);
                }
                uniqueIds.Add(uniqueId, userName);
                user.Add(userName, Context.ConnectionId);
                Clients.Client(Context.ConnectionId).registerConfirm("true");
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            KeyValuePair<string, string> u = user.Where(x => x.Value == Context.ConnectionId).FirstOrDefault();
            if (GRUSRs.Where(x => x.Key == u.Key).Any())
            {
                string grpName = GRUSRs[u.Key];



                if (stopCalled)
                {
                    GRUSRs.Remove(u.Key);
                    // Clients.Group(grpName).groupMessage("only removed from group:" + u.Key + " Left."); 

                    //if (askTimeOut.Where(x => x.Key == u.Key).Any())
                    //{
                    //    AsktimeOut(u.Key, grpName, "disc");
                    //}

                    if (gameUserTurn.Any(x => x.Key == grpName))
                    {
                        Dictionary<string, string> userturn = gameUserTurn[grpName];
                        userturn.Remove(u.Key);
                        string userList = (new Utility()).GetUserList(userturn);
                        Clients.Group(grpName).updateUserStatus(u.Key, "Disconnected");
                        gameUserTurn[grpName] = userturn;
                    }
                    Clients.Group(grpName).groupMessage("Diconnected : " + u.Key + " Left.");


                    if (GRUSRs.Where(x => x.Value == grpName).Count() == 1)
                    {
                        if (gameUserTurn.Where(x => x.Key == grpName).Any())
                        {
                            gameUserTurn.Remove(grpName);
                            Clients.Group(grpName).groupMessage("As " + u.Key + " has Left. So he/she declared as bhabo.");
                            Clients.Group(grpName).GameClosed();
                        }
                    }
                }
                else
                {  userDiscQueue.Add(u.Key, grpName);
                }
            }
            return base.OnDisconnected(stopCalled);
        }

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
            Clients.Group(groupName).groupMessage(user + " : " + message);
        }

        public void FirstTurnMessage(string groupName, string user)
        {
            Clients.Group(groupName).StartTimer(user, groupName, "hukam");
            if (!askTimeOut.Where(x => x.Key == user).Any())
            {
                askTimeOut.Add(user, groupName);
            }
            Clients.Group(groupName, Context.ConnectionId).FirstTurn(user + " has Hukam A. His turn is first.");
            Clients.Group(groupName).updateUserTurn(user);
        }

        public void AsktimeOut(string userName, string groupname, string card)
        {

            //if (userDiscQueue.Any(x => x.Value == groupname))
            //{
            //    if (userDiscQueue.Any(x => x.Key == userName))
            //    {
            //        userDiscQueue.Remove(userName);
            //    }
            //}

            //if (msgQueue.Any(x => x.Key == userName))
            //{
            //    msgQueue.Remove(userName);
            //}


            Clients.Group(groupname).resetTimer();

            if (askTimeOut.Where(x => x.Key == userName).Any())
            {
                askTimeOut.Remove(userName);
                if (askTimeOut.Where(x => x.Value == groupname).Any())
                {
                    List<KeyValuePair<string, string>> times = askTimeOut.Where(x => x.Value == groupname).ToList();
                    foreach (var t in times)
                    {
                        askTimeOut.Remove(t.Key);
                    }
                }
            }
            else
            {
                return;
            }
            Clients.Client(user[userName]).TimedOut();
            Dictionary<string, string> userturn = gameUserTurn[groupname];
            userturn.Remove(userName);
            gameUserTurn[groupname] = userturn;
            if (GRUSRs.Any(x => x.Key == userName))
            {
                GRUSRs.Remove(userName);
            }

            if (userturn.Count == 1)
            {

                Clients.Group(groupname).groupMessage("As " + userName + " has Left.So he/she declared as bhabo.");
                Clients.Group(groupname).GameClosed();
                cardThrown[groupname] = "";

                if (GRUSRs.Where(x => x.Value == groupname).Count() > 1)
                {
                    StartGame(GRUSRs.Where(x => x.Value == groupname).Count(), groupname);
                }
                else
                {
                    gameUserTurn.Remove(groupname);
                }
                return;
            }
            Clients.Group(groupname).groupMessage(userName + " Left.");
            List<KeyValuePair<string, string>> pending = userturn.Where(x => x.Value == "Pending").ToList();

            if (pending.Any())
            {
                KeyValuePair<string, string> nextUser = pending.First();

                string nextConnId = user[nextUser.Key];

                Clients.Group(groupname).FirstTurn("Next turn : " + nextUser.Key);
                if (card != "")
                {
                    Clients.Client(nextConnId).yourTurn(card);

                    Clients.Group(groupname).StartTimer(nextUser.Key, groupname, card);
                }
                else
                {
                    Clients.Client(nextConnId).startTurn();

                    Clients.Group(groupname).StartTimer(nextUser.Key, groupname, "");
                }
                if (!askTimeOut.Where(x => x.Key == nextUser.Key).Any())
                    askTimeOut.Add(nextUser.Key, groupname);

                //if (userDiscQueue.Any(x => x.Value == groupname))
                //{
                //    if (msgQueue.Any(x => x.Key == nextUser.Key))
                //    {
                //        msgQueue.Add(nextUser.Key, msgQueue[nextUser.Key] + ";YourTurn:" + card);
                //    }
                //    else
                //    {
                //        msgQueue.Add(nextUser.Key, "YourTurn:" + card);
                //    }
                //}

            }
            else
            {
                string largestCardUser = "";

                int largestcard = (new Utility()).GetLargestCard(groupname, cardThrown[groupname], out largestCardUser);


                string startTurn = user[largestCardUser];

                /// remove user if he is not largest or check if only 2 left
                if (userToRemove.Where(x => x.Value == groupname).Any())
                {
                    bool checklarge = false;
                    List<KeyValuePair<string, string>> uTR = userToRemove.Where(x => x.Value == groupname).ToList();
                    foreach (var u in uTR)
                    {
                        if (u.Key != largestCardUser)
                        {
                            userturn.Remove(u.Key);

                            Clients.Group(groupname).updateUserStatus(u.Key, "Finished");
                            gameUserTurn[groupname] = userturn;
                            Clients.Group(groupname).groupMessage(u.Key + " has finished his cards.");
                        }
                        else
                        {
                            checklarge = true;
                        }
                        userToRemove.Remove(u.Key);
                    }

                    if (checklarge && userturn.Count == 2)
                    {
                        Random rd = new Random();
                        Clients.Client(startTurn).assignRandom(cards[rd.Next(1, 52)]);
                        Clients.Group(groupname).updateUserTurn(largestCardUser);
                        return;
                    }
                    else
                    {
                        userturn.Remove(largestCardUser);
                        Clients.Group(groupname).updateUserStatus(largestCardUser, "Finished");

                        Clients.Group(groupname).groupMessage(largestCardUser + " has finished his cards.");
                        gameUserTurn[groupname] = userturn;

                        largestcard = (new Utility()).GetLargestCardAgain(groupname, cardThrown[groupname], out largestCardUser, uTR);
                    }
                }

                if (userturn.Count == 1)
                {
                    Clients.Group(groupname).groupMessage(userturn.First().Key + " became bhabo");
                    Clients.Client(userturn.First().Key).becomeBhabo(userturn.First().Key + " became bhabo.");
                    cardThrown[groupname] = "";
                    if (GRUSRs.Where(x => x.Value == groupname).Count() > 2)
                    {
                        StartGame(GRUSRs.Where(x => x.Value == groupname).Count(), groupname);
                    }
                    else
                    {
                        gameUserTurn.Remove(groupname);
                    }
                    return;
                }
                startTurn = user[largestCardUser];
                Clients.Group(groupname).updateUserTurn(largestCardUser);
                Clients.Group(groupname).turnComplete(largestCardUser);

                if (!askTimeOut.Where(x => x.Key == largestCardUser).Any())
                    askTimeOut.Add(largestCardUser, groupname);

                Clients.Client(startTurn).startTurn();

                //if (userDiscQueue.Any(x => x.Value == groupname))
                //{
                //    if (msgQueue.Any(x => x.Key == largestCardUser))
                //    {
                //        msgQueue.Add(largestCardUser, msgQueue[largestCardUser] + ";StartTurn");
                //    }
                //    else
                //    {
                //        msgQueue.Add(largestCardUser, "StartTurn");
                //    }
                //}

                Clients.Group(groupname).StartTimer(largestCardUser, groupname, "");

                Dictionary<string, string> uTurn = new Dictionary<string, string>();
                uTurn.Add(largestCardUser, "Pending");


                List<KeyValuePair<string, string>> Done = userturn.ToList();

                int index = Done.FindIndex(x => x.Key == largestCardUser);

                int length = Done.Count;

                for (int i = index + 1; i < length; i++)
                {

                    var us = Done.ElementAt(i);

                    uTurn.Add(us.Key, "Pending");
                }

                for (int i = 0; i < index; i++)
                {
                    var us = Done.ElementAt(i);

                    uTurn.Add(us.Key, "Pending");
                }

                gameUserTurn[groupname] = uTurn;
                cardThrown[groupname] = "";
            }


        }


        public void ThrowCard(int card, string username, string groupname, string lastCard, string cardTurnType)
        {
            //if (userDiscQueue.Any(x => x.Value == groupname))
            //{
            //    if (msgQueue.Any(x => x.Key == groupname))
            //    {
            //        msgQueue.Add(groupname, msgQueue[groupname] + ";CardSent:" + username + ":" + cards[card]);
            //    }
            //    else
            //    {
            //        msgQueue.Add(groupname, "CardSent:" + username + ":" + cards[card]);
            //    }
            //}

            //if (askTimeOut.Any(x => x.Key == username))
            //{
            //    askTimeOut.Remove(username);
            //    List<KeyValuePair<string, string>> times = askTimeOut.Where(x => x.Value == groupname).ToList();
            //    foreach (var t in times)
            //    {
            //        askTimeOut.Remove(t.Key);
            //    }
            //}
            Clients.Group(groupname).resetTimer();

            string cthrown = "";

            Dictionary<string, string> userturn = gameUserTurn[groupname];

            userturn[username] = "Done";

            gameUserTurn[groupname] = userturn;

            string throwncard = cards[card];
            string cardName = throwncard.Split('?')[1];
            string cardType = cardName.Split('-')[1];


            if (lastCard == "false")
            {
                if (cardTurnType == cardType)
                {
                    if (cardThrown.Any(x => x.Key == groupname))
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
            }
            else
            {
                if (cardTurnType != cardType)
                {
                    // If last card, remove user from turn list.
                    userturn.Remove(username);

                    Clients.Group(groupname).updateUserStatus(username, "Finished");

                    gameUserTurn[groupname] = userturn;
                    Clients.Group(groupname).groupMessage(username + " has finished his cards.");
                }
                else
                {
                    if (cardThrown.Any(x => x.Key == groupname))
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
                    Clients.Group(groupname).groupMessage(username + " has played his last card.");
                    userToRemove.Add(username, groupname);
                }
            }

            if (cardTurnType == cardType)
            {

                Clients.Group(groupname).thrownCard(username, throwncard);

                List<KeyValuePair<string, string>> pending = userturn.Where(x => x.Value == "Pending").ToList();

                if (pending.Any())
                {
                    KeyValuePair<string, string> nextUser = pending.First();

                    string nextConnId = user[nextUser.Key];

                    Clients.Group(groupname).FirstTurn("Next turn : " + nextUser.Key);

                    Clients.Client(nextConnId).yourTurn(cardType);



                    //if (userDiscQueue.Any(x => x.Value == groupname))
                    //{
                    //    if (msgQueue.Any(x => x.Key == nextUser.Key))
                    //    {
                    //        msgQueue.Add(nextUser.Key, msgQueue[nextUser.Key] + ";YourTurn:" + cardType);
                    //    }
                    //    else
                    //    {
                    //        msgQueue.Add(nextUser.Key, "YourTurn:" + cardType);
                    //    }
                    //}

                    Clients.Group(groupname).StartTimer(nextUser.Key, groupname, cardType);

                    Clients.Group(groupname).updateUserTurn(nextUser.Key);
                    if (!askTimeOut.Any(x => x.Key == nextUser.Key))
                        askTimeOut.Add(nextUser.Key, groupname);

                }
                else
                {

                    string largestCardUser = "";

                    int largestcard = (new Utility()).GetLargestCard(groupname, cardThrown[groupname], out largestCardUser);


                    string startTurn = user[largestCardUser];

                    /// remove user if he is not largest or check if only 2 left
                    if (userToRemove.Any(x => x.Value == groupname))
                    {
                        bool checklarge = false;
                        List<KeyValuePair<string, string>> uTR = userToRemove.Where(x => x.Value == groupname).ToList();
                        foreach (var u in uTR)
                        {
                            if (u.Key != largestCardUser)
                            {
                                userturn.Remove(u.Key);

                                Clients.Group(groupname).updateUserStatus(u.Key, "Finished");
                                gameUserTurn[groupname] = userturn;
                                Clients.Group(groupname).groupMessage(u.Key + " has finished his cards.");
                            }
                            else
                            {
                                checklarge = true;
                            }
                            userToRemove.Remove(u.Key);
                        }

                        if (checklarge && userturn.Count == 2)
                        {
                            Random rd = new Random();
                            Clients.Client(startTurn).assignRandom(cards[rd.Next(1, 52)]);
                            Clients.Group(groupname).updateUserTurn(largestCardUser);
                            return;
                        }
                        else if (checklarge && userturn.Count > 2)
                        {
                            userturn.Remove(largestCardUser);
                            Clients.Group(groupname).updateUserStatus(largestCardUser, "Finished");

                            Clients.Group(groupname).groupMessage(largestCardUser + " has finished his cards.");
                            gameUserTurn[groupname] = userturn;
                            largestcard = (new Utility()).GetLargestCardAgain(groupname, cardThrown[groupname], out largestCardUser, uTR);
                        }
                    }

                    if (userturn.Count == 1)
                    { 
                        Clients.Group(groupname).groupMessage(userturn.First().Key + " became bhabo");
                        Clients.Client(userturn.First().Key).becomeBhabo(userturn.First().Key + " became bhabo. !");
                        cardThrown[groupname] = "";
                        if (GRUSRs.Where(x => x.Value == groupname).Count() > 2)
                        {
                            StartGame(GRUSRs.Where(x => x.Value == groupname).Count(), groupname);
                        }
                        else
                        {
                            gameUserTurn.Remove(groupname);
                        }
                        return;
                    }
                    startTurn = user[largestCardUser];
                    Clients.Group(groupname).updateUserTurn(largestCardUser);
                    Clients.Group(groupname).turnComplete(largestCardUser);

                    if (!askTimeOut.Any(x => x.Key == largestCardUser))
                        askTimeOut.Add(largestCardUser, groupname);

                    Clients.Client(startTurn).startTurn();



                    //if (msgQueue.Any(x => x.Key == groupname))
                    //{
                    //    msgQueue.Remove(groupname);
                    //}

                    //if (userDiscQueue.Any(x => x.Value == groupname))
                    //{
                    //    List<string> discUsers = userDiscQueue.Where(x => x.Value == groupname).Select(x => x.Key).ToList();

                    //    foreach (string u in discUsers)
                    //    {  
                    //        if (msgQueue.Any(x => x.Key == u))
                    //        {
                    //            msgQueue.Remove(largestCardUser);
                    //        } 
                    //    } 
                    //}


                    //if (userDiscQueue.Any(x => x.Value == groupname))
                    //{
                    //    if (msgQueue.Any(x => x.Key == largestCardUser))
                    //    {
                    //        msgQueue.Add(largestCardUser, msgQueue[largestCardUser] + ";StartTURN");
                    //    }
                    //    else
                    //    {
                    //        msgQueue.Add(largestCardUser, "StartTURN");
                    //    }
                    //}



                    Clients.Group(groupname).StartTimer(largestCardUser, groupname, "");

                    Dictionary<string, string> uTurn = new Dictionary<string, string>();
                    uTurn.Add(largestCardUser, "Pending");


                    List<KeyValuePair<string, string>> Done = userturn.ToList();

                    int index = Done.FindIndex(x => x.Key == largestCardUser);

                    int length = Done.Count;

                    for (int i = index + 1; i < length; i++)
                    {

                        var us = Done.ElementAt(i);

                        uTurn.Add(us.Key, "Pending");
                    }

                    for (int i = 0; i < index; i++)
                    {
                        var us = Done.ElementAt(i);

                        uTurn.Add(us.Key, "Pending");
                    }

                    string userList = (new Utility()).GetUserList(uTurn);

                    Clients.Group(groupname).updateUserList(userList);


                    gameUserTurn[groupname] = uTurn;
                    cardThrown[groupname] = "";
                }
            }
            else
            {
                // Clients.Group(groupname).thrownCard(username, throwncard);

                // Get largest Card 
                string largestCardUser = "";
                int largestcard = (new Utility()).GetLargestCard(groupname, cardThrown[groupname], out largestCardUser);

                cthrown = cardThrown[groupname];

                cthrown = cthrown + "?" + username + ":" + card;

                string list = (new Utility()).GetThokaList(cthrown, cards);
                string dhokaGivenTo = user[largestCardUser];

                Clients.Group(groupname).groupMessage("Thoka is given by '" + username + "' to '" + largestCardUser + "'");

                Clients.Client(dhokaGivenTo).thokaGiven(list);

                //if (userDiscQueue.Any(x => x.Value == groupname))
                //{
                //    if (msgQueue.Any(x => x.Key == largestCardUser))
                //    {
                //        msgQueue.Add(largestCardUser, msgQueue[largestCardUser] + ";ThokaGiven:" + list);
                //    }
                //    else
                //    {
                //        msgQueue.Add(largestCardUser, "ThokaGiven:" + list);
                //    }
                //}


                /// If thoka is given to largest card user, dont remove
                if (userToRemove.Any(x => x.Value == groupname))
                {
                    List<KeyValuePair<string, string>> uTR = userToRemove.Where(x => x.Value == groupname).ToList();
                    foreach (var u in uTR)
                    {
                        if (u.Key != largestCardUser)
                        {
                            userturn.Remove(u.Key);

                            Clients.Group(groupname).updateUserStatus(u.Key, "Finished");
                            gameUserTurn[groupname] = userturn;
                            Clients.Group(groupname).groupMessage(u.Key + " has finished his cards.");
                        }
                        userToRemove.Remove(u.Key);
                    }
                }

                if (userturn.Count == 1)
                {
                    if (gameInPrgress.Any(x => x.Key == groupname))
                        gameInPrgress.Remove(groupname);
                    int grpCnt = groups[groupname];
                    Clients.Group(groupname).groupMessage(largestCardUser + " became bhabo.");
                    Clients.Client(largestCardUser).becomeBhabo(largestCardUser + " became bhabo.");
                    cardThrown[groupname] = "";
                    if (GRUSRs.Where(x => x.Value == groupname).Count() > 2)
                    {
                        StartGame(GRUSRs.Where(x => x.Value == groupname).Count(), groupname);
                    }
                    else
                    {
                        gameUserTurn.Remove(groupname);
                    }
                    return;
                }

                Clients.Group(groupname).turnComplete(largestCardUser);
                if (!askTimeOut.Where(x => x.Key == largestCardUser).Any())
                    askTimeOut.Add(largestCardUser, groupname);
                Clients.Group(groupname).StartTimer(largestCardUser, groupname, "");

                Clients.Group(groupname).updateUserTurn(largestCardUser);
                string startTurn = user[largestCardUser];

                Clients.Client(startTurn).startTurn();


                //if (msgQueue.Any(x => x.Key == groupname))
                //{
                //    msgQueue.Remove(groupname);
                //}
                //if (msgQueue.Any(x => x.Key == largestCardUser))
                //{
                //    msgQueue.Remove(largestCardUser);
                //}

                //if (userDiscQueue.Any(x => x.Value == groupname))
                //{
                //    if (msgQueue.Any(x => x.Key == largestCardUser))
                //    {
                //        msgQueue.Add(largestCardUser, msgQueue[largestCardUser] + ";StartTURN");
                //    }
                //    else
                //    {
                //        msgQueue.Add(largestCardUser, "StartTURN");
                //    }
                //}


                Dictionary<string, string> uTurn = new Dictionary<string, string>();
                uTurn.Add(largestCardUser, "Pending");

                List<KeyValuePair<string, string>> Done = userturn.ToList();

                int index = Done.FindIndex(x => x.Key == largestCardUser);

                int length = Done.Count;

                for (int i = index + 1; i < length; i++)
                {

                    var us = Done.ElementAt(i);

                    uTurn.Add(us.Key, "Pending");
                }

                for (int i = 0; i < index; i++)
                {
                    var us = Done.ElementAt(i);

                    uTurn.Add(us.Key, "Pending");
                }

                string userList = (new Utility()).GetUserList(uTurn);

                Clients.Group(groupname).updateUserList(userList);

                gameUserTurn[groupname] = uTurn;
                cardThrown[groupname] = "";
            }
        }


        public void UpdateConnId(string old, string newid, string name)
        {
            if (user.Any(x => x.Key == name))
            {
                user.Remove(name);
            }
            user.Add(name, newid);
            if (GRUSRs.Where(x => x.Key == name).Any())
            {
                string groupName = GRUSRs[name];

                Groups.Add(newid, groupName);
                Groups.Remove(old, groupName);
            }
            else
            {
                if (userDiscQueue.Any(x => x.Key == name))
                {

                    string grpname = userDiscQueue[name];
                    /// Add to groups for message
                    Groups.Add(Context.ConnectionId, grpname);

                    // Add to dicotionary
                    GRUSRs.Add(name, grpname);

                    List<KeyValuePair<string, string>> allUsrs = GRUSRs.Where(x => x.Value == grpname).ToList();
                    Dictionary<string, string> userTurn = new Dictionary<string, string>();

                    foreach (KeyValuePair<string, string> u in allUsrs)
                    {
                        userTurn.Add(u.Key, "Pending");
                    }
                    string userList = (new Utility()).GetUserList(userTurn);

                    Clients.Client(Context.ConnectionId).updateUserList(userList);




                    //if (msgQueue.Any(x => x.Key == grpname))
                    //{
                    //    //  msgQueue.Add(grpname, msgQueue[grpname] + ";CardSent:" + cards[card]);
                    //    string msgqueue = msgQueue[grpname];
                    //    string[] queue = msgQueue.ToString().Split(';');
                    //    foreach (string msg in queue)
                    //    {
                    //        string[] cards = msg.Split(':');
                    //        if (cards[0] == "CardSent")
                    //        {
                    //            Clients.Client(Context.ConnectionId).resetTimer();
                    //            Clients.Client(Context.ConnectionId).thrownCard(cards[1], cards[2]);
                    //        }
                    //    }

                    //}



                    userDiscQueue.Remove(name);



                }
                else
                {
                    JoinGame(name);
                }
            }
        }

        public void Register(string name)
        {
            string connectionId = Context.ConnectionId;
            if (user.Any(x => x.Key == name))
            {
                user.Remove(name);
                if (GRUSRs.Where(x => x.Key == name).Any())
                {
                    string groupName = GRUSRs[name];
                    GRUSRs.Remove(name);
                    int grpCnt = groups[groupName];
                    groups[groupName] = grpCnt - 1;
                }
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

                var grpselect = GRUSRs.GroupBy(x => x.Value)
              .Where(g => g.Count() < 6)
              .Select(y => y)
              .FirstOrDefault();

                //GRUSRs.Select
                //KeyValuePair<string, int> grp = groups.Where(x => x.Value < 6).FirstOrDefault();

                if (grpselect != null)
                {
                    groupName = grpselect.Key;
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

                //groups[groupName] = grpCnt + 1;
                grpCnt = groups[groupName];

                Clients.All.informGroupName(groupName);

                List<KeyValuePair<string, string>> allUsrs = GRUSRs.Where(x => x.Value == groupName).ToList();
                Dictionary<string, string> userTurn = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> u in allUsrs)
                {
                    userTurn.Add(u.Key, "Pending");
                }
                string userList = (new Utility()).GetUserList(userTurn);

                Clients.Group(groupName).updateUserList(userList);
                Clients.Client(Context.ConnectionId).updateUserList(userList);

                Clients.Group(groupName).groupMessage(name + " Joined !");
                Clients.Group(groupName).groupMessage("Game will start with minimum 3 users !");

                Clients.Client(Context.ConnectionId).groupMessage(name + " Joined !");
                Clients.Client(Context.ConnectionId).groupMessage("Game will start with minimum 3 users !");

                if (GRUSRs.Where(x => x.Value == groupName).Count() > 1)// && !gameUserTurn.Where(x => x.Key == groupName).Any())
                {
                    Clients.Group(groupName).groupMessage("Please wait. Card Distribution started !");


                    StartGame(GRUSRs.Where(x => x.Value == groupName).Count(), groupName);
                    gameInPrgress.Add(groupName, "InProgress");
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
                if (gameUserTurn.Where(x => x.Key == groupName).Any())
                {
                    gameUserTurn.Remove(groupName);
                }


                Dictionary<int, string> randomCards = DistCards();
                //int count = 52 / grpCnt;
                int count = 4;
                List<KeyValuePair<string, string>> allUsrs = GRUSRs.Where(x => x.Value == groupName).ToList();


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


                    userTurn.Add(u.Key, "Pending");
                }
                string userList = (new Utility()).GetUserList(userTurn);

                Clients.Group(groupName).updateUserList(userList);

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