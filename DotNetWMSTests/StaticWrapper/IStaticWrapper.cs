using System.Security.Principal;

namespace DotNetWMSTests.StaticWrapper
{
    public interface IStaticWrapper
    {
        bool IsInAllRoles(IPrincipal principal, params string[] roles);
        bool IsInAnyRoles(IPrincipal principal, params string[] roles);
    }
}