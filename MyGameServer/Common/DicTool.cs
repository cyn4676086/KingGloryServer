using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class DicTool
    {
        public static T2 GetValue<T1,T2>(Dictionary<T1,T2> dic ,T1 key)
        {
            T2 value;
            bool IsSuccess = dic.TryGetValue(key, out value);
            if (IsSuccess)
            {
                return value;
            }
            else
            {
                return default(T2);
            }
        }
    }
}
