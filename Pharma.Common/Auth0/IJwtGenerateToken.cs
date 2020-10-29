using System.Threading.Tasks;

namespace Pharma.Common.Auth0
{
    public interface IJwtGenerateToken
    {
        Task<string> GenerateToken();
    }
}
