namespace Hushigoeuf.Basic
{
    public static class HGLabelExtensions
    {
        public static string HGToCtrlString(this int target, int count, string symbol = "0")
        {
            string result = target + "";
            for (int i = result.Length; i < count; i++)
                result = symbol + result;
            return result;
        }
    }
}