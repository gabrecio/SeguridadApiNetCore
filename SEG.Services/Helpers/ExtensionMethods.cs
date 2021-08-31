using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEG.Services.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<UsuarioViewModel> WithoutPasswords(this IEnumerable<UsuarioViewModel> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static UsuarioViewModel WithoutPassword(this UsuarioViewModel user)
        {
            user.passwd = null;
            return user;
        }
    }
}
