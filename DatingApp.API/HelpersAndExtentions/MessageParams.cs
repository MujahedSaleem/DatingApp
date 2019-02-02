namespace DatingApp.API.HelpersAndExtentions
{
    public class MessageParams
    {
        private const int MaxPageSize = 50;
        public int Pagenumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string MessageContainer { get; set; }="Unread";
        public string UserId { get; set; }
    }
}