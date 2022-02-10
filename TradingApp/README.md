# Trading.Web.Api

Aplicación que se encarga de mantener actualizados y disponibles los datos de las acciones. 
# Instrucciones para iniciar Trading.Web.Api

A continuacion se presenta las instrucciones necesarias para poner en funcionamiento Trading.Web.Api.

# Pre-requisitos

- Tener instalado el **Visual Studio** 2022 con la version .Net 5.0

- Tener instalado el **Microsoft MySql** Workbench 8.0 CE

- Tener instalado **Docker Desktop** (para despliegue con docker-compose)

# Instalación 🔧

1) Descargar el proyecto y abrir en Visual Studio 2022

2) Realizar un compilado. Presionar con el click derecho sobre el solution del proyecto dentro del Visual Studio y ejecutar la opcion **Build Solution**.
   Esto se hace para verificar que el codigo compila correctamente y que las dependencias se instale correctamente.

3) Para un deploy en local con MySql se tiene que crear una conexión a MySql Workbench y modificar el archivo **appsetings.json** con la información de la nueva conexión.
Si el servidor tiene usuario y password es necesario incluirlos en el campo de la siguiente manera:
```
Server=(Nombre del servidor);Database=(Nombre de la base de datos creada);uid=(Usuario necesario para acceder al servidor); Password=(contraseña para acceder al servidor)
```

En caso de no tener usuario y password, eliminar los campos **User** y **Password**:

```
Server=(Nombre del servidor);Database=(Nombre de la base de datos creada)
```

# Descripción del funcionamiento
Al arrancar la aplicación **Trading.Web.Api** se ejecutan una serie de acciones:
-  Pruebas de Integración (**SIT**)
   - Si la Base de Datos **NO** existe :
     - Crea la conexión hacia MySql basándose en la configuración de **appsetings.json**
     - Realiza una migración **CodeFirst** para la creación de la Base de Datos y  las tablas
     - Realiza un **bulk insert** de todos los datos de las acciones desde la Api pública.
   - Si la Base de Datos existe :
     - Comprueba que los datos de nuestra Base de Datos son idénticos que los de la Api pública.
 - Se levanta el servicio de Web Api que proporciona toda la información de las acciones actualizada y disponible en cada momento.  

# Deploy 📦
## Deploy en Heroku
Pasos a seguir para el correcto despliegue en **Heroku**:

Fase 1:
- Dirigirse a https://dashboard.heroku.com/apps
- Crear una aplicación nueva desde la web eligiendo la región mas cercana
- En la sección de **Deployment method** elegir **Container Registry**
- Agregar el AddOn de **web** en la tienda de **Geroku**
- Modificar el archivo **appsetings.json** de igual manera que para servidor en local con la información obtenida del perfil de **Heroku**

Fase 2:
-  Desde el directorio de la aplicación donde se encuentra el archivo **DockerFile** que se ha generado al crear la **Web Api** abrimos una terminal
-  Ejecutamos el primer comando después de lanzar la aplicación **docker desktop** que hemos instalado anteriormente.
Al lanzar el comando de **Login de Heroku** se abre un navegador para completar la autenticación hacia la cuenta creada.
```
 heroku login
```
- Segundo comando es el login de contenedor para registro.
```
 heroku container: login
```
- Importante que el nombre de la aplicación sea el mismo que en la web de **Heroku**. Construye la imagen en docker y la sube a Heroku.
```
 heroku container: push **NombreAppEnHeroku** web
```
- Último comando publica la aplicación y la hace accesible.
```
 heroku container: release **NombreAppEnHeroku** web
```
Deploy de la Web.Api en **Heroku**:
- https://tradingwebapiapp.herokuapp.com/swagger/index.html

<img style="padding: 10px" src="./Docs/6.jpg" alt="drawing"/>

# Principales caracteristicas de la Web.Api 

## Paginado de la Web Api
- Creación de un End-Point para devolver las peticiones paginadas juntando toda la información necesaria para su posterior uso en el cliente.
```
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
- Creación de reglas a través del Middleware **FluentValidator** para impedir que se lance una petición con información marcada como inválida o vacía.
  
```
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
- Implementación de **Automapper** para el mapeo de objetos.

```
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
- Bulk Insert para la inserción de las acciones en nuestra base de datos
```
for (int i = 0; i < stocks.Count; i += insertSize)
    {
        var stocksToInsert = stocks.GetRange(i, Math.Min(insertSize, stocks.Count - i));
        await _stockRepository.BulkInsert(_mapper.Map<ICollection<Stock>>(stocksToInsert));
    }
    _unitOfWork.Commit();
```
- Activar el CORS en ASP.NET Core para permitir peticiones procedentes de diferentes dominios
```
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


# Wiki 📖

* [.Net Framework](https://docs.microsoft.com/es-es/dotnet/framework/get-started/)
* [Inserción de Dependencias](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0)
* [Middleware de ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0) 

* [Configuración en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0)
* [Enrutamiento en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/routing?view=aspnetcore-6.0)
* [Controlar errores en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/error-handling?view=aspnetcore-6.0)
* [HttpRequest](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0)

  