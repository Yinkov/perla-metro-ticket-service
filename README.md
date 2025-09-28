# perla-metro-ticket-service
Este proyecto es un servicio de gestión de tickets desarrollado en **.NET**  que forma parte de una arquitectura **SOA**.  
Su responsabilidad es gestionar los boletos de pasajeros: **crear, consultar, actualizar y eliminar tickets** de manera segura, garantizando integridad y facilitando la administración.  

En este repositorio solo se encuentra el **servicio de tickets**. La **Main API** y los otros servicios viven en repositorios separados.

---

## Tecnologías utilizadas
- **.NET** (Web API)
- **MongoDB Atlas** como base de datos
- **Swagger** 
- Arquitectura orientada a servicios (SOA)

---

## 📂 Estructura básica
perla_metro_ticket_service/

│── Controllers/ # Endpoints de la API

│── Models/ # Entidades y enums

│── Dtos/ # Data Transfer Objects

│── Repositories/ # Repositorios y acceso a datos

│── Mappers/ # Mappers para pasar de dtos a modelo

│── Interfaces/ # Interfaces de repositorios

│── Program.cs # Configuración principal

│── README.md # Este archivo

## Configuración del entorno

El servicio requiere un archivo `.env` en la raíz del proyecto con la configuración de conexión a MongoDB Atlas, este archivo debe contener 3 variable:

`DATABASE_URL=mongodb+srv://<user>:<password>@base-ticket-service.y9rcn0b.mongodb.net/?retryWrites=true&w=majority&appName=Base-Ticket-Service`

`DATABASE_NAME=<nombre-de-la-base>`

`COLECTION_TICKET_NAME=<nombre-de-la-coleccion>`

# Pasos para configurar MongoDB Atlas

Crear una cuenta en MongoDB Atlas.

Crear un Cluster.

  Al momento de crearlo se le solicuta confirmar un usuario y contraseña, estan son importantes para más adelante

En el cluster, ir a la sección Database → Connect → Connect to your application.

  Presionar la opcion driver.

Seleccionar driver:[C#/.NET] versión: [2.25 or later].

Copiar el string de conexión que tendrá el formato similar a:

mongodb+srv://**user**:**password**@base-ticket-service.y9rcn0b.mongodb.net/?retryWrites=true&w=majority&appName=Base-Ticket-Service

Reemplazar **user** y **password** por los que configuraste en Atlas al momento de hacer el cluster.

Guardar ese string en el .env en la variable DATABASE_URL.

Las variable DATABASE_NAME y COLECTION_TICKET_NAME son para nombrar la base de datos y la coleccion respectivamente, estas pueden tener cualquier nombre


# Ejecución del proyecto
Abrir la cmd en alguna carpeta de conveniencia

1. Clonar el repositorio
   
git clone https://github.com/Yinkov/perla-metro-ticket-service.git

cd perla-metro-ticket-service

3. Restaurar dependencias

En la carpeta del proyecto, ejecutar:

dotnet restore

Añadir a la carpeta que se creo [perla-metro-ticket-service] el archivo .env preparado anteriormente

4. Ejecutar el servicio

dotnet run

en la cmd vera algo como:

info: Microsoft.Hosting.Lifetime[14]

      Now listening on: http://localhost:5192
      
info: Microsoft.Hosting.Lifetime[0]

      Application started. Press Ctrl+C to shut down.
      
info: Microsoft.Hosting.Lifetime[0]

      Hosting environment: Development
      
info: Microsoft.Hosting.Lifetime[0]

      Content root path: C:\yin\4-UCN-4\10-Decimo Semestre\Arquitectura de sistemas\Talleres\pREUBA DE FUNCI AAA\perla-metro-ticket-service
      
warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]

      Failed to determine the https port for redirect.
      
info: Microsoft.Hosting.Lifetime[0]

ir a http://localhost:5192/swagger
el numero puede variar



#  Documentación de la API

Cuando el servicio esté corriendo, puedes acceder a Swagger en:

 https://localhost:**numero**/swagger

Ahí encontrarás los endpoints para:

Crear ticket (POST "/Add")

Consultar tickets (GET "/GetAllTickets", GET "/Get/{id}")

Actualizar ticket (PUT "/Update/{id}")

Eliminar ticket (soft delete) (DELETE "/Delete/{id}")

# Notas adicionales

Los tickets son únicos, ademas que por su id, por combinación de IdUser + IssueDate gracias a un índice único en MongoDB.

El borrado es lógico (isActive = false) para mantener historial, aunque va no se mostraran al hacer solicitudes de get o getall.

Los enums (Type, State) se almacenan como strings en MongoDB para mayor legibilidad.

Al modificar los datos de un ticket que esta caducado, no se puede cambiar el estado a activo, Ademas no se puede modificar un ticket para que tenga la misma issueDate que otro del mismo usuario

Validaciones:

Price siempre debe ser mayor a 0.

Type solo acepta "Ida" o "Vuelta".

State solo acepta "Activo", "Usado" o "Caducado".










