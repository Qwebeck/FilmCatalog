using FilmApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FilmApi.AuthorityProviders
{
    public class OktaMiddleware: AuthorityMiddleware
    {
        private const string oktaToken = "00-DmEWPPkypwzLTK1bdHExPWe6oGSgtrDEEIZYPjQ";
        private const string authorityUrl = "https://dev-221155.okta.com/api/v1/users?activate=true";

        protected override HttpRequestMessage CreateMessage(UserDTO user)
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
                RequestUri = new Uri(authorityUrl),
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


        public async Task<bool> CheckIfAdministrator(string userID)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://dev-221155.okta.com/api/v1/users/{userID}/groups"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"SSWS {oktaToken}"},
                }
            };
            var response = await httpClient.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();
            return content.Contains("Administrators");
        }
    }
}
