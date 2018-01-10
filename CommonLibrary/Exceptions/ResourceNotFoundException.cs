namespace SyncList.CommonLibrary.Exceptions
{
    public class ResourceNotFoundException : InputDataValidationException
    {
        public ResourceNotFoundException() : base("Resource not found")
        {
            
        }
    }
}