using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Throw.Models
{
    public class ErrorLogDataAccessLayer
    {

        ThrowSQLContext db = new ThrowSQLContext();

        public int AddError(ErrorLog error)
        {
            try
            {
                db.ErrorLog.Add(error);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
