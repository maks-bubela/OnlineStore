using System;

namespace OnlineStore.BLL.Exceptions
{
    public class EntityArgumentNullException : ArgumentException
    {
        private const string ArgumentExeptionText = "This argument can`t be null :";

        public EntityArgumentNullException(string objectName)
        : base(ArgumentExeptionText + objectName)
        { }
    }
}
