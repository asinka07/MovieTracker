using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Data.Models
{
    public class Watchlist
    {
        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser User { get; set; } = null!;
        [Required]
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public virtual Movie Movie { get; set; } = null!;
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
