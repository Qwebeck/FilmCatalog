using FilmApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FilmApi.AuthorityProviders
{
    public class OktaMiddleware : AuthorityMiddleware
    {
        private const string oktaToken = "00-DmEWPPkypwzLTK1bdHExPWe6oGSgtrDEEIZYPjQ";
        private const string userApiUrl = "https://dev-221155.okta.com/api/v1/users";

        protected override HttpRequestMessage CreateAddUserMessage(UserDTO user)
        {

            var requestBody = new
            {
                profile = new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    login = user.Login,
                    email = user.Email
                },
                credentials = new
                {
                    password = new
                    {
                        value = user.Password
                    }
                }
            };
            var content = JsonConvert.SerializeObject(requestBody);
            return new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{userApiUrl}?activate=true"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"SSWS {oktaToken}"},
                },
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
        }

        protected override string GetUserID(string content)
        {
            var matches = Regex.Match(content, "\\\"id\\\":.?\\\"(?<id>.+?)\\\",");
            string id = matches.Groups["id"].Value;
            if (id == "") throw new NoIdForCreatedUserException();
            return id;
        }

        public override async Task<string[]> GetUserGroups(string userID)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{userApiUrl}/{userID}/groups"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"SSWS {oktaToken}"},
                }
            };
            var response = await httpClient.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();
            var matches = Regex.Matches(content, "\"profile\":[^{]*{\"name\":[^\"]*\"(?<groups>.[^\"]*)");
            return matches.Select(match => match.Groups["groups"].Value).ToArray();
        }
        public override async Task<bool> CheckIfAdministrator(string userID)
        {
            var groups = await GetUserGroups(userID);
            return groups.Contains("Administrators");
        }
    }
}
