namespace MacRobert.ApiKeys;

public class ApiService
    {
        public enum Environment
        {
            prod,
            stag,
            dev,
            test
        }

    protected const string CreationDateFormat = "yyyyMMdd";

    protected static string GetProtectedValue(Environment env, DateTime creationDate, string resource)
        {
            return $"{env}_{creationDate.ToString(CreationDateFormat)}_{resource}";
        }
    }
