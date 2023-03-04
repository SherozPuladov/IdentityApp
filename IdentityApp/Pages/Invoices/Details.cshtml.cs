using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices;

public class DetailsModel : DI_BasePageModel
{
    public DetailsModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
    {
    }

    public Invoice Invoice { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || Context.Invoice == null)
        {
            return NotFound();
        }

        Invoice = await Context.Invoice
            .FirstOrDefaultAsync(m => m.InvoiceId == id);
        
        if (Invoice == null)
        {
            return NotFound();
        }

        var isCreator = await AuthorizationService
            .AuthorizeAsync(
                User, Invoice, InvoiceOperations.Read);

        var isManager = User.IsInRole(Constants.InvoiceManagersRole);

        var isAdmin = User.IsInRole(Constants.InvoiceAdminsRole);

        if (isCreator.Succeeded == false 
                && isManager == false 
                && isAdmin == false)
            return Forbid();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, InvoiceStatus status)
    {
        Invoice = await Context.Invoice
            .FirstAsync(m => m.InvoiceId == id);

        if (Invoice == null)
            return NotFound();

        var invoiceOperation = status == InvoiceStatus.Approve 
            ? InvoiceOperations.Approve
            : InvoiceOperations.Reject;

        var isAuthorized = await AuthorizationService
            .AuthorizeAsync(
                User, Invoice, invoiceOperation);

        if (isAuthorized.Succeeded == false)
            return Forbid();

        Invoice.Status = status;

        Context.Invoice.Update(Invoice);

        await Context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}