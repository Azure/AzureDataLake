using System;
using System.Collections.Generic;
using Microsoft.SCP;

namespace TwitterStream.Bolts
{
    /// <summary>
    /// This twitter sample bolt inherits from SqlAzureSampleBolt
    /// </summary>
    public class TwitterBolt : ISCPBolt
    {
        Context context;
        long count = 0;

        public TwitterBolt(Context context, Dictionary<string, Object> parms)
        {
            this.context = context;

            Dictionary<string, List<Type>> inputSchema = new Dictionary<string, List<Type>>();
            inputSchema.Add(Constants.DEFAULT_STREAM_ID, new List<Type>() { typeof(SerializableTweet) });

            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add(Constants.DEFAULT_STREAM_ID, new List<Type>() { typeof(long), typeof(string) });

            this.context.DeclareComponentSchema(new ComponentStreamSchema(inputSchema, outputSchema));
        }

        public static TwitterBolt Get(Context ctx, Dictionary<string, Object> parms)
        {
            return new TwitterBolt(ctx, parms);
        }

        public void Execute(SCPTuple tuple)
        {
            var tweet = tuple.GetValue(0) as SerializableTweet;
            ExecuteTweet(tweet);
        }

        /// <summary>
        /// This is where you business logic around a tweet will go
        /// Here we are emitting the count and the tweet text to next bolt
        /// And also inserting the tweet into SQL Azure
        /// </summary>
        /// <param name="tweet"></param>
        public void ExecuteTweet(SerializableTweet tweet)
        {
            count++;
            Context.Logger.Info("ExecuteTweet: Count = {0}, Tweet = {1}", count, tweet.Text);

            //TODO: You can do something on other tweet fields
            //Like aggreagtions on tweet.Language etc

            //Emit the value to next bolt - SignalR & SQL Azure
            //Ensure that subsequent bolts align with the data fields and types you send
            this.context.Emit(new Values(count, tweet.Text));
        }
    }
}

