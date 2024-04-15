using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FILECONTRACT_TYPE")]
public partial class HU_FILECONTRACT_TYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long FILE_CONTRACT_ID { get; set; }
    public long TYPE_ID { get; set; }
    

}
