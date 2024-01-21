using System;
namespace OnlineStore.BLL.Exceptions
{
    public class DataExistsInDatabaseException : Exception
    {
        private const string ExceptionTextForObject = "Data already exists in database: ";
        private const string ExceptionText = "Data already exists in database.";

        public DataExistsInDatabaseException(string objectName)
        : base(ExceptionTextForObject + objectName)
        { }

        public DataExistsInDatabaseException()
        : base(ExceptionText)
        { }
    }
}
