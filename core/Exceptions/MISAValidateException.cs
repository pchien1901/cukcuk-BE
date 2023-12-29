using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{
    public class MISAValidateException: Exception
    {
        private string MsgError = string.Empty;
        public string FieldName { get; }
        public MISAValidateException(string error, string fieldName)
        {
            this.MsgError = error;
            this.FieldName = fieldName;
        }

        public override string Message => this.MsgError;
    }
}
