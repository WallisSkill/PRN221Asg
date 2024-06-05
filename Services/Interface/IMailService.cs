namespace PRN221_Assignment.Services.Interface
{
    public interface IMailService
    {
        bool SendMail(string _to, string _subject, string _body);
    }
}
