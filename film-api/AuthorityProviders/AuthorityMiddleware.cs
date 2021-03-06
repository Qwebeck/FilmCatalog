﻿using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using FilmApi.Models;
using System;

namespace FilmApi.AuthorityProviders
{
    public abstract class AuthorityMiddleware
    {
        protected HttpClient httpClient = new HttpClient();
        public async Task<string> AddUser(UserDTO user) 
        {
            var message = CreateAddUserMessage(user);
            var response = await httpClient.SendAsync(message);
            var responseBody = await HandleResponse(response);
            var id = GetUserID(responseBody);
            return id;
        }

        protected async Task<string> HandleResponse(HttpResponseMessage message) 
        {
            string content = await message.Content.ReadAsStringAsync();
            if (message.StatusCode == HttpStatusCode.OK) 
            {
                return content;
            }
            else 
            {
                throw new ArgumentException(content);
            }
        }
        protected abstract HttpRequestMessage CreateAddUserMessage(UserDTO user);

        protected abstract string GetUserID(string response);
        public abstract Task<string[]> GetUserGroups(string userID);
        public abstract Task<bool> CheckIfAdministrator(string userID);
    }
}
