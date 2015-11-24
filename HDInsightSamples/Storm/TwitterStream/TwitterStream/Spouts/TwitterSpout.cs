using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.SCP;
using Microsoft.SCP.Utils;
using Tweetinvi;
using System.Configuration;
using Tweetinvi.Core.Interfaces;

namespace TwitterStream.Spouts
{
    public class TwitterSpout : ISCPSpout
    {
        Context context;
        Queue<SerializableTweet> queue = new Queue<SerializableTweet>();

        public TwitterSpout(Context context)
        {
            this.context = context;

            Dictionary<string, List<Type>> outputSchema = new Dictionary<string, List<Type>>();
            outputSchema.Add(Constants.DEFAULT_STREAM_ID, 
                new List<Type>() { typeof(SerializableTweet) });
            this.context.DeclareComponentSchema(new ComponentStreamSchema(null, outputSchema));
            
            
            //TODO: Specify your twitter credentials in app.config
            TwitterCredentials.SetCredentials(
                ConfigurationManager.AppSettings["TwitterAccessToken"],
                ConfigurationManager.AppSettings["TwitterAccessTokenSecret"],
                ConfigurationManager.AppSettings["TwitterConsumerKey"],
                ConfigurationManager.AppSettings["TwitterConsumerSecret"]);

            //TODO: Setup a Twitter Stream
            CreateFilteredStream();
        }

        public void CreateFilteredStream()
        {
            var stream = Tweetinvi.Stream.CreateFilteredStream();
            stream.MatchingTweetReceived += (sender, args) => { NextTweet(args.Tweet); };
            //TODO: Setup your filter criteria
            stream.AddTrack("China");
            stream.StartStreamMatchingAnyConditionAsync();
        }

        public void CreateSampleStream()
        {
            var stream = Tweetinvi.Stream.CreateSampleStream();
            stream.TweetReceived += (sender, args) => { NextTweet(args.Tweet); };
            stream.StartStreamAsync();
        }


        public static TwitterSpout Get(Context context, Dictionary<string, Object> parms)
        {
            return new TwitterSpout(context);
        }

        /// <summary>
        /// The twitter async stream methods call this method to queue the tweets
        /// </summary>
        /// <param name="tweet"></param>
        public void NextTweet(ITweet tweet)
        {
            queue.Enqueue(new SerializableTweet(tweet));
        }

        public void NextTuple(Dictionary<string, Object> parms)
        {
            if (queue.Count > 0)
            {
                var tweet = queue.Dequeue();
                context.Emit(new Values(tweet));
                Context.Logger.Info("NextTuple: Emitted Tweet = {0}", tweet.Text);
            }
            else
            {
                //Free up some CPU cycles if no tweets are being received
                Thread.Sleep(50);
            }
        }

        public void Ack(long seqId, Dictionary<string, Object> parms)
        {
            //do nothing - optionally you can cache the tuple and remove them in this method
            //cache.Remove(seqId);
        }

        public void Fail(long seqId, Dictionary<string, Object> parms)
        {
            //do nothing - optionally you can cache the tuples and re-emit them in this method
            //this.context.Emit(cache[seqId]);
        }
    }
}

