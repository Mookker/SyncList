using System;

namespace SyncList.CommonLibrary.Exceptions
{
    public abstract class SyncListBaseException : Exception
    {
        protected SyncListBaseException(string message) : base(message)
        {
            
        }

        protected SyncListBaseException() : base()
        {
            
        }
    }
}