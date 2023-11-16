using System;

namespace ODataSample.Services
{
    public class DataValidationException : Exception
    {

        public DataValidationException()
        {

        }

        public DataValidationException(string sMessage) : base(sMessage)
        {

        }
      


    }
}
