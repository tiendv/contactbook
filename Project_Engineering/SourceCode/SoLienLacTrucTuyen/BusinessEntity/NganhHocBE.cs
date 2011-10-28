using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.BusinessEntity
{
    public class NganhHocBE
    {
        private int maNganhHoc;
        public int MaNganhHoc 
        {
            get
            {
                return maNganhHoc;
            }

            set
            {
                maNganhHoc = value;
            }
        }

        private string tenNganhHoc;
        public string TenNganhHoc
        {
            get
            {
                return tenNganhHoc;
            }

            set
            {
                tenNganhHoc = value;
            }
        }

        private string moTa;
        public string MoTa
        {
            get
            {
                return moTa;
            }

            set
            {
                moTa = value;
            }
        }

        public NganhHocBE()
        {
        }
    }
}
