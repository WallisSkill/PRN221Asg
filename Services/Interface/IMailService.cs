namespace PRN221_Assignment.Services.Interface
{
    public interface IMailService
    {
        bool SendMail(string to, string subject, string body);
    }
}
