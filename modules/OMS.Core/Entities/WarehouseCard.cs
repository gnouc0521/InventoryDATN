using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
  [Table("WarehouseCard", Schema = netcoreConsts.SchemaName)]
  public class WarehouseCard : FullAuditedEntity<long>
  {
    [StringLength(50)]
    public string Code { get; set; }
    public int WarehouseDestinationId { get; set; }
    public int UnitId { get; set; }

    public long ItemsId { get; set; }


  }
}
