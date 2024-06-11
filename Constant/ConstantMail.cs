using Microsoft.AspNetCore.Identity.UI.Services;

namespace PRN221_Assignment.Constant
{
    public abstract class ConstantMail
    {
        public const string SenderName = "SocialV";
        public const string Sender = "socialvcop@gmail.com";
        public const string PasswordMail = "dewj ugsn wzcf ztqa";
        public const string RecoverPasswordContent = "We received a request for a password to use for your SocialV account.<br>" +
                                                     "Your password: ReplacePass <br>" +
                                                     "If you didn't request this code then you can safely ignore this email. Someone else may have entered your email address by mistake.<br>";
    }
}
