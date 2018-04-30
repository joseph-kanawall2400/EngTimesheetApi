using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JiraApi
{
	public class Jira
	{
		private struct Issue
		{
			public string Key;
			public string Category;
		}

		private struct Worklog
		{
			public string Key;
			public string Author;
			public DateTime Started;
			public int Seconds;
		}

		private string _username;
		private string _password;
		private string _catField;
		private HttpClient _client;

		public Jira(string username, string password, string uri, string catField)
		{
			_username = username;
			_password = password;
			_catField = catField;

			_client = new HttpClient();
			_client.BaseAddress = new Uri(uri);
			_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}")));
		}

		public async Task<bool> IsAuthenticatedAsync() => (await _client.GetAsync("/rest/auth/1/session")).IsSuccessStatusCode;

		public async Task<Dictionary<string, int>> GetWorklogAsync(DateTime start, string user)
		{
			start = new DateTime(start.Year, start.Month, 1);
			DateTime end = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
			List<Issue> issues = await getIssueByTimeAsync(start, end, user);

			Dictionary<string, Dictionary<string, int>> worklogs = await GetWorklogsAsync(issues, start, end);

			return worklogs.ContainsKey(user) ? worklogs[user] : null;
		}

		public async Task<Dictionary<string, Dictionary<string, int>>> GetWorklogAsync(DateTime start)
		{
			start = new DateTime(start.Year, start.Month, 1);
			DateTime end = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
			List<Issue> issues = await getIssueByTimeAsync(start, end);

			return await GetWorklogsAsync(issues, start, end);
		}

		private async Task<Dictionary<string, Dictionary<string, int>>> GetWorklogsAsync(List<Issue> issues, DateTime start, DateTime end)
		{
			Dictionary<string, Dictionary<string, int>> worklog = new Dictionary<string, Dictionary<string, int>>();
			List<Task<List<Worklog>>> tasks = new List<Task<List<Worklog>>>();

			foreach(Issue issue in issues)
			{
				tasks.Add(getWorklogsByIssueAsync(issue.Key));
			}

			await Task.WhenAll(tasks.ToArray()).ContinueWith(result =>
			{
				foreach(List<Worklog> worklogs in result.Result)
				{
					foreach(Worklog wl in worklogs)
					{
						if(wl.Started.Date >= start.Date && wl.Started.Date <= end.Date)
						{
							if(!worklog.ContainsKey(wl.Author))
							{
								worklog.Add(wl.Author, new Dictionary<string, int>());
							}

							string category = issues.SingleOrDefault(x => x.Key == wl.Key).Category;
							if(!worklog[wl.Author].ContainsKey(category))
							{
								worklog[wl.Author].Add(category, 0);
							}
							
							worklog[wl.Author][category] += wl.Seconds;
						}
					}
				}
			});
			return worklog;
		}

		private async Task<List<Worklog>> getWorklogsByIssueAsync(string key)
		{
			HttpResponseMessage responseMessage = await _client.GetAsync($"/rest/api/2/issue/{key}/worklog");
			string result = await responseMessage.Content.ReadAsStringAsync();

			List<Worklog> worklogs = new List<Worklog>();
			foreach(JToken issueToken in JObject.Parse(result).SelectToken("worklogs"))
			{
				worklogs.Add(new Worklog()
				{
					Key = key,
					Author = issueToken.SelectToken("author.name").ToString(),
					Started = DateTime.Parse(issueToken.SelectToken("started").ToString()),
					Seconds = Convert.ToInt32(issueToken.SelectToken("timeSpentSeconds"))
				});
			}
			return worklogs;
		}

		private async Task<List<Issue>> getIssueByTimeAsync(DateTime start, DateTime end, string user = "")
		{
			string jql = Uri.EscapeUriString(String.IsNullOrWhiteSpace(user) ? "" : $"worklogAuthor={user} AND ");
			jql += Uri.EscapeUriString($"worklogDate>={start.ToString("yyyy-MM-dd")} AND worklogDate<={end.ToString("yyyy-MM-dd")}");
			HttpResponseMessage responseMessage = await _client.GetAsync($"/rest/api/2/search?fields={_catField}&jql=" + jql);
			string result = await responseMessage.Content.ReadAsStringAsync();

			List<Issue> issues = new List<Issue>();
			foreach(JToken issueToken in JObject.Parse(result).SelectToken("issues"))
			{
				Issue iss = new Issue();
				iss.Key = issueToken.SelectToken("key").ToString();
				iss.Category = issueToken.SelectToken($"fields.{_catField}.value")?.ToString() ?? "";
				issues.Add(iss);
			}
			return issues;
		}
	}
}
