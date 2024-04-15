namespace Common.Extensions
{
    public class ResultWithClientError
    {
        public object Result { get; set; }
        public string Error;
        public string StatusCode;
        public MemoryStream memoryStream;
        /// <summary>
        /// Return Error Status Code & Error Code.
        /// </summary>
        public ResultWithClientError(int status)
        {
            switch (status)
            {
                case 200:
                    this.StatusCode = "200";
                    break;
                case 204:
                    this.StatusCode = "204";
                    break;
                case 404:
                    this.Error = "NO_DATA_FOUND";
                    this.StatusCode = "400";
                    break;
                case 409:
                    this.Error = "DATA_EXIST";
                    this.StatusCode = "400";
                    break;
                case 400:
                    this.Error = "NO_DATA_FOUND";
                    this.StatusCode = "400";
                    break;
                default:
                    this.StatusCode = "";
                    break;
            }
        }


        /// <summary>
        /// Return Data.
        /// </summary>
        public ResultWithClientError(object data)
        {
            this.Result = data;
            this.StatusCode = "200";
        }
        public ResultWithClientError(MemoryStream data)
        {
            this.memoryStream = data;
            this.StatusCode = "200";
        }
        public ResultWithClientError(int status, object data)
        {
            this.Result = data;
            this.StatusCode = status.ToString();
        }
        /// <summary>
        /// Return Error Status Code.
        /// Return Error Code.
        /// </summary>
        public ResultWithClientError(string error)
        {
            this.Error = error;
            this.StatusCode = "400";
        }
    }
}
