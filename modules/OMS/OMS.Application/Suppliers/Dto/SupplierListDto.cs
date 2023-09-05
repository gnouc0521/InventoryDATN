using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Suppliers.Dto
{
    [AutoMap(typeof(Supplier))]
    public class SupplierListDto :Entity<long>
    {
        [StringLength(50)]
        public string Code { get; set; }
        public Address provinces { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public string Area { get; set; }


        [StringLength(500)]
        public string Adress { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public string Fax { get; set; }

        [StringLength(500)]
        public string TaxCode { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        public string Bank { get; set; }
        public string STK { get; set; }
        public string NameRepresentative { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }
    }
}
