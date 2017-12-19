using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.DataLayer.TransferObjects
{
    public class RepositoryResponse<T> where T : class
    {
        public T Value { get; set; }

        public Exception Exception { get; set; }
        public RepositoryResponseMessageEnum RepositoryResponseMessage { get; set; }
        private bool _successful = true;
        public bool Successful
        {
            set
            {
                _successful = value;
            }
            get
            {
                return _successful && Exception == null;
            }
        }

        public enum RepositoryResponseMessageEnum
        {
            None,
            NotFound
        }
    }



}
