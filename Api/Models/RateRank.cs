using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("rate_rank")]
public class RateRank
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] [Column("id")] public int Id { get; set; }
    [Column("deposit_rank")] public double DepositRank { get; set; }
    [Column("rate")] public double Rate { get; set; }
}