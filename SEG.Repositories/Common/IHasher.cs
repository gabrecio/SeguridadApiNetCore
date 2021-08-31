using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Repositories.Common
{
    public interface IHasher
    {
        int SaltSize { get; set; }
        string Encrypt(string original);
        bool CompareStringToHash(string s, string hash);
    }
}
