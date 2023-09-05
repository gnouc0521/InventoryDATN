using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class WarehouseEnum
    {
        public enum TypeWarehouse : byte
        {
            [Display(Name = "Đang hoạt động")]
            WarehouseActive = 0,

            [Display(Name = "Kiểm kê")]
            WarehouseInventory = 1,

            [Display(Name = "Đầy kho")]
            WarehouseFull = 2,
        }
    }
}
