namespace Hushigoeuf
{
    public static class HGLabelExtensions
    {
        /// <summary>
        /// Возвращает строку из заданного числа таким образом, чтобы 99 превратились в "099" или "0099".
        /// </summary>
        public static string HGToCtrlString(this int target, int count, string symbol = "0")
        {
            var result = target + "";
            for (var i = result.Length; i < count; i++)
                result = symbol + result;
            return result;
        }
    }
}