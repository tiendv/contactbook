using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SoLienLacTrucTuyen_WebRole
{
    public class SortablePaginationRepeater: Repeater
    {
        public enum PageLocationEnum
        {
            Top,
            Bottom,
            Both
        }
        private PageLocationEnum pageLocation;
        public PageLocationEnum PageLocation
        {
            get
            {
                if (ViewState["PageLocation"] == null)
                {
                    return pageLocation;
                }
                else
                {
                    return (PageLocationEnum)ViewState["PageLocation"];
                }
            }

            set
            {
                ViewState["PageLocation"] = value;
            }
        }

        public enum PagerStyleEnum
        {
            Dropdown, 
            NumericPages, 
            NextPrev, 
            TextBox
        }
        private PagerStyleEnum pagerStyle;
        public PagerStyleEnum PagerStyle
        {
            get
            {
                if (ViewState["PagerStyle"] == null)
                {
                    return pagerStyle;
                }
                else
                {
                    return (PagerStyleEnum)ViewState["PagerStyle"];
                }
            }

            set
            {
                ViewState["PagerStyle"] = value;
            }
        }
        
        private int pageSize;
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null)
                {
                    return pageSize;
                }
                else
                {
                    return (int)ViewState["PageSize"];
                }
            }

            set
            {
                ViewState["PageSize"] = value;
            }
        }

        private int pageButtonCount;
        public int PageButtonCount
        {
            get
            {
                if (ViewState["PageButtonCount"] == null)
                {
                    return pageButtonCount;
                }
                else
                {
                    return (int)ViewState["PageButtonCount"];
                }
            }

            set
            {
                ViewState["PageButtonCount"] = value;
            }
        }
                
        private int currentPageIndex;
        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["CurrentPageIndex"] == null)
                {
                    return currentPageIndex;
                }
                else
                {
                    return (int)ViewState["CurrentPageIndex"];
                }
            }

            set
            {
                ViewState["CurrentPageIndex"] = value;
            }
        }

        private string sortBy;
        public string SortBy
        {
            get
            {
                if (ViewState["SortBy"] == null)
                {
                    return sortBy;
                }
                else
                {
                    return (string)ViewState["SortBy"];
                }
            }

            set
            {
                ViewState["SortBy"] = value;
            }
        }

        private string tableWidth;
        public string TableWidth
        {
            get
            {
                if (ViewState["TableWidth"] == null)
                {
                    return tableWidth;
                }
                else
                {
                    return (string)ViewState["TableWidth"];
                }
            }

            set
            {
                ViewState["TableWidth"] = value;
            }
        }

        private string goButtonCssClass;
        public string GoButtonCssClass
        {
            get
            {
                if (ViewState["GoButtonCssClass"] == null)
                {
                    return goButtonCssClass;
                }
                else
                {
                    return (string)ViewState["GoButtonCssClass"];
                }
            }

            set
            {
                ViewState["GoButtonCssClass"] = value;
            }
        }

        private string sortColumn;
        public string SortColumn
        {
            get
            {
                if (ViewState["SortColumn"] == null)
                {
                    return sortColumn;
                }
                else
                {
                    return (string)ViewState["SortColumn"];
                }
            }

            set
            {
                ViewState["SortColumn"] = value;
            }
        }

        private PagedDataSource pagedDataSource;

        public override void DataBind()
        {
            base.DataBind();

            if (DataSource.GetType() == typeof(System.Collections.IEnumerable))
            {
                pagedDataSource.DataSource = (System.Collections.IEnumerable)DataSource;
            }
            else
            {
                if (DataSource.GetType() == typeof(System.Data.DataView))
                {
                    System.Data.DataView data = (System.Data.DataView)DataSource;
                    pagedDataSource.DataSource = data.Table.Rows;
                }
                else
                {
                    if (DataSource.GetType() == typeof(System.Data.DataTable))
                    {
                        System.Data.DataTable data = (System.Data.DataTable)DataSource;
                        pagedDataSource.DataSource = data.DefaultView;
                    }
                    else
                    {
                        if (DataSource.GetType() == typeof(System.Data.DataSet))
                        {
                            System.Data.DataSet data = (System.Data.DataSet)DataSource;
                            if (DataMember != string.Empty && data.Tables.Contains(DataMember))
                            {
                                pagedDataSource.DataSource = data.Tables[DataMember].DefaultView;
                            }
                            else
                            {
                                if (data.Tables.Count > 0)
                                {
                                    pagedDataSource.DataSource = data.Tables[0].DefaultView;
                                }
                                else
                                {
                                    throw new SortablePaginationException("DataSet doesn't have any tables.");
                                }
                            }
                        }
                        else
                        {
                            throw new SortablePaginationException("DataSource must be of type "
                                + "System.Collections.IEnumerable.  The DataSource you provided is of type "
                                + DataSource.GetType().FullName);
                        }
                    }
                }
            }

            // Set the page size as provided by the consumer
            pagedDataSource.PageSize = this.PageSize;

            // Insure that the page doesn't exceed the maximum number of pages available
            if(this.currentPageIndex >= pagedDataSource.PageCount)
            {
                this.currentPageIndex = pagedDataSource.PageCount - 1;
            }

            pagedDataSource.CurrentPageIndex = this.currentPageIndex;
            base.DataSource = pagedDataSource;
            base.DataBind();
        }
    }

    public class SortablePaginationException : Exception
    {
        public SortablePaginationException(string msg)
        {
            
        }
    }
}