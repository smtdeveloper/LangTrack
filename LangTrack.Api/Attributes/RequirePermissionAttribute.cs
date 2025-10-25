using Microsoft.AspNetCore.Authorization;

namespace LangTrack.Api.Attributes;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string resource, string action)
    {
        Policy = $"{resource}:{action}";
    }
}
