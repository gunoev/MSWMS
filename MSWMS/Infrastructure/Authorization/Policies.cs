using Microsoft.AspNetCore.Authorization;
using MSWMS.Entities;

namespace MSWMS.Infrastructure.Authorization;

public static class Policies
{
    public const string RequireAdmin = "RequireAdmin";
    public const string RequireManager = "RequireManager";
    public const string RequirePicker = "RequirePicker";
    
    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(RequireAdmin, policy =>
            policy.RequireRole(Role.RoleType.Admin.ToString()));
        
        options.AddPolicy(RequireManager, policy =>
            policy.RequireRole(Role.RoleType.Manager.ToString(), Role.RoleType.Admin.ToString()));
        
        options.AddPolicy(RequirePicker, policy =>
            policy.RequireRole(Role.RoleType.Picker.ToString(), Role.RoleType.Manager.ToString(), Role.RoleType.Admin.ToString()));
    }
}