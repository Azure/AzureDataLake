using System;
using Tweetinvi.Core.Interfaces;

namespace TwitterStream
{
    //Workaround to make the Tweet from TweetInvi Serializable
    //Mark this class Serializable
    [Serializable]
    public class SerializableTweet
    {
        public string Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRetweet { get; set; }
        public string Language { get; set; }
        public bool Retweeted { get; set; }
        public string Text { get; set; }

        public SerializableTweet(ITweet tweet)
        {
            this.Creator = tweet.Creator.Name;
            this.CreatedAt = tweet.CreatedAt;
            this.IsRetweet = tweet.IsRetweet;
            this.Language = tweet.Language.ToString();
            this.Retweeted = tweet.Retweeted;
            this.Text = tweet.Text;
        }
    }
}
