using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

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
            cards.Add(11, "card-hukam-7");

        }

        public void SendOfflineMessage(string sender, string receiver, string message)
        { 
           
        }
    }
}