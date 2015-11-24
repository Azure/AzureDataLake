using System;
using System.Collections.Generic;
using Microsoft.SCP;
using Microsoft.SCP.Topology;
using TwitterStream.Spouts;
using TwitterStream.Bolts;

namespace TwitterStream
{
    [Active(true)]
    class TwitterTopology : TopologyDescriptor
    {
        public ITopologyBuilder GetTopologyBuilder()
        {
            TopologyBuilder topologyBuilder = new TopologyBuilder(typeof(TwitterTopology).Name + DateTime.Now.ToString("-yyyyMMddHHmmss"));

            topologyBuilder.SetSpout(
                typeof(TwitterSpout).Name,
                TwitterSpout.Get,
                new Dictionary<string, List<string>>()
                {
                    {Constants.DEFAULT_STREAM_ID, new List<string>(){"tweet"}}
                },
                1);
            topologyBuilder.SetBolt(
                typeof(TwitterBolt).Name,
                TwitterBolt.Get,
                new Dictionary<string, List<string>>() 
                {
                    {Constants.DEFAULT_STREAM_ID, new List<string>(){"count", "tweet"}}
                },
                1).shuffleGrouping(typeof(TwitterSpout).Name);

            topologyBuilder.SetBolt(
                typeof(SqlAzureBolt).Name,
                SqlAzureBolt.Get,
                new Dictionary<string, List<string>>(),
                1).shuffleGrouping(typeof(TwitterBolt).Name);

            topologyBuilder.SetBolt(
                typeof(SignalRBroadcastBolt).Name,
                SignalRBroadcastBolt.Get,
                new Dictionary<string, List<string>>(),
                1).shuffleGrouping(typeof(TwitterBolt).Name);
            return topologyBuilder;
        }
    }
}

