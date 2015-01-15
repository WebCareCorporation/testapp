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

        public static Dictionary<int, string> cards = new Dictionary<int, string>();


        public GameHub()
        {
            cards.Add(1, "card-hukam-a");
            cards.Add(2, "card-hukam-2");
            cards.Add(3, "card-hukam-3");
            cards.Add(4, "card-hukam-4");
            cards.Add(5, "card-hukam-5");
            cards.Add(6, "card-hukam-6");
            cards.Add(7, "card-hukam-7");
            cards.Add(8, "card-hukam-8");
            cards.Add(9, "card-hukam-9");
            cards.Add(10, "card-hukam-10");
            cards.Add(11, "card-hukam-j");
            cards.Add(12, "card-hukam-q");
            cards.Add(13, "card-hukam-k");


            cards.Add(14, "card-heart-a");
            cards.Add(15, "card-heart-2");
            cards.Add(16, "card-heart-3");
            cards.Add(17, "card-heart-4");
            cards.Add(18, "card-heart-5");
            cards.Add(19, "card-heart-6");
            cards.Add(20, "card-heart-7");
            cards.Add(21, "card-heart-8");
            cards.Add(22, "card-heart-9");
            cards.Add(23, "card-heart-10");
            cards.Add(24, "card-heart-j");
            cards.Add(25, "card-heart-q");
            cards.Add(26, "card-heart-k");

            cards.Add(27, "card-chidi-a");
            cards.Add(28, "card-chidi-2");
            cards.Add(29, "card-chidi-3");
            cards.Add(30, "card-chidi-4");
            cards.Add(31, "card-chidi-5");
            cards.Add(32, "card-chidi-6");
            cards.Add(33, "card-chidi-7");
            cards.Add(34, "card-chidi-8");
            cards.Add(35, "card-chidi-9");
            cards.Add(36, "card-chidi-10");
            cards.Add(37, "card-chidi-j");
            cards.Add(38, "card-chidi-q");
            cards.Add(39, "card-chidi-k");

            cards.Add(41, "card-itt-a");
            cards.Add(42, "card-itt-2");
            cards.Add(43, "card-itt-3");
            cards.Add(44, "card-itt-4");
            cards.Add(45, "card-itt-5");
            cards.Add(46, "card-itt-6");
            cards.Add(47, "card-itt-7");
            cards.Add(48, "card-itt-8");
            cards.Add(49, "card-itt-9");
            cards.Add(49, "card-itt-10");
            cards.Add(50, "card-itt-j");
            cards.Add(51, "card-itt-q");
            cards.Add(52, "card-itt-k");


        }

        public void SendOfflineMessage(string sender, string receiver, string message)
        {

            Dictionary<int, string> dividelist = new Dictionary<int, string>();

            ArrayList selected = new ArrayList();

            int c = 52;
            Random random = new Random();
            int s = random.Next(52);
            while (c > 0)
            {
                while (selected.Contains(s))
                {
                    s = random.Next(52);
                }  
                selected.Add(s);
                dividelist.Add(s, cards[s]);
                c = c - 1; 
            }



        }
    }
}