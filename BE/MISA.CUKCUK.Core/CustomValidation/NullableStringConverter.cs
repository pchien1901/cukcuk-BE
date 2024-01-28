using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class NullableStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var @string = reader.GetString();
            if(string.IsNullOrEmpty(@string))
            {
                return null;
            }
            else
            {
                return default(string);
            }
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
