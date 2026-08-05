using System;
using System.Text;

public static class Logger
{
    public static string FormatLogMessage(string template, params object[] args)
    {
        string ReplacePlaceholders(ReadOnlySpan<char> messageTemplate, object[] arguments)
        {
            var sb = new StringBuilder(messageTemplate.Length + arguments.Length * 8);

            int i = 0;
            while (i < messageTemplate.Length)
            {
                char c = messageTemplate[i];

                if (c == '{')
                {
                    int closeIndex = messageTemplate.Slice(i).IndexOf('}');
                    if (closeIndex > 0)
                    {
                        ReadOnlySpan<char> indexSpan = messageTemplate.Slice(i + 1, closeIndex - 1);

                        if (int.TryParse(indexSpan, out int argIndex) &&
                            argIndex >= 0 && argIndex < arguments.Length)
                        {
                            AppendArgument(sb, arguments[argIndex]);
                            i += closeIndex + 1;
                            continue;
                        }
                    }
                }

                sb.Append(c);
                i++;
            }

            return sb.ToString();
        }

        void AppendArgument(StringBuilder sb, object arg)
        {
            switch (arg)
            {
                case null:
                    sb.Append("null");
                    break;
                case DateTime dt:
                    sb.Append(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case string s:
                    if (int.TryParse(s, out int intVal))
                        sb.Append(intVal);
                    else if (double.TryParse(s, out double dblVal))
                        sb.Append(dblVal);
                    else
                        sb.Append(s);
                    break;
                default:
                    sb.Append(arg);
                    break;
            }
        }

        return ReplacePlaceholders(template.AsSpan(), args);
    }
}

public class Program
{
    public static void Main()
    {
        string result = Logger.FormatLogMessage(
            "User {0} logged in from {1} at {2}",
            "JohnDoe", "192.168.1.1", DateTime.Now);

        Console.WriteLine(result);
    }
}