namespace SeeTheWorld_Script_v2
{
    public class Model
    {
        public Image[] Images { get; set; }
    }

    public class Image
    {
        public string StartDate { get; set; }
        public string FullStartDate { get; set; }
        public string EndDate { get; set; }
        public string Url { get; set; }
        public string UrlBase { get; set; }
        public string Copyright { get; set; }
        public string CopyrightLink { get; set; }
        public string Title { get; set; }
        public string Quiz { get; set; }
        public bool Wp { get; set; }
        public string Hsh { get; set; }
        public int Drk { get; set; }
        public int Top { get; set; }
        public int Bot { get; set; }
        public object[] Hs { get; set; }
    }
}
