using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Type;

namespace BridgeportClaims.Data.SerializableValues
{
    public class SerializableValues
    {
        /// <summary>
        /// SPECIAL NOTE: the IType for the Json Deserializer was just an estimate.
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            if (names.Length != 1)
                throw new InvalidOperationException("Invalid column count");

            var val = rs[names[0]] as string;

            return !string.IsNullOrWhiteSpace(val) ? JsonConvert.DeserializeObject<IType>(val) : null;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var parameter = (DbParameter)cmd.Parameters[index];

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = JsonConvert.SerializeObject(value);
            }
        }
    }

    public class Error
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime ErrorDateTime { get; set; }
        public virtual Exception Exception { get; set; }
    }
}