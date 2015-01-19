using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public class Utility
    {

        public int GetLargestCard(string groupname, string ct, out string largestCardUser)
        {
            // Get largest Card
            int largestcard = 0;
            largestCardUser = "";
            // string ct = cardThrown[groupname];
            string[] cts = ct.Split('?');
            foreach (string c in cts)
            {
                if (c != "")
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
            }
            return largestcard;
        }

        public int GetLargestCardAgain(string groupname, string ct, out string largestCardUser, List<KeyValuePair<string, string>> toremove)
        {
            // Get largest Card
            int largestcard = 0;
            largestCardUser = "";
            // string ct = cardThrown[groupname];
            string[] cts = ct.Split('?');
            foreach (string c in cts)
            {
                if (c != "")
                {
                    int cardNumber = Convert.ToInt32(c.Split(':')[1]);

                    if (!toremove.Where(x=>x.Key ==c.Split(':')[0]).Any())
                    {
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
                }
            }
            return largestcard;
        }
        public string GetThokaList(string ct,Dictionary<int,string> cards)
        {
            // Get largest Card
            string cardList = "";
            // string ct = cardThrown[groupname];
            string[] cts = ct.Split('?');
            foreach (string c in cts)
            {
                if (c != "")
                {
                    int cardNumber = Convert.ToInt32(c.Split(':')[1]);
                    cardList += cardNumber+"?"+ cards[cardNumber] + ";";
                }
            }
            return cardList;

        }
    }
}