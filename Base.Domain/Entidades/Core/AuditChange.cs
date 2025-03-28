using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entidades.Core
{
    [Table("Tbl_AuditChanges")]
    public class AuditChange : BaseEntity
    {
        public string Action { get; set; }
        public int IdEntity { get; set; }
        public string TableName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public string IPAddress { get; set; }
        public DateTime RowVersion { get; set; }

    }
}
