using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Infraestrutura.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}