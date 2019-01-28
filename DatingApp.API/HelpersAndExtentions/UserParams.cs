namespace DatingApp.API.HelpersAndExtentions
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int Pagenumber { get; set; } = 1;
        private int pageSize=10;
        public int PageSize
        {
            get { return pageSize;}
            set {pageSize = value >MaxPageSize?MaxPageSize:value;}
        }
        
        public string Gender { get; set; }
        public string UserId { get; set; }
        public int minAge { get; set; } =18;
        public int maxAge { get; set; }=99;
        public string name { get; set; }="all";

        public string orderBy { get; set; }


    }
}