using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class GiaoVienChuNhiemBL
    {
        private GiaoVienChuNhiemDA gvcnDA;

        public GiaoVienChuNhiemBL()
        {
            gvcnDA = new GiaoVienChuNhiemDA();
        }

        #region Insert, Update, Delete
        public void Insert(int maLopHoc, int maGiaoVien)
        {
            gvcnDA.Insert(maLopHoc, maGiaoVien);
        }

        public void Update(int maGVCN, int maGiaoVien)
        {
            gvcnDA.Update(maGVCN, maGiaoVien);
        }

        public void Delete(int maGVCN)
        {
            gvcnDA.Delete(maGVCN);
        }
        #endregion

        public LopHoc_GVCN GetGiaoVienChuNhiem(int maGVCN)
        {
            return gvcnDA.GetGiaoVienChuNhiem(maGVCN);
        }

        public List<TabularGiaoVienChuNhiem> GetListTbGiaoVienChuNhiems(
            int maNamHoc, int maNganhHoc, int maKhoiLop, int maLopHoc,
            string maGiaoVien, string tenGiaoVien,
            int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            if (maLopHoc != 0)
            {
                if ((maGiaoVien == "") || (string.Compare(maGiaoVien, "tất cả", true) == 0))
                {
                    if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                    {
                        return gvcnDA.GetListTbGiaoVienChuNhiems(maLopHoc,
                            pageCurrentIndex, pageSize, out totalRecords);
                    }
                    else
                    {
                        return gvcnDA.GetListTbGiaoVienChuNhiemsByTen(maLopHoc, tenGiaoVien,
                            pageCurrentIndex, pageSize, out totalRecords);
                    }
                }
                else
                {
                    if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                    {
                        return gvcnDA.GetListTbGiaoVienChuNhiemsByMa(maLopHoc, maGiaoVien,
                            pageCurrentIndex, pageSize, out totalRecords);
                    }
                    else
                    {
                        return gvcnDA.GetListTbGiaoVienChuNhiems(maLopHoc, maGiaoVien, tenGiaoVien,
                            pageCurrentIndex, pageSize, out totalRecords);
                    }

                }
            }
            else
            {
                if (maNganhHoc == 0)
                {
                    if (maKhoiLop == 0) // maNganhHoc == 0 + maKhoiLop == 0
                    {
                        if ((maGiaoVien == "") || (string.Compare(maGiaoVien, "tất cả", true) == 0))
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNam(maNamHoc,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndTen(maNamHoc, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndMa(maNamHoc, maGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNam(maNamHoc, maGiaoVien, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }

                        }
                    }
                    else //maNganhHoc == 0 + maKhoiLop != 0
                    {
                        if ((maGiaoVien == "") || (string.Compare(maGiaoVien, "tất cả", true) == 0))
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndKhoi(maNamHoc, maKhoiLop,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndKhoiAndTen(maNamHoc, maKhoiLop, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndKhoiAndMa(maNamHoc, maKhoiLop, maGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndKhoi(maNamHoc, maKhoiLop, maGiaoVien, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }

                        }
                    }
                }
                else //maNganhHoc != 0
                {
                    if (maKhoiLop == 0) //maNganhHoc != 0 + maKhoiLop = 0
                    {
                        if ((maGiaoVien == "") || (string.Compare(maGiaoVien, "tất cả", true) == 0))
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndNganh(maNamHoc, maNganhHoc,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndNganhAndTen(maNamHoc, maNganhHoc, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndNganhAndMa(maNamHoc, maNganhHoc, maGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndNganh(maNamHoc, maNganhHoc, maGiaoVien, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }

                        }
                    }
                    else //maNganhHoc != 0 + maKhoiLop != 0
                    {
                        if ((maGiaoVien == "") || (string.Compare(maGiaoVien, "tất cả", true) == 0))
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNam(maNamHoc, maNganhHoc, maKhoiLop,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndTen(maNamHoc, maNganhHoc, maKhoiLop, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if ((tenGiaoVien == "") || (string.Compare(tenGiaoVien, "tất cả", true) == 0))
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNamAndMa(maNamHoc, maNganhHoc, maKhoiLop, maGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                return gvcnDA.GetListTbGiaoVienChuNhiemsByNam(maNamHoc, maNganhHoc, maKhoiLop, maGiaoVien, tenGiaoVien,
                                    pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                    }
                }
            }
        }      
    }
}
