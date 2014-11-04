using System.IO;
using System.Web.UI;

namespace MvcDynamicForms.Utilities
{
    public static class SerializationUtility
    {
        public static string Serialize(object obj)
        {            
            StringWriter writer = new StringWriter();
            new LosFormatter().Serialize(writer, obj);
            return writer.ToString();
        }
        public static T Deserialize<T>(string data)
        {
            if (data == null) return default(T);
            return (T)(new LosFormatter()).Deserialize(data);
        }
    }
}