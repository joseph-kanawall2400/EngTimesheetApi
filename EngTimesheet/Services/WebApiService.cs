using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EngTimesheet.Services
{
	public class WebApiService
	{
		private HttpClient _client;

		public WebApiService(string uri)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri(uri);
		}

		public async Task<string> Login(string email, string password)
		{
			HttpResponseMessage response = await _client.GetAsync($"/api/account/login?username={email}&password={password}");
			return response.IsSuccessStatusCode
				? ((JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()))["token"].ToString()
				: null;
		}
	}
}
