using Newtonsoft.Json;

namespace TradingApp.Web.Api.Extensions
{
    public static class UtilHelpers
    {
        public static string ToJSON(this object @object) => JsonConvert.SerializeObject(@object, Formatting.None);
    }
}
