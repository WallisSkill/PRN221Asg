namespace PRN221_Assignment.Repository.Interface
{
    public interface IAdminRepository
    {
        List<int> GetBlockedAccount();
        void AddBlockedAccount(int newId);
        List<string> GetAllBadWords();
        void RemoveBlockedAccount(int userId);
    }
}
