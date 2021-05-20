using ChatbotAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Helpers {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter {
        private string _role;

        public string Role {
            get {
                return _role ?? String.Empty;
            }
            set {
                _role = value;
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
            var user = (User)context.HttpContext.Items["User"];
            if (user == null) {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            if (_role != null) {
                if (user.Role.Equals("Admin")) {
                    return;
                }
                else if (user.Role.Equals("Staff") && _role.Equals("Staff")) {
                    return;
                }
                else {
                    context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                    return;
                }
            }
        }
    }
}
