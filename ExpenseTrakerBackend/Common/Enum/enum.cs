namespace PracticeCrud.Common.Enum
{
    public class @enum
    {
    }

    public static class LoginStatus
    {
        public const int EmailNotExist = -404;
        public const int UserDeactive = -2;
        public const int UserDeleted = -3;
    }

    public static class Status
    {
        public const int Failed = -1;
        public const int Success = 0;
        public const int InUse = -401;
        public const int URLExpired = -3;
        public const int URLUsed = -4;
        public const int IsOnLoadCheck = -5;
    }
    public enum SaveResult
    {
        DataExists = 0,
        Inserted = 1,
        Updated = 2,
        SqlException = -1,
        DurationValidation = 3
    }
}
