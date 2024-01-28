using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{
    public class MISACanNotDeleteForeignField: Exception
    {
        private string MsgError = string.Empty;
        public string entity { get; }
        public MISACanNotDeleteForeignField(string error, string entity)
        {
            this.MsgError = error;
            this.entity = entity;
        }

        public override string Message => this.MsgError;
    }
}
