namespace NotificationApi.Models
{
    public class EmailData
    {
        public string mailtype { get; set; }
        public string name { get; set; }
        public string To { get; set; }
       
    }
    public class EmailResponse
    {
        public string key { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}
