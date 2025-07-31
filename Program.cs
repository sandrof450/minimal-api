using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Enums;
using minimal_api.Dominio.interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;
using minimal_api.Infraestrutura.DTOs;

#region builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["JWT:Key"];
if (string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
  option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
  option.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
    ValidateIssuer = false,
    ValidateAudience = false,
  };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServicos>();
builder.Services.AddScoped<IVeiculoSevico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Insira o token JWT desta maneira: {seu_token_jwt}"
  });

  options.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      new string[] {}
    }
  });
});


builder.Services.AddDbContext<DbContexto>(
  options => options.UseMySql(
    builder.Configuration.GetConnectionString("mysql"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
  )
);

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region  Administradores
string GerarTokenJwt(Administrador administrador)
{
  if (string.IsNullOrEmpty(key)) return string.Empty;

  var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
  var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


  var claims = new List<Claim>
  {
    new Claim("Email", administrador.Email),
    new Claim("Perfil", administrador.Perfil),
    new Claim(ClaimTypes.Role, administrador.Perfil)
  };

  var token = new JwtSecurityToken(
    claims: claims,
    expires: DateTime.Now.AddDays(1),
    signingCredentials: credentials
  );
  
  return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
  var administrador = administradorServico.Login(loginDTO);
  if (administrador != null) {
    string token = GerarTokenJwt(administrador);
    return Results.Ok(new AdministradorLogado
    {
      Email = administrador.Email,
      Perfil = administrador.Perfil,
      Token = token
    });
  }
  else
    return Results.Unauthorized();
}).AllowAnonymous().WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
  var validacao = new ErrorDeValidacao
  {
    Mensagens = new List<string>()
  };

  if (string.IsNullOrEmpty(administradorDTO.Email))
    validacao.Mensagens.Add("O email é obrigatório.");

  if (string.IsNullOrEmpty(administradorDTO.Senha))
    validacao.Mensagens.Add("A senha é obrigatória.");

  if (administradorDTO.Perfil == null)
    validacao.Mensagens.Add("O perfil é obrigatório.");

  if (validacao.Mensagens.Any())
    return Results.BadRequest(validacao);


  var administrador = new Administrador
  {
    Email = administradorDTO.Email,
    Senha = administradorDTO.Senha,
    Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString(),
  };

  administradorServico.Incluir(administrador);

  return Results.Created($"/administrador/{administrador.Id}", new AdministradorModelView
    {
      Id = administrador.Id,
      Email = administrador.Email,
      Perfil = administrador.Perfil,
    });

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
.WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
  var adms = new List<AdministradorModelView>();
  var administradores = administradorServico.Todos(pagina);
  foreach (var adm in administradores)
  {
    adms.Add(new AdministradorModelView
    {
      Id = adm.Id,
      Email = adm.Email,
      Perfil = adm.Perfil,
    });
  }

  return Results.Ok(adms);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
.WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
  var administrador = administradorServico.BuscarPorId(id);

  if (administrador == null) return Results.NotFound();

  return Results.Ok(new AdministradorModelView
    {
      Id = administrador.Id,
      Email = administrador.Email,
      Perfil = administrador.Perfil,
    });
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
.WithTags("Administradores");
#endregion

#region veiculos 
ErrorDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
  var validacao = new ErrorDeValidacao
  {
    Mensagens = new List<string>()
  };

  if (string.IsNullOrEmpty(veiculoDTO.Nome))
    validacao.Mensagens.Add("O nome do veículo é obrigatório.");

  if (string.IsNullOrEmpty(veiculoDTO.Marca))
    validacao.Mensagens.Add("A marca do veículo é obrigatória."); 

  if (veiculoDTO.Ano <= 0 || veiculoDTO.Ano < 1950 || veiculoDTO.Ano > DateTime.Now.Year)
    validacao.Mensagens.Add("O ano do veículo é obrigatório e deve ser um número válido.");

  return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoSevico veiculoSevico) =>
{
  var validacao = validaDTO(veiculoDTO);

  if (validacao.Mensagens.Any())
    return Results.BadRequest(validacao);

  var veiculo = new Veiculo
  {
    Nome = veiculoDTO.Nome,
    Marca = veiculoDTO.Marca,
    Ano = veiculoDTO.Ano
  };

  veiculoSevico.Incluir(veiculo);

  return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm, Editor"})
.WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoSevico veiculoServico) =>
{
  var veiculos = veiculoServico.Todos(pagina);

  return Results.Ok(veiculos);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm, Editor"})
.WithTags("Veículos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoSevico veiculoSevico) =>
{
  var veiculo = veiculoSevico.BuscarPorId(id);

  if (veiculo == null) return Results.NotFound();

  return Results.Ok(veiculo);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm, Editor"})
.WithTags("Veículos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoSevico veiculoSevico) =>
{
  var validacao = validaDTO(veiculoDTO);

  if (validacao.Mensagens.Any())
    return Results.BadRequest(validacao);

  var veiculo = veiculoSevico.BuscarPorId(id);
  if (veiculo == null) return Results.NotFound();

  veiculo.Nome = veiculoDTO.Nome;
  veiculo.Marca = veiculoDTO.Marca;
  veiculo.Ano = veiculoDTO.Ano;

  veiculoSevico.Atualizar(veiculo);

  return Results.Ok(veiculo);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
.WithTags("Veículos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoSevico veiculoSevico) =>
{
  var veiculo = veiculoSevico.BuscarPorId(id);
  if (veiculo == null) return Results.NotFound();

  veiculoSevico.Apagar(veiculo);

  return Results.NoContent();
  
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
.WithTags("Veículos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion