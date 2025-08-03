using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.interfaces;
using minimal_api.Infraestrutura.Db;
using minimal_api.Infraestrutura.DTOs;

namespace minimal_api.Dominio.Servicos
{
  public class AdministradorServicos : IAdministradorServico
  {
    private readonly DbContexto _contexto;
    public AdministradorServicos(DbContexto db)
    {
      _contexto = db;
    }

    public Administrador Incluir(Administrador administrador)
    {
      _contexto.Administradores.Add(administrador);
      _contexto.SaveChanges();

      return administrador;
    }

    public List<Administrador> Todos(int? pagina)
    {
      var query = _contexto.Administradores.AsQueryable();

      int itensPorPagina = 10;
      if (pagina != null)
      {
        query = query.Skip(((int)pagina - 1) * itensPorPagina)
                    .Take(itensPorPagina);

      }
      return query.ToList();
    }

    Administrador? IAdministradorServico.Login(LoginDTO loginDTO)
    {
      return _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
    }
    
    public Administrador? BuscarPorId(int id)
    {
      return _contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
    }
    
  }
}