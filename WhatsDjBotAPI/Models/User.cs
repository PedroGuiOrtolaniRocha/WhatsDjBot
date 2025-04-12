using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsDjBotAPI.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
    }
}
