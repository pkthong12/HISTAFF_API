namespace Common.Extensions
{
    public class ResultWithError
    {
        public object Data { get; set; }
        public object InnerBody { get; set; }
        public string Error;
        public string StatusCode;
        public MemoryStream memoryStream;
        /// <summary>
        /// Return Error Status Code & Error Code.
        /// </summary>
        public ResultWithError(int status)
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
        public ResultWithError(object data)
        {
            this.InnerBody = data;
            this.Data = data;
            this.StatusCode = "200";
        }
        public ResultWithError(MemoryStream data)
        {
            this.memoryStream = data;
            this.StatusCode = "200";
        }
        public ResultWithError(int status, object data)
        {
            this.InnerBody = data;
            this.Data = data;
            this.StatusCode = status.ToString();
        }
        /// <summary>
        /// Return Error Status Code.
        /// Return Error Code.
        /// </summary>
        public ResultWithError(string error)
        {
            this.Error = error;
            this.StatusCode = "400";
        }
    }
}
