using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

		public async Task SendPasswordEmail(string email)
		{
			HttpResponseMessage response = await _client.PostAsync($"/api/account/password/{email}", null);
			if(!response.IsSuccessStatusCode)
			{
				throw new Exception(JsonConvert.DeserializeObject<JToken>(await response.Content.ReadAsStringAsync()).Pretty());
			}
		}

		public async Task ResetPassword(string token, string password)
		{
			HttpResponseMessage response = await _client.PostAsync("/api/account/password", GenerateContent(new
			{
				Token = token,
				Password = password
			}));

			if(!response.IsSuccessStatusCode)
			{
				throw new Exception(JsonConvert.DeserializeObject<JToken>(await response.Content.ReadAsStringAsync()).Pretty());
			}
		}

		public async Task Register(string email, string firstName, string lastName)
		{
			HttpResponseMessage response = await _client.PostAsync("/api/account/register", GenerateContent(new
			{
				Email = email,
				FirstName = firstName,
				LastName = lastName
			}));

			if(!response.IsSuccessStatusCode)
			{
				throw new Exception(JsonConvert.DeserializeObject<JToken>(await response.Content.ReadAsStringAsync()).Pretty());
			}
		}

		private HttpContent GenerateContent(object o)
		{
			HttpContent content = new StringContent(JsonConvert.SerializeObject(o));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return content;
		}
	}
}
