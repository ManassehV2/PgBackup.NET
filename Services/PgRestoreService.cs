namespace PgBackup.Services
{
    public interface IPgRestoreService
    {
        bool RestoreFromeFile(string filePath);

    }
    internal class PgRestoreService : IPgRestoreService
    {
        public bool RestoreFromeFile(string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}