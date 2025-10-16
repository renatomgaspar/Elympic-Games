using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Matches
{
    public class InsertScoresViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Team One")]
        public int TeamOneId { get; set; }
        [ValidateNever]
        public Team TeamOne { get; set; }


        [Required]
        [Display(Name = "Team Two")]
        public int TeamTwoId { get; set; }
        [ValidateNever]
        public Team TeamTwo { get; set; }


        [Display(Name = "Team One Score")]
        public int? TeamOneScore { get; set; }


        [Display(Name = "Team Two Score")]
        public int? TeamTwoScore { get; set; }
    }
}
