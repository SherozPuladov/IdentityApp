﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Invoice == null)
                return NotFound();


            Invoice = await Context.Invoice
                .FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (Invoice == null)
                return NotFound();


            var isAuthorized = await AuthorizationService
                .AuthorizeAsync(
                    User, Invoice, InvoiceOperations.Update);

            var isAdmin = User.IsInRole(Constants.InvoiceAdminsRole);

            if (isAuthorized.Succeeded == false && isAdmin == false)
                return Forbid();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var invoice = await Context.Invoice.AsNoTracking()
                .SingleOrDefaultAsync(m => m.InvoiceId == id);

            if(invoice == null)
                return NotFound();

            Invoice.CreatorId = invoice.CreatorId;

            var isAuthorized = await AuthorizationService
                .AuthorizeAsync(
                    User, Invoice, InvoiceOperations.Update);

            var isAdmin = User.IsInRole(Constants.InvoiceAdminsRole);

            if (isAuthorized.Succeeded == false && isAdmin == false)
                return Forbid();

            Invoice.Status = invoice.Status;

            Context.Attach(Invoice).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(Invoice.InvoiceId))
                    return NotFound();
                else
                    throw;
                
            }

            return RedirectToPage("./Index");
        }

        private bool InvoiceExists(int id)
        {
          return Context.Invoice.Any(e => e.InvoiceId == id);
        }
    }
}