using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
	/// <summary>
	/// Static class used in views which access requires authorization, to check whether the user has been assigned to the role authorizing him to run a given view
	/// </summary>
	public static class RoleAssignExtension
    {
		/// <summary>
		/// The method checks that the user is assigned to all provided roles
		/// </summary>
		/// <param name="principal">Interface for checking if the user is assigned to the provided role</param>
		/// <param name="roles">User-assigned roles in the application</param>
		/// <returns>Returns true if the user is assigned to each role. Otherwise - returns false</returns>
		public static bool IsInAllRoles(this IPrincipal principal, params string[] roles)
		{
			return roles.All(r1 => r1.Split(',').All(r2 => principal.IsInRole(r2.Trim())));
		}
		/// <summary>
		/// The method checks that the user is assigned to any provided roles
		/// </summary>
		/// <param name="principal">Interface for checking if the user is assigned to the provided role</param>
		/// <param name="roles">User-assigned roles in the application</param>
		/// <returns>Returns true if the user is assigned to any role. Otherwise - returns false</returns>
		public static bool IsInAnyRoles(this IPrincipal principal, params string[] roles)
		{
			return roles.Any(r1 => r1.Split(',').Any(r2 => principal.IsInRole(r2.Trim())));
		}
	}
}
