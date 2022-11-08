namespace WebApp.Template.UserCards
{
    public class DefaultUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            return String.Empty;
        }

        protected override string SetPicture()
        {
            return $"<img src='/userpictures/Sample_User_Icon.png' class='card-img-top'>";
        }
    }
}
