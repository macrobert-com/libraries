using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MacRobert.Util;

/// <summary>
/// Interpolate on a string using {objectName} syntax. Objects must implement ToString.
/// </summary>
public partial class InterpolatedString
{
    public InterpolatedString(string template)
    {
        Template = template;
    }

    public string Template { get; }

    private record CacheKey(Type Type, string Template);

    public string Interpolate<T>(T data, bool strict = true)
    {
        var key = new CacheKey(typeof(T), Template);
        if (!Cache.TryGetValue(key, out var del))
        {
            del = CreateFunc<T>(Template, strict);
            Cache.TryAdd(key, del);
        }

        return ((Func<T, string>)del)(data);
    }

    private static readonly ConcurrentDictionary<CacheKey, Delegate> Cache = new();

    private static Func<T, string> CreateFunc<T>(string template, bool strict)
    {
        var matches = InterpolationToken().Matches(template);

        var parameter = Expression.Parameter(typeof(T), "obj");
        var expressionList = new List<Expression>();

        int start = 0;
        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Index > start)
            {
                expressionList.Add(Expression.Constant(template[start..match.Index]));
            }

            var property = typeof(T).GetProperty(match.Groups[1].Value);
            Expression propValue;
            if (property == null)
            {
                if (strict)
                {
                    throw new ArgumentException($"Property '{match.Groups[1].Value}' does not exist on type '{typeof(T)}'.");
                }
                else
                {
                    propValue = Expression.Constant(string.Empty);
                }
            }
            else
            {
                var prop = Expression.Property(parameter, match.Groups[1].Value);
                var toStringMethod = prop.Type.GetMethod("ToString", Type.EmptyTypes);
                propValue = Expression.Call(prop, toStringMethod ?? throw new InvalidOperationException());
            }

            expressionList.Add(propValue);

            start = match.Index + match.Length;
        }

        if (start < template.Length)
        {
            expressionList.Add(Expression.Constant(template[start..]));
        }

        var concatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string[]) });
        var toArrayCall = Expression.NewArrayInit(typeof(string), expressionList);
        var body = Expression.Call(concatMethod, toArrayCall);

        return Expression.Lambda<Func<T, string>>(body, parameter).Compile();
    }

    [GeneratedRegex("\\{(\\w+)\\}")]
    private static partial Regex InterpolationToken();
}
