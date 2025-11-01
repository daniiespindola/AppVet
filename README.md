# AppVet — Sistema de Gestión Veterinaria 🐾
👥 Integrantes

- Daniel Espindola
- Rebeca Anahí Luna Colque

⚙️ Tecnologías

.NET 8 (ASP.NET Core MVC)

C# / Entity Framework Core

SQL Server

Google OAuth 2.0 (inicio de sesión con cuenta de Google)

# Descripción

**AppVet** es una aplicación web que gestiona la información de una veterinaria.
Permite registrar **clientes, veterinarios** y **administradores**, cada uno con diferentes permisos y vistas personalizadas (Dashboards).

**Roles:**

- **Administrador:** acceso total, puede asignar o quitar roles.

- **Veterinario:** CRUD de mascotas, fichas médicas y turnos.

- **Cliente:** solo lectura (visualiza su información y fichas).

⚠️ **Nota sobre el rol Administrador**
El administrador puede **editar, ver detalles y eliminar** clientes y veterinarios, pero **no puede crearlos** directamente.  
La creación de usuarios se realiza únicamente mediante **inicio de sesión con Google**, donde el usuario decide si registrarse como Cliente o Veterinario.


# Inicio de Sesión con Google 🔑

El sistema usa autenticación con Google.

Si el usuario no existe, se crea con rol "Pendiente".

En **HomeController**, el correo del administrador puede modificarse:
```bash
string rol = (email == "TU_CORREO@gmail.com") ? "Administrador" : "Pendiente";
```
👉 Cada integrante puede poner su propio correo para ser administrador.

# Base de Datos

Motor: SQL Server
ORM: Entity Framework Core
Cadena de conexión **(appsettings.json):**
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=DbAppVet;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

⚠️ Cambiar TU_SERVIDOR por el nombre local de tu equipo.

# Ejecución del Proyecto 🚀

- Clonar o abrir el proyecto en Visual Studio.

- Verificar la cadena de conexión en appsettings.json.

- Crear la base de datos con los siguientes comandos en la Consola del Administrador de Paquetes:
```bash
Add-Migration InitialCreate
Update-Database
```

- Ejecutar el proyecto con F5.

- Iniciar sesión con una cuenta de Google.

# Backup de la Base de Datos 💾

Para hacer una copia de seguridad:

Abrir SQL Server Management Studio (SSMS).

Clic derecho sobre DbAppVet → Tareas → Copia de seguridad....

Guardar el archivo .bak (por ejemplo: C:\Backup\DbAppVet.bak).

👉 Ese archivo puede restaurarse en otro equipo con Restaurar base de datos
