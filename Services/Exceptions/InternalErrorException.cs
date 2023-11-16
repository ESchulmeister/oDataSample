using System;

namespace ODataSample.Services
{
    public class InternalErrorException :Exception
    {

        public InternalErrorException()
        {

        }

        public InternalErrorException(string sMessage) : base(sMessage)
        {

        }
        //public InternalErrorException(Exception oException) : base(oException.Message, oException)
        //{

        //}


    }
}
