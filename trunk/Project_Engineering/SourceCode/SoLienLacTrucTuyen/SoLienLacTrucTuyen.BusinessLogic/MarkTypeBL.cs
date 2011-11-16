using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MarkTypeBL: BaseBL
    {
        MarkTypeDA markTypeDA;

        public MarkTypeBL(School school)
            : base(school)
        {
            markTypeDA = new MarkTypeDA(school);
        }

        public void InsertLoaiDiem(string markTypeName, double markRatio, short maxMarksPerTerm, bool calAverageMark)
        {
            DanhMuc_LoaiDiem markType = new DanhMuc_LoaiDiem
            {
                TenLoaiDiem = markTypeName,
                HeSoDiem = markRatio,
                SoCotToiDa = maxMarksPerTerm,
                TinhDTB = calAverageMark
            };

            markTypeDA.InsertMarkType(markType);
        }

        public void DeleteMarkType(DanhMuc_LoaiDiem markType)
        {
            markTypeDA.DeleteMarkType(markType);
        }

        public void UpdateMarkType(string editedMarkTypeName, string newMarkTypeName, double newMarkRatio, short maxMarksPerTerm, bool calAverageMark)
        {
            DanhMuc_LoaiDiem markType = GetMarkType(editedMarkTypeName);

            markType.TenLoaiDiem = newMarkTypeName;
            markType.HeSoDiem = newMarkRatio;
            markType.SoCotToiDa = maxMarksPerTerm;
            markType.TinhDTB = calAverageMark;

            markTypeDA.UpdateMarkType(markType);
        }

        public DanhMuc_LoaiDiem GetMarkType(string markTypeName)
        {
            return markTypeDA.GetMarkType(markTypeName);
        }

        public DanhMuc_LoaiDiem GetMarkType(int markTypeId)
        {
            return markTypeDA.GetMarkType(markTypeId);
        }

        public List<DanhMuc_LoaiDiem> GetListMarkTypes()
        {
            return markTypeDA.GetListMarkTypes();
        }

        public List<DanhMuc_LoaiDiem> GetListMarkTypes(string markTypeName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_LoaiDiem> lMarkTypes = new List<DanhMuc_LoaiDiem>();

            if (String.Compare(markTypeName, "tất cả", true) == 0 || markTypeName == "")
            {
                lMarkTypes = markTypeDA.GetListMarkTypes(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                DanhMuc_LoaiDiem markType = GetMarkType(markTypeName);
                lMarkTypes.Add(markType);
                totalRecords = 1;
            }

            return lMarkTypes;
        }

        public bool MarkTypeNameExists(string markTypeName)
        {
            return markTypeDA.MarkTypeExists(markTypeName);
        }

        public bool MarkTypeNameExists(string oldMarkTypeName, string newMarkTypeName)
        {
            bool bExist = false;

            if (oldMarkTypeName == newMarkTypeName)
            {
                bExist = false;
            }
            else
            {
                bExist = markTypeDA.MarkTypeExists(newMarkTypeName);
            }

            return bExist;
        }

        public bool IsDeletable(string markTypeName)
        {
            DanhMuc_LoaiDiem markType = GetMarkType(markTypeName);
            return markTypeDA.IsDeletable(markType);
        }

        public DanhMuc_LoaiDiem GetAppliedCalAvgMarkType()
        {
            return markTypeDA.GetAppliedCalAvgMarkType();
        }
    }
}
