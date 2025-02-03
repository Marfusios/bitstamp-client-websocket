using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses.Books
{
    internal class OrderBookLevelConverter : JsonConverter
	{
        private readonly OrderBookSide _side;

		public OrderBookLevelConverter()
		{
		}

		public OrderBookLevelConverter(OrderBookSide side)
		{
			_side = side;
		}

		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(double[][]);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
			JsonSerializer serializer)
		{
			var array = JArray.Load(reader);
			return JArrayToTradingTicker(array);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

        private BookLevel[] JArrayToTradingTicker(JArray data)
		{
			var result = new List<BookLevel>();
			foreach (var item in data)
			{
				var array = item.ToArray();

				var level = new BookLevel();

				if (array.Length == 2)
				{
					level.Side = _side;
					level.Price = (double) array[0];
					level.Amount = (double) array[1];
				}
				else
				{
					level.Side = _side;
					level.Price = (double) array[0];
					level.Amount = (double) array[1];
					level.OrderId = (long) array[2];
				}

				result.Add(level);
			}

			return result.ToArray();
		}
	}
}