using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PushNotification;
using System.Net;
using System.Text;
using System.IO;
namespace WebApplication1
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Key : Unique iD
        /// Value : Name
        /// </summary>
        public static Dictionary<string, string> uniqueIds = new Dictionary<string, string>();

        /// <summary>
        /// Key : Name
        /// Value : Connection ID
        /// </summary>
        public static Dictionary<string, string> user = new Dictionary<string, string>();

        /// <summary>
        /// Key :Group Name
        /// Value : Group ID
        /// </summary>
        public static Dictionary<string, string> groups = new Dictionary<string, string>();

        /// <summary>
        /// Key : Username
        /// Value : Group ID
        /// </summary>
        public static Dictionary<string, string> groupusers = new Dictionary<string, string>();

        /// <summary>
        /// Key : Name
        /// Value : GCM ID
        /// </summary>
        public static Dictionary<string, string> userGCMIDs = new Dictionary<string, string>();

        public void SendOfflineMessage(string sender, string receiver, string message)
        {
            PushNotification.PushNotification push = new PushNotification.PushNotification();
            if (userGCMIDs.Any(x => x.Key == receiver))
            {
                push.Android(userGCMIDs[receiver], "bathindavarinder@gmail.com", "2237679@Va", sender + " : " + message);
            }
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

        public void Send(string name, string to, string message)
        {
            if (to != "" && user.Any(a => a.Key == to))
            {
                Clients.Client(user[to]).sendMessage(name, message);
                Clients.Client(Context.ConnectionId).sendMessage(name, message);
            }
            else
            {
                Clients.All.sendMessage(name, message);
            }
        }
        public void UpdateUserGCMID(string name, string GCMID)
        {
            if (!userGCMIDs.Any(x => x.Key == name))
            {
                userGCMIDs.Add(name, GCMID);
            }
            else
            {
                userGCMIDs.Remove(name);
                userGCMIDs.Add(name, GCMID);
            }
        }

        public void UpdateName(string connId, string name)
        {
            if (!user.Any(x => x.Key == name))
                user.Add(name, connId);

            string users = "";
            foreach (KeyValuePair<string, string> pair in user)
            {
                users = users + pair.Key + ",";
            }
            users = users.Substring(0, users.Length - 1);
            Clients.All.updateList(users);
        }

        public void RemoveUser(string name)
        {
            user.Remove(name);

            string users = "";
            foreach (KeyValuePair<string, string> pair in user)
            {
                users = users + pair.Key + ",";
            }
            users = users.Substring(0, users.Length - 1);
            Clients.All.updateList(users);
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            dynamic t;
            try
            {
                string connId = Context.ConnectionId;

                if (user.Any(x => x.Value == connId))
                {
                    KeyValuePair<string, string> u = user.Where(x => x.Value == connId).FirstOrDefault();
                    string userName = u.Key;

                    if (groupusers.Any(x => x.Key == userName))
                    {
                        KeyValuePair<string, string> gu = groupusers.Where(x => x.Key == userName).FirstOrDefault();

                        string groupId = gu.Value;

                        KeyValuePair<string, string> g = groups.Where(x => x.Value == groupId).FirstOrDefault();

                        string groupName = g.Key;


                        Groups.Remove(connId, groupName);
                        Clients.Group(groupName).addChatMessage(userName + " left.");
                        Clients.Group(groupName).leftRoom(userName);
                    }

                    groupusers.Remove(u.Key);
                    user.Remove(u.Key);
                    user.Add(userName, "");
                }
            }

            finally
            {
                t = base.OnDisconnected(stopCalled);
            }
            return t;
        }

        public void UpdateConnId(string old, string newid, string name)
        {
            if (user.Any(x => x.Key == name))
            {
                user.Remove(name);
                user.Add(name, newid);
                string grpId = groupusers[name];
                string groupName = groups.Where(x => x.Value == grpId).FirstOrDefault().Key.ToString();
                Groups.Add(newid, groupName);
                Groups.Remove(old, groupName);
                //Clients.Group(groupName).confirmJoin(name);
                //Clients.Client(Context.ConnectionId).updateMembers(users);
            }
        }

        public void JoinRoom(string groupName, string userName)
        {
            if (user.Any(x => x.Key == userName))
            {
                user.Remove(userName);
            }

            user.Add(userName, Context.ConnectionId);
            if (groups.Any(x => x.Key == groupName))
            {
                string guid = groups.Where(x => x.Key == groupName).FirstOrDefault().Value.ToString();
                if (!groupusers.Any(x => x.Key == userName))
                {
                    groupusers.Add(userName, guid.ToString());
                }
            }
            else
            {
                Guid guid = Guid.NewGuid();
                groups.Add(groupName, guid.ToString());

                if (!groupusers.Any(x => x.Key == userName))
                {
                    groupusers.Add(userName, guid.ToString());
                }

            }
            KeyValuePair<string, string> grp = groups.Where(x => x.Key == groupName).FirstOrDefault();

            IEnumerable<KeyValuePair<string, string>> members = groupusers.Where(x => x.Value == grp.Value.ToString());
            string users = "";
            foreach (KeyValuePair<string, string> p in members)
            {
                users += users + p.Key + ",";
            }

            Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).confirmJoin(userName);
            Clients.Client(Context.ConnectionId).updateMembers(users);
        }


        public void LeaveRoom(string groupName, string userName, string connectionId)
        {
            Groups.Remove(connectionId, groupName);
            Clients.Client(Context.ConnectionId).confirmLeft();
            Clients.Group(groupName).addChatMessage(userName + " left.");
            Clients.Group(groupName).leftRoom(userName);
            groupusers.Remove(userName);
        }

        public void SendGroupMessage(string grpName, string name, string message)
        {
            Clients.Group(grpName).addChatMessage(name + " : " + message);
        }
        public void SendPersonalMessage(string name, string message, string sender)
        {
            string senderConnId = user[sender];
            string reciever = user[name];
            Clients.Client(senderConnId).byPersonalChat("avail : " + reciever + " End : " + message, name);




            if (string.IsNullOrEmpty(reciever))
            {
                if (userGCMIDs.Any(x => x.Key == name))
                {
                    Clients.Client(senderConnId).byPersonalChat(" push notify : " + userGCMIDs[name], name);
                    try
                    {
                        SendNotification(userGCMIDs[name], sender + " : " + message);
                        //PushNotification.PushNotification push = new PushNotification.PushNotification();
                        //string result = push.Android(userGCMIDs[name], "bathindavarinder@gmail.com", "2237679@Va", sender + " : " + message);
                    }
                    catch (Exception ex)
                    {
                        Clients.Client(senderConnId).byPersonalChat(" push res : " + ex.Message, name);
                    }
                }
                else
                {
                    Clients.Client(senderConnId).byPersonalChat(" push not avail ", name);
                }
            }
            else
            {
                Clients.Client(reciever).recievePersonalChat(message, sender);
            }


        }

        public string SendNotification(string deviceId, string message)
        {
            string GoogleAppID = "AIzaSyAH_bMQkNYTT16IR3ycM4aulDRZ3Z-OOR8";
            var SENDER_ID = "899559090645";
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" +
            System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }


    }
}