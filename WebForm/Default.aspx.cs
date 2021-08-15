using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm
{
    public partial class _Default : Page
    {
        public PagedList<SampleClass> SampleList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int page;
            if (!int.TryParse(Request.QueryString["page"], out page)) page = 1;
            SampleList = PagedList<SampleClass>.ToPagedList(SampleData.GetData(), page, 10);
        }
    }


    class Helpers
    {
        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IEnumerable<DataRow> AsEnumerable(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }

        /// <summary>
        /// Convert DataTable to IEnumerable
        /// </summary>
        /// <param name="table">DataTable to Convert</param>
        /// <returns></returns>
        public static IQueryable<DataRow> AsQuerable(DataTable table)
        {
            return AsEnumerable(table).AsQueryable();
        }
    }
    public class PagedList<T> : List<T>
    {
        #region properties
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious { get { return CurrentPage > 1; } }
        public bool HasNext { get { return CurrentPage < TotalPages; } }
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
        #endregion

        #region HTML
        public static StringBuilder PagedListPager(PagedList<T> data)
        {
            return PagedListPager(data, string.Empty);
        }
        public static StringBuilder PagedListPager(PagedList<T> data, string page)
        {
            if (!string.IsNullOrEmpty(page))
            {
                if (page.IndexOf("?") < 0) page += "?";
                else page += "&";
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
                pagin.Append("<li class=\"PagedList-skipToFirst\"><a href=\"" + page + "page=1\"><<</a></li>");
                pagin.Append("<li class=\"PagedList-skipToPrevious\"><a href=\"" + page + "page=" + (data.CurrentPage - 1) + "\"><</a></li>");
                pagin.Append("<li class=\"disabled PagedList-ellipses\"><a>...</a></li>");
            }
            else if (data.CurrentPage == 1) pagin.Append(string.Empty);
            else pagin.Append("<li class=\"PagedList-skipToNexts\"><a href=\"" + page + "page=" + (data.CurrentPage - 1) + "\"><</a></li>");

            int index = 1;
            do
            {
                if (i <= data.TotalPages)
                {
                    if (i == data.CurrentPage) pagin.Append("<li class=\"active page-number\" data-index=\"" + index + "\"><span>" + i + "</span></li>");
                    else pagin.Append("<li class=\"page-number\" data-index=\"" + (i == data.TotalPages || c == 6 ? 0 : index) + "\"><a href=\"" + page + "page=" + i + "\">" + i + "</a></li>");
                }
                else c = 7;
                i++;
                c++;
                index++;
            } while (c < 7 && i <= data.TotalPages);

            i--;
            if (data.CurrentPage < data.TotalPages - 4)
            {
                pagin.Append("<li class=\"disabled PagedList-ellipses\"><a>...</a></li>");
                pagin.Append("<li class=\"PagedList-skipToNext\"><a href=\"" + page + "page=" + (data.CurrentPage + 1) + "\">></a></li>");
                pagin.Append("<li class=\"PagedList-skipToLast\"><a href=\"" + page + "page=" + data.TotalPages + "\">>></a></li>");
            }
            else if (data.CurrentPage == data.TotalPages) pagin.Append(string.Empty);
            else pagin.Append("<li class=\"PagedList-skipToNext\"><a href=\"" + page + "page=" + (data.CurrentPage + 1) + "\">></a></li>");
            pagin.Append("</ul></div>");
            return pagin;
        }
        #endregion
    }
    public class SampleClass
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }
    public class SampleData
    {
        public List<SampleClass> SampleList { get; set; }

        public SampleData()
        {
            SampleList = new List<SampleClass>();
            for (int i = 0; i < 1000; i++)
            {
                SampleList.Add(new SampleClass()
                {
                    Id = i,
                    Created = new DateTime(new Random().Next(1976, DateTime.Today.Year), new Random().Next(1, 12), new Random().Next(1, 28)),
                    Name = $"Name {i}",
                    Description = $"Description for name {i}",
                    Price = i + (new Random().Next(1, 10000) / 100.0m),
                    Qty = i + (new Random().Next(1, 10) - (i > 5 ? i - 2 : i - 1))
                });
            }
        }

        public static List<SampleClass> GetData()
        {
            SampleData data = new SampleData();
            return data.SampleList;

        }
    }
}
