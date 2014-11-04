using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Epi.Web.Enter.Common.Security
{
    /// <summary>
    /// Class to create salted hashed values for username / passwords.  A hash alone is not enough to defeat 
    /// most attacks so the current industry best practice is to "salt" password hashes     
    /// 
    /// 
    /// </summary>
    public class PasswordHasher
    {

        /// <summary>
        /// For added security the salt for the password hash is created with salt also  
        /// </summary>
        //  private const string SALT_FOR_SALT = "74$5^&75x%$%771";
        private string SALT_FOR_SALT;


        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHasher" /> class.
        /// </summary>
        /// <param name="SALT_FOR_SALT">The SAL t_ FO r_ SALT.</param>
        public PasswordHasher(string SALT_FOR_SALT)
        {
            this.SALT_FOR_SALT = SALT_FOR_SALT;
        }

        /// <summary>
        /// Returns a cryptographically strong random value created from 100 non-zero bytes     
        /// </summary>
        /// <returns></returns>
        //public static string CreateSalt()
        //{
        //    // First create a cryptographically strong random byte array to "salt the salt"  
        //    byte[] salt = new Byte[100];
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //    rng.GetNonZeroBytes(salt);


        //    return Convert.ToBase64String(salt);
        //}

        /// <summary>
        /// Returns the "salted" hashed salt for the pasword salt based on a user supplied str      
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>


        public string CreateSalt(string UserName)
        {
            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(UserName,
                System.Text.Encoding.UTF8.GetBytes(SALT_FOR_SALT), 10000);
            return Convert.ToBase64String(hasher.GetBytes(25));
        }


        /// <summary>
        /// Returns the "salted" hashed  password                   
        /// </summary>
        /// <param name="Salt"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string HashPassword(string Salt, string Password)
        {
            Rfc2898DeriveBytes Hasher = new Rfc2898DeriveBytes(Password,
                System.Text.Encoding.UTF8.GetBytes(Salt), 10000);
            return Convert.ToBase64String(Hasher.GetBytes(25));
        }
    }
}
