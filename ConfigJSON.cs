using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldurBot
{
	/// <summary>
	/// simple struct that stores bot token
	/// and prefix for accessing commands
	/// </summary>
	public struct ConfigJSON
	{
		[JsonProperty("token")]
		public string Token { get; private set; }

		[JsonProperty("api")]
		public string ApiSecret { get; private set; }

	}
}
