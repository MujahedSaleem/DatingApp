namespace DatingApp.API.HelpersAndExtentions
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int totalPages, int itemPerPage, int totalItem)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.ItemPerPage = itemPerPage;
            this.TotalItem = totalItem;

        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalItem { get; set; }


    }
}