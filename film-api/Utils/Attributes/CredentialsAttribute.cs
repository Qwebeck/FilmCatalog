using System;
namespace FilmApi.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field)
    ]
    public class CredentialsAttribute: Attribute
    {
        public CredentialsAttribute() { }
    }
}
