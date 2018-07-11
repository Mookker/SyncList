using System;
using System.IO;
using SyncList.CommonLibrary.Exceptions;

namespace SyncList.CommonLibrary.Validation
{
    public static class Validator
    {
        public static void Assert(bool condition, ValidationAreas area)
        {
            if(condition)
                return;

            switch (area)
            {
                case ValidationAreas.InputParameters:
                    throw new InputDataValidationException();
                case ValidationAreas.NotExists:
                    throw new ResourceNotFoundException();
                case ValidationAreas.AlreadyExists:
                    throw new AlreadyExistsException();
            }
        }
    }
}