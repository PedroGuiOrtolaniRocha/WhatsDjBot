using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsDjBotAPI.Models
{
    [Table("Music")]
    public class Music
    {
        public int Id { get; private set; }
        public string Link { get; set; }
        public string Platform { get; set; }
        public int UserId { get; set; }
    }
}
