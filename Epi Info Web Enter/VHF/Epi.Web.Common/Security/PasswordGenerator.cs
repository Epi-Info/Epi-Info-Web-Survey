using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace Epi.Web.Enter.Common.Security
{
    public class PasswordGenerator
    {
        public PasswordGenerator()
        {
            this.Minimum = DefaultMinimum = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMinimumLength"]);
            this.Maximum = DefaultMaximum = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMaximumLength"]);
            this.ConsecutiveCharacters = Convert.ToBoolean(ConfigurationManager.AppSettings["ConsecutiveCharacters"]); //false;
            this.RepeatCharacters = Convert.ToBoolean(ConfigurationManager.AppSettings["RepeatCharacters"]);	//= true;
            this.Symbols = ConfigurationManager.AppSettings["Symbols"].ToString();
            this.UseSymbols = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSymbols"]); //= false;
            this.UseNumeric = Convert.ToBoolean(ConfigurationManager.AppSettings["UseNumbers"]); //= false;
            this.UseLowerCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLowerCase"]);
            this.UseUpperCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUpperCase"]);
            this.Exclusions = null;

            rng = new RNGCryptoServiceProvider();
        }

        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // Assumes lBound >= 0 && lBound < uBound
            // returns an int >= lBound and < uBound
            uint urndnum;
            byte[] rndnum = new Byte[4];
            if (lBound == uBound - 1)
            {
                // test for degenerate case where only lBound can be returned   
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));

            do
            {
                rng.GetBytes(rndnum);
                urndnum = System.BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        protected char GetRandomCharacter(string pwdstring)
        {
            int upperBound = pwdstring.ToCharArray().GetUpperBound(0);



            int randomCharPosition = GetCryptographicRandomNumber(pwdstring.ToCharArray().GetLowerBound(0), upperBound);

            char randomChar = pwdstring.ToCharArray()[randomCharPosition];

            return randomChar;
        }

        public string Generate()
        {
            // Pick random length between minimum and maximum   
            int pwdLength = GetCryptographicRandomNumber(this.Minimum, this.Maximum);

            StringBuilder pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = this.Maximum;

            // Generate random characters
            char lastCharacter, nextCharacter;

            if (true == this.UseSymbols)
            {
                passwordArrary = new string(pwdCharArray) + ConfigurationManager.AppSettings["Symbols"].ToString();
                pwdCharArray = passwordArrary.ToCharArray();
            }

            // Initial dummy character flag
            lastCharacter = nextCharacter = '\n';

            for (int i = 0; i < pwdLength; i++)
            {
                //nextCharacter = GetRandomCharacter(new string(pwdCharArray));

                if (UseLowerCase && !lowerExists)//&& !lowerExists)
                {
                    nextCharacter = GetRandomCharacter(lowercasealph);
                    lowerExists = true;
                }

                else if (UseUpperCase && !upperExists)
                {
                    nextCharacter = GetRandomCharacter(uppercasealph);
                    upperExists = true;
                }

                else if (UseSymbols && !symbolExists)
                {
                    nextCharacter = GetRandomCharacter(ConfigurationManager.AppSettings["Symbols"].ToString());
                    symbolExists = true;
                }

                else if (UseNumeric && !numericExists)
                {
                    nextCharacter = GetRandomCharacter(numeric);
                    numericExists = true;
                }


                if (false == this.ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter(new string(pwdCharArray));
                    }
                }

                if (false == this.RepeatCharacters)
                {
                    string temp = pwdBuffer.ToString();
                    int duplicateIndex = temp.IndexOf(nextCharacter);
                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter(new string(pwdCharArray));
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != this.Exclusions))
                {
                    while (-1 != this.Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter(new string(pwdCharArray));
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Exclusions: Specifies the set of characters to exclude in password generation.
        /// </summary>
        public string Exclusions
        {
            get { return this.exclusionSet; }
            set { this.exclusionSet = value; }
        }

        /// <summary>
        /// Specifies the minimum length of the generated password.
        /// </summary>
        public int Minimum
        {
            get { return this.minSize; }
            set
            {
                this.minSize = value;
                if (this.DefaultMinimum > this.minSize)
                {
                    this.minSize = this.DefaultMinimum;
                }
            }
        }

        /// <summary>
        /// Maximum: Specifies the maximum length of the generated password.
        /// </summary>
        public int Maximum
        {
            get { return this.maxSize; }
            set
            {
                this.maxSize = value;
                if (this.minSize >= this.maxSize)
                {
                    this.maxSize = this.DefaultMaximum;
                }
            }
        }

        /// <summary>
        /// ExcludeSymbols: Excludes symbols from the set of characters used to generate the password
        /// </summary>
        public bool UseSymbols
        {
            get { return this.hasSymbols; }
            set { this.hasSymbols = value; }
        }

        /// <summary>
        /// Switch to Use Upper Case
        /// </summary>
        public bool UseUpperCase
        {
            get { return this.hasUpperCase; }
            set { this.hasUpperCase = value; }
        }

        /// <summary>
        /// Switch to read symbols
        /// </summary>
        public string Symbols { get; set; }

        /// <summary>
        /// Switch to Use lower case
        /// </summary>
        public bool UseLowerCase
        {
            get { return this.hasLowerCase; }
            set { this.hasLowerCase = value; }
        }

        /// <summary>
        /// Switch to use numbers
        /// </summary>
        public bool UseNumeric
        {
            get { return this.hasNumber; }
            set { this.hasNumber = value; }
        }

        /// <summary>
        /// RepeatingCharacters: Controls generation of repeating characters in the generated password.
        /// </summary>
        public bool RepeatCharacters
        {
            get { return this.hasRepeating; }
            set { this.hasRepeating = value; }
        }

        /// <summary>
        /// //ConsecutiveCharacters: Controls generation of consecutive characters in the generated password.
        /// </summary>
        public bool ConsecutiveCharacters
        {
            get { return this.hasConsecutive; }
            set { this.hasConsecutive = value; }
        }

        private int DefaultMinimum;// = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMinimumLength"]);
        private int DefaultMaximum;// = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMaximumLength"]);
        //private int UBoundDigit = Convert.ToInt32(ConfigurationManager.AppSettings["UBoundDigit"]);

        private RNGCryptoServiceProvider rng;
        private int minSize;
        private int maxSize;
        private bool hasRepeating;
        private bool hasConsecutive;
        private bool hasSymbols;
        private bool hasUpperCase;
        private bool hasLowerCase;
        private bool hasNumber;
        private string exclusionSet;
        private char[] pwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private string lowercasealph = "abcdefghijklmnopqrstuvwxyz";
        private string uppercasealph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string numeric = "0123456789";
        bool lowerExists = false, upperExists = false, numericExists = false, symbolExists = false;

        string passwordArrary = "";
    }
}

