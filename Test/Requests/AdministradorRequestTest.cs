using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Infraestrutura.DTOs;
using Test.Helpers;

namespace Test.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Helpers.Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Helpers.Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TesteGetPropiedades()
        {
            //Arange
            var loginDTO = new LoginDTO()
            {
                Email = "adm@teste.com",
                Senha = "senha123",
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            //Act
            var response = await Setup.client.PostAsync("/administradores/login", content);


            //Assert
            // Assert.AreEqual(1, adm.Id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            

        }
    }
}