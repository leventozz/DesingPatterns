using System.Text;

namespace WebApp.Template.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();
            sb.Append("<a href='#'class='card-link'>Send Message</a>");
            sb.Append("<a href='#'class='card-link'>Profile Detail</a>");
            return sb.ToString();
        }

        protected override string SetPicture()
        {
            return $"<img src='{AppUser.PictureUrl}' class='card-img-top'>";
        }
    }
}
