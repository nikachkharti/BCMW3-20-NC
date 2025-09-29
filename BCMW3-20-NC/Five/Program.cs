namespace Five
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string text = "Hello";
            //string text2 = "HELLO";

            //Console.WriteLine(text.Length); // ტექსტის ზომა
            //bool result = text.Contains("World"); // შეიცავს თუ არა ტექსტი ქვეტექსტს.
            //bool result = text.StartsWith("hz", StringComparison.OrdinalIgnoreCase);
            //bool result = text.EndsWith("hz", StringComparison.OrdinalIgnoreCase);

            //string result = text.ToUpper();
            //string result = text.ToLower();

            //var result = text.Replace("he", "Zj", StringComparison.OrdinalIgnoreCase);
            //var result = text.Trim('H', 'e');
            //var result = text.TrimStart();
            //var result = text.TrimEnd();

            //string[] splitedText = text.Split(',', StringSplitOptions.TrimEntries);
            //string joinedText = string.Join('|', splitedText);

            //var result = text.Substring(0, 4);

            //var result = text
            //    .ToLower()
            //    .IndexOf("o".ToLower());

            //var result = text
            //    .ToLower()
            //    .LastIndexOf("o".ToLower());


            //int x = 12;
            //string xString = x.ToString();

            //var result = text.Equals(text2, StringComparison.OrdinalIgnoreCase);


            //var result = text.CompareTo(text2); //0  1  -1



            var result = CutomStartsWith("Hello World", "hello", StringComparison.OrdinalIgnoreCase);

        }

        static bool CutomStartsWith(string source, string value, StringComparison stringComparison)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
                return false;

            if (value.Length > source.Length)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (stringComparison.Equals(StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.Equals(source[i].ToString(), value[i].ToString(), stringComparison))
                        return false;
                }
            }

            return true;
        }

    }
}
