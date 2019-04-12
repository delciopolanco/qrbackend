using qrbackend.Models.ViewModels.Login;

namespace qrbackend.Models.ViewModels.Token
{
    public class JwtRefreshToken : LoginModelBase
    {
        public string RefreshToken { get; set; }

        public JwtRefreshToken()
        {
            this.FunctionName = "SaveRefreshToken";
        }
    }
}
