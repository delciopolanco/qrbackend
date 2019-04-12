using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace qr_backend.Helpers
{
    public static class JwtManager
    {
        public static JwtData GetDataFromJWT(string jwt)
        {

            JwtData data = new JwtData();
            try
            {
                if (string.IsNullOrEmpty(jwt))
                    return null;


                var jwtHandler = new JwtSecurityTokenHandler();
                var isReadable = jwtHandler.CanReadToken(jwt);

                if (isReadable != true)
                    return null;


                var token = jwtHandler.ReadJwtToken(jwt);
                var claims = token.Claims;
                var claimValue = string.Empty;

                foreach (PropertyInfo prop in typeof(JwtData).GetProperties())
                {
                    try
                    {
                        claimValue = claims.FirstOrDefault(x => x.Type.ToLower().Contains(prop.Name.ToLower())).Value;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                  
                    data[prop.Name] = claimValue;
                }        
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
    }
}
