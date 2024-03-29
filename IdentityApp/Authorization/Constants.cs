﻿namespace IdentityApp.Authorization;

public class Constants
{
    public static readonly string CreateOperationName = "Create";
    public static readonly string ReadOperationName = "Read";
    public static readonly string UpdateOperationName = "Update";
    public static readonly string DeleteOperationName = "Delete";

    public static readonly string ApprovedOperationName = "Approve";
    public static readonly string RejectedOperationName = "Reject";

    public static readonly string InvoiceAdminsRole = "InvoiceAdmin";
    public static readonly string InvoiceManagersRole = "InvoiceManger";
}