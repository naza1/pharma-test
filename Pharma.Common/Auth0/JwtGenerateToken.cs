using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Pharma.Common.Auth0
{
    public class JwtGenerateToken : IJwtGenerateToken
    {
        private readonly Auth0Config _auth0Config;

        public JwtGenerateToken(IOptions<Auth0Config> auth0Config)
        {
            _auth0Config = auth0Config.Value;
        }

        public async Task<string> GenerateToken()
        {
            var client = new RestClient(_auth0Config.TokenUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=4kh5T4vnB77mwh3T28tYK4y1DUN5cZ3m&client_secret=iXCwgxdEq-KSCsUjDFvZJOdUUOsxkltPTKMAhb3coUdU-5Z-pYQKBDszJYL8qZwh&audience=https://dev-ujgo43lj.auth0.com/api/v2/", ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);

            var jObject = JObject.Parse(response.Content);
            
            return jObject["access_token"].ToString();
        }
    }
}
