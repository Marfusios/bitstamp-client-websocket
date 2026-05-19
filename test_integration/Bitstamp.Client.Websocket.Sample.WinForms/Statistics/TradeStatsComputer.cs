using System;
using System.Collections.Generic;
using System.Linq;
using Bitstamp.Client.Websocket.Responses;
using Bitstamp.Client.Websocket.Responses.Trades;

namespace Bitstamp.Client.Websocket.Sample.WinForms.Statistics
{
    internal class TradeStatsComputer
    {
        private readonly List<Trade> _lastTrades = new List<Trade>();

        public void HandleTrade(Trade newTrade)
        {
            _lastTrades.Add(newTrade);
        }

        public TradeStats GetStatsFor(int minutes)
        {
            var timeLimit = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(minutes));
            var trades = _lastTrades.Where(x => x.Timestamp >= timeLimit || x.Microtimestamp >= timeLimit).ToArray();

            var buys = trades.Where(x => x.Side == TradeSide.Buy).Sum(x => x.Amount);
            var sells = trades.Where(x => x.Side == TradeSide.Sell).Sum(x => x.Amount);

            if (buys <= 0 && sells <= 0)
                return TradeStats.NULL;

            var total = buys + sells;
            return new TradeStats(buys / total * 100, sells / total * 100, trades.Length);
        }
    }

    internal class TradeStats
    {
        public static readonly TradeStats NULL = new TradeStats(0, 0, 0);

        public TradeStats(double buysPerc, double sellsPerc, int totalCount)
        {
            BuysPerc = buysPerc;
            SellsPerc = sellsPerc;
            TotalCount = totalCount;
        }

        public double BuysPerc { get; }

        public double SellsPerc { get; }

        public int TotalCount { get; }
    }
}
