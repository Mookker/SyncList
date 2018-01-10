namespace SyncList.CommonLibrary.Exceptions
{
    public class InputDataValidationException : SyncListBaseException
    {
        public InputDataValidationException(string message) : base(message)
        {
            
        }

        public InputDataValidationException() : base("Invalid parameters passed")
        {
            
        }
    }
}