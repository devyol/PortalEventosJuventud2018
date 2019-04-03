using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EVE01.DO.DATA;

namespace EVE01.UI.Clases
{
    public class UserRepository
    {
        public bool ValidateUser(string username, string password)
        {
            using (var db = new EntitiesEVE01())
            {
                var result = from u in db.EVE01_USUARIO where (u.USUARIO.ToUpper() == username.ToUpper()) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (SEG01_DO.Encriptor.validarPassword(password, dbuser.PASSWORD))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }
    }
}