using System;
using System.Text.Json;
using FilmApi.Models;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Linq;
using FilmApi.Utils.Attributes;
using System.Collections.Generic;

namespace FilmApi.Utils.JSONConverters
{
    public class UserDTOConverter: JsonConverter<UserDTO>
    {
        public override void Write(Utf8JsonWriter writer, UserDTO user, JsonSerializerOptions options)
        {
            var properties = user.GetType().GetProperties();
            var output = "{";
            var credentialsOutput = "\"credentials\": {";
            foreach (var property in properties)
            {
                var field = $"\"{char.ToLower(property.Name[0]) + property.Name.Substring(1)}\": \"{property.GetValue(user)}\"";
                if (property.IsDefined(typeof(CredentialsAttribute)))
                {
                    if (credentialsOutput[^1] != '{') credentialsOutput += ",";
                    credentialsOutput += field;
                }
                else
                {
                    if (output[^1] != '{') output += ","; 
                        output += field;
                }
                    
            }
            credentialsOutput += "}";
            output += "," + credentialsOutput + "}";
            output = output.Replace("\u0022", "\"");
            writer.WriteStringValue(output);
        }

        public override UserDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
