using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]
namespace SignalRChat
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // app.MapSignalR();
            //RouteTable.Routes.MapHubs();
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(20);

            // Wait a maximum of 30 seconds after a transport connection is lost
            // before raising the Disconnected event to terminate the SignalR connection.
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(10);

            // For transports other than long polling, send a keepalive packet every
            // 10 seconds. 
            // This value must be no more than 1/3 of the DisconnectTimeout value.
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(3);

            app.Map("/signalr", map =>
            {
                //  map.Use(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true,
                };
                
                map.RunSignalR(hubConfiguration);
            });
            
        }
    }
}
