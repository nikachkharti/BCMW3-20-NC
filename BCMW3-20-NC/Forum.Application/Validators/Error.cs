namespace Forum.Application.Validators
{
    public static class Error
    {
        #region ERROR TITLES
        public static string NotFoundErrorTitle { get; } = "Resource not found.";
        public static string InputValidationErrorTitle { get; } = "One or more validation errors occurred.";
        public static string BadRequestErrorTitle { get; } = "Invalid request";
        public static string InternalServerErrorTitle { get; } = "Internal server error.";
        public static string UnauthorizedErrorTitle { get; } = "Unauthorized access.";
        public static string UnexpectedErrorTitle { get; } = "An unexpected error occurred.";
        #endregion

        #region ERROR TYPES
        public static string NotFoundErrorType { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
        public static string InputValidationErrorType { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
        public static string BadRequestErrorType { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
        public static string UnauthorizedErrorType { get; } = "https://tools.ietf.org/html/rfc9110#section-15.5.2";
        public static string InternalServerErrorType { get; } = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
        #endregion

        #region ERROR MESSAGES

        public static Dictionary<string, string[]> BuildErrorMessage(string errorTitle, string errorMessage) => new()
        {
            {errorTitle,[errorMessage] }
        };

        #endregion
    }
}
