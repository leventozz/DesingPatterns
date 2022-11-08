using BaseProject.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.Template.UserCards
{
    public class UserCardTagHelper : TagHelper
    {
        public AppUser AppUser{ get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate template;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                template = new PrimeUserCardTemplate();
            else
                template = new DefaultUserCardTemplate();

            template.SetUser(AppUser);

            output.Content.SetHtmlContent(template.Build());
        }
    }
}
