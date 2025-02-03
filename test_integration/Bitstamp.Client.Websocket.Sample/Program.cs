using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Bitstamp.Client.Websocket.Sample
{
    internal class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private static readonly string ApiKey = "your api key";
        private static readonly string ApiSecret = "";

        private static async Task Main(string[] args)
        {
            var logger = InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|    BITSTAMP CLIENT    |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("              STARTING              ");
            Log.Debug("====================================");


            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url, logger.CreateLogger<BitstampWebsocketCommunicator>()))
            {
                communicator.Name = "Bitstamp-1";

                using (var client = new BitstampWebsocketClient(communicator, logger.CreateLogger<BitstampWebsocketClient>()))
                {
                    SubscribeToStreams(client);

                    communicator.ReconnectionHappened.Subscribe(async type =>
                    {
                        Log.Information("Reconnection happened, type: {type}, resubscribing..", type.Type);
                        await SendSubscriptionRequests(client);
                    });

                    await communicator.Start();

                    ExitEvent.WaitOne();
                }
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static async Task SendSubscriptionRequests(BitstampWebsocketClient client)
        {
            //client.Send(new SubscribeRequest("btcusd", Channel.OrderBook));
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBook));

            //client.Send(new SubscribeRequest("btcusd", Channel.OrderBookDetail));
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBookDetail));

            //client.Send(new SubscribeRequest("btcusd", Channel.OrderBookDiff));
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBookDiff));

            client.Send(new SubscribeRequest("btcusd", Channel.Ticker));
            client.Send(new SubscribeRequest("btceur", Channel.Ticker));
        }

        private static void SubscribeToStreams(BitstampWebsocketClient client)
        {
            client.Streams.ErrorStream.Subscribe(x =>
                Log.Warning($"Error received, message: {x?.Data.Message}"));

            client.Streams.SubscriptionSucceededStream.Subscribe(x =>
            {
                Log.Information($"Subscribed to {x?.Symbol} {x?.Channel}");
            });

            client.Streams.UnsubscriptionSucceededStream.Subscribe(x =>
            {
                Log.Information($"Unsubscribed from {x?.Symbol} {x?.Channel}");
            });


            client.Streams.OrderBookStream.Subscribe(x =>
            {
                Log.Information($"Order book L2 [{x.Symbol}]");
                Log.Information($"    {x.Data?.Asks.FirstOrDefault()?.Price} " +
                                $"{x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} " +
                                $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Asks?.Length})");
                Log.Information($"    {x.Data?.Bids.FirstOrDefault()?.Price} " +
                                $"{x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} " +
                                $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Bids?.Length})");
            });

            client.Streams.OrdersStream.Subscribe(x =>
            {
                //Log.Information($"{x.Data} {x.Data.Asks[0]} {x.Data.EventBids[0]}");
                //Log.Information($"{x.Symbol} {x.Data.Channel} {x.Data.Amount}");
            });

            client.Streams.OrderBookDetailStream.Subscribe(x =>
            {
                Log.Information($"Order book L3 [{x.Symbol}]");
                Log.Information($"    {x.Data?.Asks.FirstOrDefault()?.Price} " +
                                $"{x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} " +
                                $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Asks?.Length}) " +
                                $"id: {x.Data?.Asks?.FirstOrDefault()?.OrderId}");
                Log.Information($"    {x.Data?.Bids.FirstOrDefault()?.Price} " +
                                $"{x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} " +
                                $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Bids?.Length}) " +
                                $"id: {x.Data?.Bids?.FirstOrDefault()?.OrderId}");
            });

            client.Streams.OrderBookDiffStream.Subscribe(x =>
            {
                Log.Information($"Order book L2 diffs [{x.Symbol}]");
                Log.Information($"    updates {x.Data?.Asks.Count(y => y.Amount > 0)} " +
                                $"deletes {x.Data?.Asks.Count(y => y.Amount <= 0)}  " +
                                $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Asks?.Length}) ");
                Log.Information($"    updates {x.Data?.Bids.Count(y => y.Amount > 0)} " +
                                $"deletes {x.Data?.Bids.Count(y => y.Amount <= 0)}  " +
                                $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                $"({x.Data?.Bids?.Length}) ");
            });


            client.Streams.HeartbeatStream.Subscribe(x =>
                Log.Information($"Heartbeat received, product: {x?.Channel}, seq: {x?.Event}"));

            client.Streams.TickerStream.Subscribe(x =>
            {
                Log.Information($"Trade executed [{x.Symbol}] {x.Data?.Side} price: {x.Data?.Price} size: {x.Data?.Amount}");
            });
        }

        private static SerilogLoggerFactory InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console(LogEventLevel.Debug)
                .CreateLogger();
            Log.Logger = logger;
            return new SerilogLoggerFactory(logger);
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
            ExitEvent.Set();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Log.Warning("Unloading process");
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Canceling process");
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}