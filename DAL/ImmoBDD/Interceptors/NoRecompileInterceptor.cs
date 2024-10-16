using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImmoBDD.Interceptors
{
    public class NoRecompileInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
        {
            // if(commandText.Contains("--NORECOMPIle"){
            //
            //}
            return base.CommandCreating(eventData, result);
        }
    }
}
