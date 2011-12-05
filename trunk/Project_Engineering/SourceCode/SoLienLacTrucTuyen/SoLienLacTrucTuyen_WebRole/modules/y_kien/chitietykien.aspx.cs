using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DetailedParentsCommentPage : BaseContentPage
    {
        #region Fields
        private ParentsCommentBL parentsCommentBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                // User can not access this page
                return;
            }

            parentsCommentBL = new ParentsCommentBL(UserSchool);
            if (!Page.IsPostBack)
            {
                int commentId = Int32.Parse(Request.QueryString[AppConstant.QUERY_PARENTSCOMMENT]);
                FillDetailedParentsComment(commentId);
            }
        }

        private void FillDetailedParentsComment(int commentId)
        {
            ParentComment_Comment parentComment = parentsCommentBL.GetParentsComments(commentId);
            if (parentComment != null)
            {
                LblTitle.Text = parentComment.Title;
                LblContent.Text = parentComment.CommentContent;
                TxtReply.Text = parentComment.Feedback;
            }
        }
        #endregion

        #region Methods
        
        #endregion
        
        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                //if (lopHoc != null)
                //{
                //    int ClassId = lopHoc.ClassId;
                //    if (!lopHocBL.CheckCanDeleteLopHoc(ClassId))
                //    {
                //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                //        btnDeleteItem.Enabled = false;
                //    }
                //}
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDetailItem":
                    {
                        //int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        //Class_Class lophoc = lopHocBL.GetLopHoc(ClassId);

                        //LblClassNameChiTiet.Text = lophoc.ClassName;
                        //LblFacultyNameChiTiet.Text = (new facultyBL(UserSchool)).GetNganhHoc(lophoc.FacultyId).FacultyName;
                        //LblGradeNameChiTiet.Text = (new grades(UserSchool)).GetKhoiLop(lophoc.GradeId).GradeName;
                        //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                        //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        //mPEDetail.Show();

                        //this.HdfClassId.Value = ClassId.ToString();
                        //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnReply_Click(object sender, ImageClickEventArgs e)
        {
            ParentComment_Comment parentsComment = new ParentComment_Comment();
            parentsComment.CommentId = Int32.Parse(Request.QueryString[AppConstant.QUERY_PARENTSCOMMENT]);

            string strReply = TxtReply.Text;
            parentsCommentBL.Reply(parentsComment, strReply);

            Response.Redirect(AppConstant.PAGEPATH_PARENTSCOMMENTS);
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_PARENTSCOMMENTS);
        }

        #endregion
    }
}