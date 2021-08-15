using Microsoft.AspNetCore.Components;
using Pagination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public partial class PaginationComponent<T>
    {
        #region Items
        [Parameter] public IEnumerable<T> Items { get; set; }
        #endregion

        #region list
        [Parameter] public string TableContainerCss { get; set; } = "table-container";
        [Parameter] public RenderFragment Head { get; set; }
        /// <summary>
        /// Only one with this property
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] 
        public Dictionary<string, object> AdditionalAttributes { get; set; }


        [Parameter] public RenderFragment<T> Body { get; set; }

        [Parameter] public RenderFragment Loading { get; set; }

        [Parameter] public RenderFragment Empty { get; set; }

        [Parameter] public int PageSize { get; set; } = 10;
        #endregion

        #region Paging footer
        [Parameter] public string ContainerCss { get; set; }
        [Parameter] public string ElementCss { get; set; }
        [Parameter] public string ActiceCss { get; set; }
        [Parameter] public int Columns { get; set; } = 1;
        #endregion

        PagedList<T> PagedItems;

        MarkupString DefaultHead;
        MarkupString DefaultBody;
        private readonly string DefaultCSSClass = "table table-responsive is-bordered is-striped is-hoverable is-fullwidth";

        protected override void OnParametersSet()
        {
            DrawList(1);
        }

        void ToPage(int page)
        {
            PagedItems = PagedList<T>.ToPagedList(Items, page, 10);
            DrawList(page);
        }

        void DrawList(int page)
        {
            if (Items != null)
            {
                PagedItems = PagedList<T>.ToPagedList(Items, page, PageSize);
                if (AdditionalAttributes == null)
                {
                    AdditionalAttributes = new Dictionary<string, object>();
                }
                if (!AdditionalAttributes.ContainsKey("class"))
                {
                    //check if have a table css class on the model send
                    DisplayTableAttribute cssClass = typeof(T).GetCustomAttribute<DisplayTableAttribute>();
                    if (cssClass != null && cssClass.TableClass != null) AdditionalAttributes.Add("class", cssClass.TableClass);
                    else AdditionalAttributes.Add("class", DefaultCSSClass);
                }

                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public |           //get public names
                                                                    BindingFlags.Instance);         //get instance names


                //get all my attributes
                DisplayTableAttribute[] attributes = new DisplayTableAttribute[properties.Length];

                StringBuilder html;
                if (Body == null)
                {
                    if (Head == null)
                    {
                        html = new StringBuilder();
                        for (int i = 0; i < Columns; i++)
                        {
                            attributes[i] = properties[i].GetCustomAttribute<DisplayTableAttribute>();                  //get if my custom attributes
                                                                                                                        //custom header class
                            string OpenTHTag = attributes[i] != null && attributes[i].HeaderClass != null ? $"<th class=\"{attributes[i].HeaderClass}\">" : "<th>";
                            //custom header name
                            Attribute alias = Attribute.GetCustomAttribute(properties[i], typeof(DisplayAttribute));     //get if have attribute display to change the name of the property                    
                            string header = attributes[i] != null && attributes[i].Header != null ? attributes[i].Header :      //custom header name
                                            alias == null ? properties[i].Name : ((DisplayAttribute)alias).GetName();         //if not get the display attribute or name
                            html.Append($"{OpenTHTag}{header}</th>");
                        }
                        DefaultHead = new MarkupString(html.ToString());
                    }

                    Columns = properties.Length;
                    html = new StringBuilder();
                    //get all the item to show the values
                    foreach (T item in PagedItems)
                    {
                        html.Append("<tr>");
                        //show the values
                        for (int i = 0; i < properties.Length; i++)
                        {
                            attributes[i] = properties[i].GetCustomAttribute<DisplayTableAttribute>();                  //get if my custom attributes
                            string OpenTDTag = attributes[i] != null && attributes[i].ColClass != null ? $"<td class=\"{attributes[i].ColClass}\">" : "<td>";
                            var value = attributes[i] != null && attributes[i].ValueFormat != null ? string.Format(attributes[i].ValueFormat, properties[i].GetValue(item)) : properties[i].GetValue(item);
                            html.Append($"{OpenTDTag}{value}</td>");
                        }
                        html.Append("</tr>");
                    }
                    DefaultBody = new MarkupString(html.ToString());
                }
            }
        }
    }
}
