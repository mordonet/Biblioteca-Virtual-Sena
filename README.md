# 📚 Biblioteca Virtual SENA

Aplicación web para consultar, guardar y administrar libros digitales en PDF. Es un proyecto formativo del **SENA**.

## Funcionalidades

- Registro e inicio de sesión de usuarios (cookies de autenticación)
- Catálogo de libros con filtros y vista de detalle
- Lectura y descarga de libros en PDF
- Lista de libros favoritos por usuario
- Panel de administrador: agregar, editar y eliminar libros y usuarios
- Páginas de error personalizadas (404 / 500)

## Tecnologías

| Capa | Tecnología |
|------|-----------|
| Backend | ASP.NET Core 8 MVC (C#) |
| Base de datos | MySQL / MariaDB (XAMPP) con Entity Framework Core + Pomelo |
| Frontend | Razor Views, HTML, CSS, JavaScript |
| Extra | `proyecto-nosql/`: API en Node.js + MongoDB |

## Cómo ejecutarlo

1. Instala **XAMPP** y enciende **MySQL** desde el panel de control.
2. Abre phpMyAdmin (`http://localhost/phpmyadmin`) e importa el archivo [`database/BibliotecaVirtualDB.sql`](database/BibliotecaVirtualDB.sql). Ese archivo crea la base de datos `BibliotecaVirtualDB` con todas sus tablas.
3. Revisa la cadena de conexión en `BibliotecaVirtual/BibliotecaVirtual/appsettings.json` (usuario `root` y clave vacía por defecto).
4. Abre `BibliotecaVirtual/BibliotecaVirtual.sln` en Visual Studio y presiona **F5**, o ejecuta:

   ```bash
   cd BibliotecaVirtual/BibliotecaVirtual
   dotnet run
   ```

5. Entra a `http://localhost:5220`.

**Usuario administrador de prueba:** `admin@biblioteca.com` / `admin123`

## Modelo de datos

- **Usuarios**: Id, Nombre, Correo, Password, Rol
- **Libros**: Id, Titulo, Autor, Categoria, Descripcion, UrlPortada, UrlArchivo, UsuarioId
- **Favoritos**: Id, UsuarioId → Usuarios, LibroId → Libros

## Autores

- **Brayan Martínez** ([@mordonet](https://github.com/mordonet))
- [@micalooo](https://github.com/micalooo), coautor del proyecto original
