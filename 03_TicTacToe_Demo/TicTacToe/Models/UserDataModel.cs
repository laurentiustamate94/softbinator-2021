using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class UserDataModel
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "First player name")]
        public string FirstPlayerName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Second player name")]
        public string SecondPlayerName { get; set; }
    }
}
