using System.Linq;
using Bitstamp.Client.Websocket.Responses.Books;

namespace Bitstamp.Client.Websocket.Sample.WinForms.Statistics
{
    internal class OrderBookStatsComputer
    {
        private BookLevel[] _bids = new BookLevel[0];
        private BookLevel[] _asks = new BookLevel[0];

        public void HandleOrderBook(OrderBook orderBook)
        {
            _bids = orderBook.Bids ?? new BookLevel[0];
            _asks = orderBook.Asks ?? new BookLevel[0];
        }

        public OrderBookStats GetStats()
        {
            var bids = _bids.OrderByDescending(x => x.Price).ToArray();
            var asks = _asks.OrderBy(x => x.Price).ToArray();

            if (!bids.Any() || !asks.Any())
                return OrderBookStats.NULL;

            var bidAmounts = bids.Take(20).Sum(x => x.Amount * x.Price);
            var askAmounts = asks.Take(20).Sum(x => x.Amount * x.Price);
            var total = bidAmounts + askAmounts;

            return new OrderBookStats(
                bids[0].Price,
                asks[0].Price,
                bidAmounts / total * 100,
                askAmounts / total * 100,
                bidAmounts,
                askAmounts);
        }
    }

    internal class OrderBookStats
    {
        public static readonly OrderBookStats NULL = new OrderBookStats(0, 0, 0, 0, 0, 0);

        public OrderBookStats(double bid, double ask, double bidAmountPerc, double askAmountPerc,
            double bidAmount, double askAmount)
        {
            Bid = bid;
            Ask = ask;
            BidAmountPerc = bidAmountPerc;
            AskAmountPerc = askAmountPerc;
            BidAmount = bidAmount;
            AskAmount = askAmount;
        }

        public double Bid { get; }

        public double Ask { get; }

        public double BidAmountPerc { get; }

        public double AskAmountPerc { get; }

        public double BidAmount { get; }

        public double AskAmount { get; }
    }
}
