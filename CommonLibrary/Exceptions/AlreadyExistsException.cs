namespace SyncList.CommonLibrary.Exceptions
{
    public class AlreadyExistsException : InputDataValidationException
    {
        public AlreadyExistsException() : base("Already exists")
        {
            
        }
    }
}