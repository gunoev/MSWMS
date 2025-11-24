using Microsoft.AspNetCore.Authorization;
using MSWMS.Entities;

namespace MSWMS.Infrastructure.Authorization;

public static class Policies
{
    public const string RequireAdmin = "RequireAdmin";
    public const string RequireManager = "RequireManager";
    public const string RequirePicker = "RequirePicker";
    public const string RequireDispatcher = "RequireDispatcher";
    public const string RequireLoadingOperator = "RequireLoadingOperator";
    public const string RequireManagerOrDispatcher = "RequireManagerOrDispatcher";
    public const string RequireManagerOrPicker = "RequireManagerOrPicker";
    public const string RequireDispatcherOrLoadingOperator = "RequireDispatcherOrLoadingOperator";

    
    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(RequireAdmin, policy =>
            policy.RequireRole(nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequireManager, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Manager), 
                nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequirePicker, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Picker), 
                nameof(Role.RoleType.Manager), 
                nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequireDispatcher, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Dispatcher), 
                nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequireLoadingOperator, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.LoadingOperator), 
                nameof(Role.RoleType.Manager),
                nameof(Role.RoleType.Dispatcher), 
                nameof(Role.RoleType.Admin)));
        
        options.AddPolicy(RequireManagerOrDispatcher, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Manager), 
                nameof(Role.RoleType.Dispatcher), 
                nameof(Role.RoleType.Admin)));

        options.AddPolicy(RequireManagerOrPicker, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Manager), 
                nameof(Role.RoleType.Picker), 
                nameof(Role.RoleType.Admin)));

        options.AddPolicy(RequireDispatcherOrLoadingOperator, policy =>
            policy.RequireRole(
                nameof(Role.RoleType.Dispatcher), 
                nameof(Role.RoleType.LoadingOperator), 
                nameof(Role.RoleType.Admin)));
    }
}