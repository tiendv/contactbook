using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class AttitudeDA : BaseDA
    {
        public AttitudeDA(School school)
            : base(school)
        {

        }

        public void InsertAttitude(DanhMuc_ThaiDoThamGia attitude)
        {
            db.DanhMuc_ThaiDoThamGias.InsertOnSubmit(attitude);
            db.SubmitChanges();
        }

        public void UpdateAttitude(DanhMuc_ThaiDoThamGia editedAttitude)
        {
            DanhMuc_ThaiDoThamGia attitude = null;
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           where att.MaThaiDoThamGia == editedAttitude.MaThaiDoThamGia
                                                           select att;
            if (iqAttitude.Count() != null)
            {
                attitude = iqAttitude.First();
                attitude.TenThaiDoThamGia = editedAttitude.TenThaiDoThamGia;
                db.SubmitChanges();
            }
        }

        public void DeleteAttitude(DanhMuc_ThaiDoThamGia deletedAttitude)
        {
            DanhMuc_ThaiDoThamGia attitude = null;
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           where att.MaThaiDoThamGia == deletedAttitude.MaThaiDoThamGia
                                                           select att;
            if (iqAttitude.Count() != null)
            {
                attitude = iqAttitude.First();
                db.DanhMuc_ThaiDoThamGias.DeleteOnSubmit(attitude);
                db.SubmitChanges();
            }
        }

        public DanhMuc_ThaiDoThamGia GetAttitude(int attitudeId)
        {
            DanhMuc_ThaiDoThamGia attitude = null;
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           where att.MaThaiDoThamGia == attitudeId
                                                           select att;
            if (iqAttitude.Count() != null)
            {
                attitude = iqAttitude.First();
            }

            return attitude;
        }        

        public List<DanhMuc_ThaiDoThamGia> GetListAttitudes()
        {
            List<DanhMuc_ThaiDoThamGia> lAttitudes = new List<DanhMuc_ThaiDoThamGia>();

            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           select att;
            if (iqAttitude.Count() != null)
            {
                lAttitudes = iqAttitude.OrderBy(att => att.TenThaiDoThamGia).ToList();
            }

            return lAttitudes;
        }        

        public List<DanhMuc_ThaiDoThamGia> GetListAttitudes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           select att;
            return GetListAttitudes(ref iqAttitude, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_ThaiDoThamGia> GetListAttitudes(string attitudeName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           where att.TenThaiDoThamGia == attitudeName
                                                           select att;
            return GetListAttitudes(ref iqAttitude, pageCurrentIndex, pageSize, out totalRecords);
        }

        public bool IsDeletable(DanhMuc_ThaiDoThamGia attitude)
        {
            IQueryable<HocSinh_HoatDong> iqStudentActivity = from studAct in db.HocSinh_HoatDongs
                                                             where studAct.MaThaiDoThamGia == attitude.MaThaiDoThamGia
                                                             select studAct;
            if (iqStudentActivity.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AttitudeNameExists(string attitudeName)
        {
            IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude = from att in db.DanhMuc_ThaiDoThamGias
                                                           where att.TenThaiDoThamGia == attitudeName
                                                           select att;
            if (iqAttitude.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<DanhMuc_ThaiDoThamGia> GetListAttitudes(ref IQueryable<DanhMuc_ThaiDoThamGia> iqAttitude, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_ThaiDoThamGia> lAttitudes = new List<DanhMuc_ThaiDoThamGia>();
            totalRecords = iqAttitude.Count();
            if (iqAttitude.Count() != 0)
            {
                lAttitudes = iqAttitude.OrderBy(att => att.TenThaiDoThamGia)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lAttitudes;
        }
    }
}
