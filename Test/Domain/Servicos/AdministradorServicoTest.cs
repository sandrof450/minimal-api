using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto CriarContextoTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            //Configurar o ConfirutinBuilder
            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            var configuration = builder.Build();

            return new DbContexto(configuration);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            //Arange
            var context = CriarContextoTeste();
            context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS Administradores");
            context.Database.ExecuteSqlRaw("CREATE TABLE Administradores (Id INT PRIMARY KEY AUTO_INCREMENT,Email VARCHAR(255) NOT NULL,Senha VARCHAR(255) NOT NULL,Perfil VARCHAR(50) NOT NULL)");
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");


            var adm = new Administrador();
            adm.Id = 1;
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";

            var administradorServico = new AdministradorServicos(context);

            //Act
            administradorServico.Incluir(adm);
            //administradorServico.BuscarPorId(1);

            //Assert
            Assert.AreEqual(1, administradorServico.Todos(1).Count());
            Assert.AreEqual("teste@teste.com", adm.Email);
            Assert.AreEqual("teste", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}