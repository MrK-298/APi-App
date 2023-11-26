namespace WebApplication1.Function
{
    public static class Blacklist
    {
        private static List<string> _tokens = new List<string>();

        public static void Add(string token)
        {
            _tokens.Add(token);
        }

        public static bool Contains(string token)
        {
            return _tokens.Contains(token);
        }
    }

}
