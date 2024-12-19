namespace BillCollector.Infrastructure
{
    public class AppKeys
    {
        public CbaSetting Cba { get; set; }
        public double LoginTokenExpiryInMinutes { get; set; }
    }


    public class CbaSetting
    {
        public string BaseUrl { get; set; }
    }

}
