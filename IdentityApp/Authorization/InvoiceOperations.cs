using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization;

public class InvoiceOperations
{
    public static OperationAuthorizationRequirement Create =
        new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };

    public static OperationAuthorizationRequirement Read =
        new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };

    public static OperationAuthorizationRequirement Update =
        new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };

    public static OperationAuthorizationRequirement Delete =
        new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };

    public static OperationAuthorizationRequirement Approved =
        new OperationAuthorizationRequirement { Name = Constants.ApprovedOperationName };

    public static OperationAuthorizationRequirement Rejected =
        new OperationAuthorizationRequirement { Name = Constants.RejectedOperationName };
}
