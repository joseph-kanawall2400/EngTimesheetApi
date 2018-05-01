using EngTimesheet.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

		public async Task<string> SendPasswordEmail(string email)
		{
			HttpResponseMessage response = await _client.PostAsync($"/api/account/password/{email}", null);
			await CheckException(response);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task ResetPassword(string token, string password)
		{
			HttpResponseMessage response = await _client.PostAsync("/api/account/password", GenerateContent(new AccountPasswordDTO()
			{
				Token = token,
				Password = password
			}));
			await CheckException(response);
		}

		public async Task<List<TimeDTO>> GetTimes(string token, int id, DateTime date)
		{
			HttpResponseMessage response = await AuthorizedGetAsync(token, $"/api/users/{id}/times/{date:yyyy-MM}");
			await CheckException(response);
			return JsonConvert.DeserializeObject<List<TimeDTO>>(await response.Content.ReadAsStringAsync());
		}

		public async Task UpdateTime(string token, int id, TimeDTO time)
		{
			HttpResponseMessage response = await AuthorizedPostAsync(token, $"/api/users/{id}/times", GenerateContent(time));
			await CheckException(response);
		}

		public async Task<string> Register(string email, string firstName, string lastName)
		{
			HttpResponseMessage response = await _client.PostAsync("/api/account/register", GenerateContent(new AccountRegisterDTO()
			{
				Email = email,
				FirstName = firstName,
				LastName = lastName
			}));
			await CheckException(response);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<UserDTO> GetUser(string token)
		{
			HttpResponseMessage response = await _client.GetAsync($"/api/users/{token}");
			await CheckException(response);
			return JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
		}

		public async Task<List<UserDTO>> GetUsers(string token)
		{
			HttpResponseMessage response = await AuthorizedGetAsync(token, "/api/users");
			await CheckException(response);
			return JsonConvert.DeserializeObject<List<UserDTO>>(await response.Content.ReadAsStringAsync());
		}

		public async Task UpdateUser(string token, UserDTO model)
		{
			HttpResponseMessage response = await AuthorizedPostAsync(token, "/api/users", GenerateContent(model));
			await CheckException(response);
		}

		private async Task CheckException(HttpResponseMessage response)
		{
			if(!response.IsSuccessStatusCode)
			{
				throw new Exception(JsonConvert.DeserializeObject<JToken>(await response.Content.ReadAsStringAsync()).Pretty());
			}
		}

		private Task<HttpResponseMessage> AuthorizedGetAsync(string token, string uri)
		{
			return AuthorizedSendAsync(token, new HttpRequestMessage(HttpMethod.Get, uri));
		}

		private Task<HttpResponseMessage> AuthorizedPostAsync(string token, string uri, HttpContent content)
		{
			return AuthorizedSendAsync(token, new HttpRequestMessage(HttpMethod.Post, uri) { Content = content });
		}

		private async Task<HttpResponseMessage> AuthorizedSendAsync(string token, HttpRequestMessage request)
		{
			_client.DefaultRequestHeaders.Add("AuthToken", token);
			HttpResponseMessage response = await _client.SendAsync(request);
			_client.DefaultRequestHeaders.Remove("AuthToken");
			return response;
		}

		private HttpContent GenerateContent(object o)
		{
			HttpContent content = new StringContent(JsonConvert.SerializeObject(o));
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return content;
		}
	}
}
