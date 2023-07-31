using System.Collections.Generic;

namespace IdentityApi.Models.ResponseModels
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            IsSuccess = false;
            Message = string.Empty;
            Response = null;
            Errors = null;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int TotalRecords { get; set; }
        public object Response { get; set; }
        public IList<string> Errors { get; set; }

        public int Status
        {
            get
            {
                return IsSuccess ? 200 : 400;
            }
        }
    }
}
