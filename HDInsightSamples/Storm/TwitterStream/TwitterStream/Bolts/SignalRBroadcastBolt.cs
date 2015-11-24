using System;
using System.Collections.Generic;
using Microsoft.SCP;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using System.Configuration;

namespace TwitterStream.Bolts
{
    public class SignalRBroadcastBolt : ISCPBolt
    {
        Context context;

        //SingnalR Connection
        HubConnection hubConnection;
        IHubProxy twitterHubProxy;
        Stopwatch timer = Stopwatch.StartNew();

        //Constructor
        public SignalRBroadcastBolt(Context context)
        {
            Context.Logger.Info("SignalRBroadcastBolt constructor called");
            //Set context
            this.context = context;

            //Define the schema for the incoming tuples from spout
            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            //Input schema counter updates
            inputSchema.Add(Constants.DEFAULT_STREAM_ID, new List<Type>() { typeof(long), typeof(string) });

            //Declare both incoming and outbound schemas
            this.context.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, null));

            // Initialize SignalR connection
            StartSignalRHubConnection();
        }

        //Get a new instance
        public static SignalRBroadcastBolt Get(Context ctx, Dictionary<string, Object> parms)
        {
            return new SignalRBroadcastBolt(ctx);
        }

        //Process a tuple from the stream
        public void Execute(SCPTuple tuple)
        {
            Context.Logger.Info("Execute enter");
            var tweetCount = (long)tuple.GetValue(0);
            var tweet = tuple.GetValue(1) as string;

            try
            {
                //Only send updates every 500 milliseconds
                //Ignore the messages in between so that you don't overload the SignalR website with updates at each tuple
                //If you have only aggreagates to send that can be spaced, you don't need this timer
                if (timer.ElapsedMilliseconds >= 100)
                {
                    SendSingnalRUpdate(tweetCount, tweet);
                    timer.Restart();
                }
            }
            catch (Exception ex)
            {
                Context.Logger.Error("SignalRBroadcastBolt Exception: " + ex.Message + "\nStackTrace: \n" + ex.StackTrace);
            }

            Context.Logger.Info("Execute exit");
        }

        private void StartSignalRHubConnection()
        {
            //TODO: Specify your SignalR website settings in SCPHost.exe.config
            this.hubConnection = new HubConnection(ConfigurationManager.AppSettings["SignalRWebsiteUrl"]);
            this.twitterHubProxy = hubConnection.CreateHubProxy(ConfigurationManager.AppSettings["SignalRHub"]);
            hubConnection.Start().Wait();
        }

        private void SendSingnalRUpdate(long rowCount, string tweet)
        {
            if (hubConnection.State != ConnectionState.Connected)
            {
                hubConnection.Stop();
                StartSignalRHubConnection();
            }
            twitterHubProxy.Invoke("UpdateCounter", rowCount, tweet);
        }
    }
}
