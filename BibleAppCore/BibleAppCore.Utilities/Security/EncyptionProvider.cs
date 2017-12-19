using BibleAppApi.Models;
using BibleAppCore.Utilities.ExtensionMethods;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace BibleAppCore.Utilities.Security
{
    
    public class EncyptionProvider : IEncyptionProvider
    {
        private IConfiguration Configuration { get; set; }
        public EncyptionProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Credentials HashPassword(Credentials credentials)
        {
            credentials.Password.HashString(Configuration.GetSection("Salt").Value);
            return credentials;
        }

    }
}
