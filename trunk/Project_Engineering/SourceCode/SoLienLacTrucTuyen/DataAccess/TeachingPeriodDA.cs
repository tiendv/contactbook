using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class TeachingPeriodDA : BaseDA
    {
        public TeachingPeriodDA(School school)
            : base(school)
        {
        }

        public void DeleteTeachingPeriod(DanhMuc_Tiet deleteTeachingPeriod)
        {
            DanhMuc_Tiet teachingPeriod = null;

            IQueryable<DanhMuc_Tiet> iTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                       where tchPeriod.MaTiet == teachingPeriod.MaTiet
                                                       && tchPeriod.SchoolId == school.SchoolId
                                                       select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
                db.DanhMuc_Tiets.DeleteOnSubmit(teachingPeriod);
                db.SubmitChanges();
            }
        }

        public void InsertTeachingPeriod(DanhMuc_Tiet newTeachingPeriod)
        {
            newTeachingPeriod.SchoolId = school.SchoolId;
            db.DanhMuc_Tiets.InsertOnSubmit(newTeachingPeriod);
            db.SubmitChanges();
        }

        public void UpdateTeachingPeriod(DanhMuc_Tiet editedTeachingPeriod)
        {
            DanhMuc_Tiet teachingPeriod = null;

            IQueryable<DanhMuc_Tiet> iTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                       where tchPeriod.MaTiet == editedTeachingPeriod.MaTiet
                                                       && tchPeriod.SchoolId == school.SchoolId
                                                       select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
                teachingPeriod.TenTiet = editedTeachingPeriod.TenTiet;
                teachingPeriod.MaBuoi = editedTeachingPeriod.MaBuoi;
                teachingPeriod.ThuTu = editedTeachingPeriod.ThuTu;
                teachingPeriod.ThoiGianBatDau = editedTeachingPeriod.ThoiGianBatDau;
                teachingPeriod.ThoiDiemKetThu = editedTeachingPeriod.ThoiDiemKetThu;

                db.SubmitChanges();
            }
        }

        public DanhMuc_Tiet GetTeachingPeriod(int teachingPeriodId)
        {
            DanhMuc_Tiet teachingPeriod = null;

            IQueryable<DanhMuc_Tiet> iTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                       where tchPeriod.MaTiet == teachingPeriodId
                                                       && tchPeriod.SchoolId == school.SchoolId
                                                       select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                teachingPeriod = iTeachingPeriod.First();
            }

            return teachingPeriod;
        }

        public List<DanhMuc_Tiet> GetTeachingPeriods()
        {
            List<DanhMuc_Tiet> lTeachingPeriods = new List<DanhMuc_Tiet>();

            IQueryable<DanhMuc_Tiet> iTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                       where tchPeriod.SchoolId == school.SchoolId
                                                       select tchPeriod;
            if (iTeachingPeriod.Count() != 0)
            {
                lTeachingPeriods = iTeachingPeriod.OrderBy(tchPeriod => tchPeriod.ThoiGianBatDau).ThenBy(tchPeriod => tchPeriod.ThuTu).ToList();
            }

            return lTeachingPeriods;
        }

        public List<DanhMuc_Tiet> GetTeachingPeriods(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_Tiet> iqTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                        where tchPeriod.SchoolId == school.SchoolId
                                                        select tchPeriod;
            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_Tiet> GetTeachingPeriods(string teachingPeriodName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_Tiet> iqTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                        where tchPeriod.TenTiet == teachingPeriodName
                                                        && tchPeriod.SchoolId == school.SchoolId
                                                        select tchPeriod;
            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_Tiet> GetTeachingPeriods(CauHinh_Buoi session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_Tiet> iqTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                        where tchPeriod.CauHinh_Buoi.MaBuoi == session.MaBuoi
                                                        && tchPeriod.SchoolId == school.SchoolId
                                                        select tchPeriod;
            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public List<DanhMuc_Tiet> GetTeachingPeriods(string teachingPeriodName, CauHinh_Buoi session, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            IQueryable<DanhMuc_Tiet> iqTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                        where tchPeriod.TenTiet == teachingPeriodName
                                                            && tchPeriod.MaBuoi == session.MaBuoi
                                                            && tchPeriod.SchoolId == school.SchoolId

                                                        select tchPeriod;
            return GetTeachingPeriods(ref iqTeachingPeriod, pageCurrentIndex, pageSize, out totalRecords);
        }

        public bool IsDeletable(DanhMuc_Tiet teachingPeriod)
        {
            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schedule in db.LopHoc_MonHocTKBs
                                                      where schedule.MaTiet == teachingPeriod.MaTiet
                                                      && schedule.aspnet_User.aspnet_Membership.SchoolId == school.SchoolId
                                                      select schedule;
            if (iqSchedule.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool TeachingPeriodNameExists(string teachingPeriodName)
        {
            IQueryable<DanhMuc_Tiet> iqTeachingPeriod = from tchPeriod in db.DanhMuc_Tiets
                                                        where tchPeriod.TenTiet == teachingPeriodName
                                                        && tchPeriod.SchoolId == school.SchoolId
                                                        select tchPeriod;
            if (iqTeachingPeriod.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<DanhMuc_Tiet> GetTeachingPeriods(ref IQueryable<DanhMuc_Tiet> iqTeachingPeriod, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<DanhMuc_Tiet> lTeachingPeriods = new List<DanhMuc_Tiet>();

            totalRecords = iqTeachingPeriod.Count();
            if (totalRecords != 0)
            {
                lTeachingPeriods = iqTeachingPeriod.OrderBy(tchPeriod => tchPeriod.ThoiGianBatDau)
                    .ThenBy(tiet => tiet.ThuTu).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lTeachingPeriods;
        }
    }
}
