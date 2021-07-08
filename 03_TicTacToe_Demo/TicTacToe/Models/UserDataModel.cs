using System.ComponentModel.DataAnnotations;

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
