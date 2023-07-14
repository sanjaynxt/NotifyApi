namespace NotificationApi.Models
{
    public class EmailData
    {
        public EmailData()
        {
            //url = "https://dev-orderflow.foboh.com.au";
        }
        public string key { get; set; }
        public string type { get; set; }
        //public string url { get; set; }
        public string name { get; set; }
        public string To { get; set; }
        //public string From { get; set; }
        //public string Body { get; set; }

    }
    public class EmailResponse
    {
        public string key { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}
