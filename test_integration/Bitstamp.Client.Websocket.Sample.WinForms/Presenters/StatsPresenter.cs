using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses.Books;
using Bitstamp.Client.Websocket.Responses.Trades;
using Bitstamp.Client.Websocket.Sample.WinForms.Statistics;
using Bitstamp.Client.Websocket.Sample.WinForms.Views;
using Serilog;
using Websocket.Client;

namespace Bitstamp.Client.Websocket.Sample.WinForms.Presenters
{
    internal class StatsPresenter
    {
        private readonly IStatsView _view;

        private TradeStatsComputer _tradeStatsComputer;
        private OrderBookStatsComputer _orderBookStatsComputer;

        private IBitstampCommunicator _communicator;
        private BitstampWebsocketClient _client;

        private readonly string _defaultPair = "btcusd";
        private readonly string _currency = "$";

        public StatsPresenter(IStatsView view)
        {
            _view = view;

            HandleCommands();
        }

        private void HandleCommands()
        {
            _view.OnInit = OnInit;
            _view.OnStart = async () => await OnStart();
            _view.OnStop = OnStop;
        }

        private void OnInit()
        {
            Clear();
        }

        private async Task OnStart()
        {
            var pair = _view.Pair;
            if (string.IsNullOrWhiteSpace(pair))
                pair = _defaultPair;
            pair = pair.ToLowerInvariant();

            _tradeStatsComputer = new TradeStatsComputer();
            _orderBookStatsComputer = new OrderBookStatsComputer();

            _communicator = new BitstampWebsocketCommunicator(BitstampValues.ApiWebsocketUrl);
            _client = new BitstampWebsocketClient(_communicator);

            Subscribe(_client);

            _communicator.ReconnectionHappened.Subscribe(info =>
            {
                _view.Status($"Reconnected (type: {info.Type})", StatusType.Info);
                SendSubscriptions(_client, pair);
            });

            _communicator.DisconnectionHappened.Subscribe(info =>
            {
                if (info.Type == DisconnectionType.Error)
                {
                    _view.Status($"Disconnected by error, next try in {_communicator.ErrorReconnectTimeout?.TotalSeconds} sec",
                        StatusType.Error);
                    return;
                }

                _view.Status($"Disconnected (type: {info.Type})", StatusType.Warning);
            });

            await _communicator.Start();
        }

        private void OnStop()
        {
            _client?.Dispose();
            _communicator?.Dispose();
            _client = null;
            _communicator = null;
            Clear();
        }

        private void Subscribe(BitstampWebsocketClient client)
        {
            client.Streams.TickerStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleTrades);
            client.Streams.OrderBookStream.ObserveOn(TaskPoolScheduler.Default).Subscribe(HandleOrderBook);
        }

        private void SendSubscriptions(BitstampWebsocketClient client, string pair)
        {
            client.Send(new SubscribeRequest(pair, Channel.Ticker));
            client.Send(new SubscribeRequest(pair, Channel.OrderBook));
        }

        private void HandleTrades(TradeResponse response)
        {
            var trade = response.Data;
            Log.Information($"Received [{trade.Side}] trade, price: {trade.Price}, amount: {trade.Amount}");
            _tradeStatsComputer.HandleTrade(trade);

            FormatTradesStats(_view.Trades1Min, _tradeStatsComputer.GetStatsFor(1));
            FormatTradesStats(_view.Trades5Min, _tradeStatsComputer.GetStatsFor(5));
            FormatTradesStats(_view.Trades15Min, _tradeStatsComputer.GetStatsFor(15));
            FormatTradesStats(_view.Trades1Hour, _tradeStatsComputer.GetStatsFor(60));
            FormatTradesStats(_view.Trades24Hours, _tradeStatsComputer.GetStatsFor(60 * 24));

            _view.Status("Connected", StatusType.Info);
        }

        private void FormatTradesStats(Action<string, Side> setAction, TradeStats trades)
        {
            if (trades == TradeStats.NULL)
                return;

            if (trades.BuysPerc >= trades.SellsPerc)
            {
                setAction($"{trades.BuysPerc:###}% buys{Environment.NewLine}{trades.TotalCount}", Side.Buy);
                return;
            }

            setAction($"{trades.SellsPerc:###}% sells{Environment.NewLine}{trades.TotalCount}", Side.Sell);
        }

        private void HandleOrderBook(OrderBookResponse response)
        {
            _orderBookStatsComputer.HandleOrderBook(response.Data);

            var stats = _orderBookStatsComputer.GetStats();
            if (stats == OrderBookStats.NULL)
                return;

            _view.Bid = stats.Bid.ToString("#.00");
            _view.Ask = stats.Ask.ToString("#.00");

            _view.BidAmount = $"{stats.BidAmountPerc:###}%{Environment.NewLine}{FormatToMillions(stats.BidAmount)}";
            _view.AskAmount = $"{stats.AskAmountPerc:###}%{Environment.NewLine}{FormatToMillions(stats.AskAmount)}";
        }

        private string FormatToMillions(double amount)
        {
            var millions = amount / 1000000;
            return $"{_currency}{millions:#.00} M";
        }

        private void Clear()
        {
            _view.Bid = string.Empty;
            _view.Ask = string.Empty;
            _view.BidAmount = string.Empty;
            _view.AskAmount = string.Empty;
            _view.Ping = string.Empty;
            _view.Trades1Min(string.Empty, Side.Buy);
            _view.Trades5Min(string.Empty, Side.Buy);
            _view.Trades15Min(string.Empty, Side.Buy);
            _view.Trades1Hour(string.Empty, Side.Buy);
            _view.Trades24Hours(string.Empty, Side.Buy);
        }
    }
}
