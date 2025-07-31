using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.interfaces;
using minimal_api.Infraestrutura.Db;

namespace minimal_api.Dominio.Servicos
{
  public class VeiculoServico : IVeiculoSevico
  {
    private readonly DbContexto _contexto;
    public VeiculoServico(DbContexto db)
    {
        _contexto = db;
    }

    public void Apagar(Veiculo veiculo)
    {
      _contexto.Veiculos.Remove(veiculo);
      _contexto.SaveChanges();
    }

    public void Atualizar(Veiculo veiculo)
    {
      _contexto.Veiculos.Update(veiculo);
      _contexto.SaveChanges();
    }

    public Veiculo? BuscarPorId(int id)
    {
      return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Incluir(Veiculo veiculo)
    {
      _contexto.Veiculos.Add(veiculo);
      _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
      var query = _contexto.Veiculos.AsQueryable();
      if (!string.IsNullOrEmpty(nome))
      {
        query = query.Where(v => v.Nome.Contains(nome));
      }

      int itensPorPagina = 10;
      if (pagina != null)
      {
        query = query.Skip(((int)pagina - 1) * itensPorPagina)
                    .Take(itensPorPagina);
        
      }
      
      return query.ToList();
    }
  }
}