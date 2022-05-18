using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChatApp.BLL.Contracts.DTOs
{
    public class ChatDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }


    }
}
