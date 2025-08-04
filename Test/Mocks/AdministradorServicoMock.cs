using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.interfaces;
using minimal_api.Infraestrutura.DTOs;

namespace Test.Mocks
{
  public class AdministradorServicoMock : IAdministradorServico
    {
        private static List<Administrador> Administradores = new List<Administrador>()
        {
            new Administrador {
                Id = 1,
                Email = "adm@teste.com",
                Senha = "senha123",
                Perfil = "Adm"
            },
            new Administrador {
                Id = 2,
                Email = "editor@teste.com",
                Senha = "senha123",
                Perfil = "Editor"
            },
        };

        public Administrador? BuscarPorId(int id)
        {
            return Administradores.Find(a => a.Id == id);
        }

        public Administrador Incluir(Administrador administrador)
        {
            administrador.Id = Administradores.Count() + 1; // Simulate auto-increment ID
            Administradores.Add(administrador);

            return administrador;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            return Administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
        }

        public List<Administrador> Todos(int? pagina)
        {
            return Administradores;
        }
    }
}