# Trading.Web.Api

Aplicaci贸n que se encarga de mantener actualizados y disponibles los datos de las acciones. 

Diagrama de componentes de la Trading.Web.Api

<img style="padding: 10px" src="./Docs/component Diagram.jpg" alt="drawing" width="600px"/>

# Instrucciones para iniciar Trading.Web.Api

A continuacion se presentan las instrucciones necesarias para poner en funcionamiento Trading.Web.Api.

# Pre-requisitos

- Tener instalado el **Visual Studio** 2022 con la version .Net 5.0

- Tener instalado el **Microsoft MySql** Workbench 8.0 CE

- Tener instalado **Docker Desktop** (para despliegue con docker-compose)

# Instalaci贸n 馃敡

1) Descargar el proyecto y abrir en Visual Studio 2022

2) Realizar un compilado. Presionar con el click derecho sobre la solution del proyecto dentro del Visual Studio y ejecutar la opcion **Build Solution**.
   Esto se hace para verificar que el codigo compila correctamente y que las dependencias se instalen correctamente.

3) Para un deploy en local con MySql se tiene que crear una conexi贸n a MySql Workbench y modificar el archivo **appsetings.json** con la informaci贸n de la nueva conexi贸n.
Si el servidor tiene usuario y password es necesario incluirlos en el campo de la siguiente manera:
```
Server=(Nombre del servidor);Database=(Nombre de la base de datos creada);uid=(Usuario necesario para acceder al servidor); Password=(contrase帽a para acceder al servidor)
```

En caso de no tener usuario y password, eliminar los campos **User** y **Password**:

```
Server=(Nombre del servidor);Database=(Nombre de la base de datos creada)
```

# Descripci贸n del funcionamiento
Al arrancar la aplicaci贸n **Trading.Web.Api** se ejecutan una serie de acciones:
-  Realizado el (**SIT**) de la base de datos
   - Si la Base de Datos **NO** existe :
     - Crea la conexi贸n hacia MySql bas谩ndose en la configuraci贸n de **appsetings.json**
     - Realiza una migraci贸n **CodeFirst** para la creaci贸n de la Base de Datos y  las tablas
     - Realiza un **bulk insert** de todos los datos de las acciones desde la Api p煤blica.
   - Si la Base de Datos existe :
     - Comprueba que los datos de nuestra Base de Datos son id茅nticos que los de la Api p煤blica.
 - Se levanta el servicio de Web Api que proporciona toda la informaci贸n de las acciones actualizadas y disponibles en cada momento.  

# Deploy 馃摝
## Deploy en Heroku
Pasos a seguir para el correcto despliegue en **Heroku**:

Fase 1:
- Dirigirse a https://dashboard.heroku.com/apps
- Crear una aplicaci贸n nueva desde la web eligiendo la regi贸n mas cercana
- En la secci贸n de **Deployment method** elegir **Container Registry**
- Agregar el AddOn de **web** en la tienda de **Geroku**
- Modificar el archivo **appsetings.json** de igual manera que para servidor en local con la informaci贸n obtenida del perfil de **Heroku**

Fase 2:
-  Desde el directorio de la aplicaci贸n donde se encuentra el archivo **DockerFile** que se ha generado al crear la **Web Api** abrimos una terminal
-  Ejecutamos el primer comando despu茅s de lanzar la aplicaci贸n **docker desktop** que hemos instalado anteriormente.
Al lanzar el comando de **Login de Heroku** se abre un navegador para completar la autenticaci贸n hacia la cuenta creada.
```
 heroku login
```
- Segundo comando es el login de contenedor para registro.
```
 heroku container: login
```
- Importante que el nombre de la aplicaci贸n sea el mismo que en la web de **Heroku**. Construye la imagen en docker y la sube a Heroku.
```
 heroku container: push **NombreAppEnHeroku** web
```
- 脷ltimo comando publica la aplicaci贸n y la hace accesible.
```
 heroku container: release **NombreAppEnHeroku** web
```
Deploy de la Web.Api en **Heroku**:
- https://tradingwebapiapp.herokuapp.com/swagger/index.html

<img style="padding: 10px" src="./Docs/6.jpg" alt="drawing"/>

# Principales caracteristicas de la Web.Api 

## Paginado de la Web Api
- Creaci贸n de un End-Point para devolver las peticiones paginadas juntando toda la informaci贸n necesaria para su posterior uso en el cliente.
``` C#
{
  "pageNumber": 2,
  "pageSize": 3,
  "totalPages": 4116,
  "totalRecords": 12346,
  "data": [
    {
      "name": "Goldman Sachs Physical Gold ETF",
      "ticket": "AAAU"
    },
    {
      "name": "Ares Acquisition Corporation - Class A",
      "ticket": "AAC"
    },
    {
      "name": "Ares Acquisition Corporation - Units (1 Ord Share Class A & 1/5 War)",
      "ticket": "AAC-U"
    }
  ]
}
```
- Creaci贸n de reglas a trav茅s del Middleware **FluentValidator** para impedir que se lance una petici贸n con informaci贸n marcada como inv谩lida o vac铆a.
   
``` C#
namespace TradingApp.Web.Api.Extensions
{
    public class PortfolioStockDtoValidator : AbstractValidator<PortfolioStockDTO>
    {
        public PortfolioStockDtoValidator()
        {
            RuleFor(model => model.Amount).NotEmpty();
        }
    }
}
```
- Implementaci贸n de **Automapper** para el mapeo de objetos.

``` C#
private static MapperConfiguration GetMapperConfiguration()
        {
            return new(mapperConfiguration =>
            {
                mapperConfiguration
                    //mapeo entre data transfer object y entidades
                    .CreateMap<StockDTO, StockEntity>()
                    .ReverseMap();
                mapperConfiguration
                    .CreateMap<StockEntity, Stock>()
                    .ReverseMap();             
            });
        }
```
- Bulk Insert para la inserci贸n de las acciones en nuestra base de datos
``` C#
for (int i = 0; i < stocks.Count; i += insertSize)
    {
        var stocksToInsert = stocks.GetRange(i, Math.Min(insertSize, stocks.Count - i));
        await _stockRepository.BulkInsert(_mapper.Map<ICollection<Stock>>(stocksToInsert));
    }
    _unitOfWork.Commit();
```
- Activar el CORS en ASP.NET Core para permitir peticiones procedentes de diferentes dominios
``` C#
//CORS Enabled 
        services.AddCors(opt =>
        {
            opt.AddPolicy(name: _policyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
```


# Wiki 馃摉

* [.Net Framework](https://docs.microsoft.com/es-es/dotnet/framework/get-started/)
* [Inserci贸n de Dependencias](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0)
* [Middleware de ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0) 

* [Configuraci贸n en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0)
* [Enrutamiento en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/routing?view=aspnetcore-6.0)
* [Controlar errores en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/error-handling?view=aspnetcore-6.0)
* [HttpRequest](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0)

  