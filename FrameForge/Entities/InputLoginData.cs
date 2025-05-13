﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InputLoginData
    {

        [Required]
        [JsonProperty("name")]
        [MaxLength(50)]
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsTeacher { get; set; }
    }
}
