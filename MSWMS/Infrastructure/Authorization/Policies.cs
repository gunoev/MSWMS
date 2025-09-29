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
            policy.RequireRole(nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequireManager, policy =>
            policy.RequireRole(nameof(Role.RoleType.Manager), nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequirePicker, policy =>
            policy.RequireRole(nameof(Role.RoleType.Picker), nameof(Role.RoleType.Manager), nameof(Role.RoleType.Admin)));
    }
}