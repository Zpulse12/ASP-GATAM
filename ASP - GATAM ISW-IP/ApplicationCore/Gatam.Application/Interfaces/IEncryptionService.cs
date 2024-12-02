using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    internal interface IEncryptionService
    {
        string Encrypt(string plaintext);
        string Decrypt(string cyphertext);
    }
}
