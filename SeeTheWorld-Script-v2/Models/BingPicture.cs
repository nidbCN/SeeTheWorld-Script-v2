namespace SeeTheWorld_Script_v2.Models
{
    public class BingDeSerializeModel
    {
        public BingPicture[] Images { get; set; }
    }

    public class BingPicture
    {
        public string StartDate { get; set; }
        public string Url { get; set; }
        public string Copyright { get; set; }
    }
}
