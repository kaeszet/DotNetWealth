using DotNetWMS.Resources;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DotNetWMSTests.StaticWrapper
{
    public class StaticWrapper : IStaticWrapper
    {
        public bool IsInAllRoles(IPrincipal principal, params string[] roles)
        {
            return RoleAssignExtension.IsInAllRoles(principal, roles);
        }
        public bool IsInAnyRoles(IPrincipal principal, params string[] roles)
        {
            return RoleAssignExtension.IsInAnyRoles(principal, roles);
        }
    }
}
