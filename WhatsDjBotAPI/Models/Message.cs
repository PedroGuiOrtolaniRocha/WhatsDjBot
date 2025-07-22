using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsDjBotAPI.Models
{
    [Table("Message")]
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; }
        public string texto_bot { get; set; }
        public string texto_user { get; set; }

    }
}
