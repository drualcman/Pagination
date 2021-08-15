using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms.Pagination
{
    public class PagedList<T> : List<T>
    {
        #region properties
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        #endregion

        #region constructor
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }
        #endregion

        #region Methods
        public static PagedList<DataRow> ToPagedList(DataTable data, int pageNumber, int pageSize)
        {
            IQueryable<DataRow> source = Helpers.AsQuerable(data);
            int count = source.Count();
            List<DataRow> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<DataRow>(items, count, pageNumber, pageSize);
        }

        public static PagedList<DataRow> ToPagedList(DataView data, int pageNumber, int pageSize)
        {
            IQueryable<DataRow> source = Helpers.AsQuerable(data.ToTable());
            int count = source.Count();
            List<DataRow> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<DataRow>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }


        public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
        #endregion

        #region HTML
        public static StringBuilder PagedListPager(PagedList<T> data)
        {
            return PagedListPager(data, string.Empty);
        }
        public static StringBuilder PagedListPager(PagedList<T> data,  string page)
        {
            if (!string.IsNullOrEmpty(page))
            {
                if (page.IndexOf("?") < 0) page += "?";
                else
                {
                    string lastCharacter = page.Substring(page.Length - 1, 1);
                    if (lastCharacter != "&") page += "&";
                }
            }
            else page = "?";

            StringBuilder pagin = new StringBuilder();
            pagin.Append("<div class=\"pagination-container\"><ul class=\"pagination\">");
            int i;
            if (data.CurrentPage > 5) i = data.CurrentPage - 5;
            else i = 1;
            int c = 0;
            if (data.CurrentPage > 6)
            {
                pagin.Append($"<li class=\"PagedList-skipToFirst\"><a href=\"{page}page=1\"><<</a></li>");
                pagin.Append($"<li class=\"PagedList-skipToPrevious\"><a href=\"{page}page={data.CurrentPage - 1}\"><</a></li>");
                pagin.Append($"<li class=\"disabled PagedList-ellipses\"><a>...</a></li>");
            }
            else if (data.CurrentPage == 1) pagin.Append(string.Empty); 
            else pagin.Append($"<li class=\"PagedList-skipToNexts\"><a href=\"{page}page={data.CurrentPage - 1}\"><</a></li>");

            do
            {
                if (i <= data.TotalPages)
                {
                    if (i == data.CurrentPage) pagin.Append($"<li class=\"active\"><span>{i}</span></li>");
                    else pagin.Append($"<li><a href=\"{page}page={i}\">{i}</a></li>");
                }
                else c = 10;
                i++;
                c++;
            } while (c < 10 && i <= data.TotalPages);
            i--;
            if (data.CurrentPage < data.TotalPages - 4)
            {
                pagin.Append($"<li class=\"disabled PagedList-ellipses\"><a>...</a></li>");
                pagin.Append($"<li class=\"PagedList-skipToNext\"><a href=\"{page}page={data.CurrentPage + 1}\">></a></li>");
                pagin.Append($"<li class=\"PagedList-skipToLast\"><a href=\"{page}page={data.TotalPages}\">>></a></li>");
            }
            else if (data.CurrentPage == data.TotalPages) pagin.Append(string.Empty);
            else pagin.Append($"<li class=\"PagedList-skipToNext\"><a href=\"{page}page={data.CurrentPage + 1}\">></a></li>");
            pagin.Append("</ul></div>");
            return pagin;
        }
        #endregion
    }
}
