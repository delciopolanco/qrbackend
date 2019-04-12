namespace qrbackend.Api.Services.Authentication.Security
{
    public interface ISecurity
    {
        void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPassWordHash(string password, byte[] storeHash, byte[] storedSalt);
    }
}
