using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Invoice> Invoice { get;set; } = default!;
        public async Task OnGetAsync()
        {
            var invoices = from i in Context.Invoice
                           select i;

            var isAdmin = User.IsInRole(Constants.InvoiceAdminsRole);            
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            var currentUserId = UserManager.GetUserId(User);

            if(isManager == false && isAdmin == false)
            {
                invoices = invoices.Where(i => i.CreatorId == currentUserId);
            }

            Invoice = await invoices.ToListAsync();
        }
    }
}
