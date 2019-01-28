using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.HelpersAndExtentions
{
    public class PageList<T> : List<T>
    {
        public PageList(List<T> items, int currentPage, int pageSize, int totalCount)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages =(int) Math.Ceiling(totalCount/(double)pageSize);
            this.AddRange(items);
            

        }
        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source,int CurrentPage,int PageSize) {
            int count = await source.CountAsync();
            var item =await source.Skip(((CurrentPage-1)*PageSize)).Take(PageSize).ToListAsync();
            return new PageList<T>(item,CurrentPage,PageSize,count);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }


    }
}