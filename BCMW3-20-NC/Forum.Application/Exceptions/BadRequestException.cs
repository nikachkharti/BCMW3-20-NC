using Forum.Application.Validators;

namespace Forum.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public BadRequestException(IDictionary<string, string[]> errors) :
            base(GetFirstErrorMessage(errors))
        {
            Errors = errors;
        }

        private static string GetFirstErrorMessage(IDictionary<string, string[]> errors)
        {
            return errors.Values.SelectMany(v => v).FirstOrDefault() ?? Error.BadRequestErrorTitle;
        }
    }
}
