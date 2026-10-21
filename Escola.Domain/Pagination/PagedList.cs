using System;
using System.Collections.Generic;
using System.Text;

namespace Escola.Domain.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set;}
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set;}

        public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
    }
}