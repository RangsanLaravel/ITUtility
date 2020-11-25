﻿using System;
using System.Runtime.Serialization;


namespace ITUtility
{
    [DataContract]
    public class ProcessResult
    {
        private bool successed;
        private string errorMessage;

        [DataMember]
        public bool Successed
        {
            get { return successed; }
            set { successed = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }

    public class ServiceException : Exception
    {
        private ProcessResult _processResult;
        public ProcessResult processResult { get { return _processResult; } }

        public ServiceException() : base() { }

        public ServiceException(string message) : base(message) { }

        public ServiceException(string message, ServiceException inner) : base(message, inner) { }

        public ServiceException(ProcessResult processresult) : base(processresult.ErrorMessage) { _processResult = processresult; }

        public ServiceException(ProcessResult processresult, ServiceException inner) : base(processresult.ErrorMessage, inner) { _processResult = processresult; }

        public ServiceException(string ErrorMessage, bool Successed) : base(ErrorMessage) { _processResult = new ProcessResult { Successed = Successed, ErrorMessage = ErrorMessage }; }

        public ServiceException(string ErrorMessage, bool Successed, ServiceException inner) : base(ErrorMessage, inner) { _processResult = new ProcessResult { Successed = Successed, ErrorMessage = ErrorMessage }; }
    }
}
