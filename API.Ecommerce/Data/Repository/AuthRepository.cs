using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models;
using API.Ecommerce.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository()
        {
            _context = new ApplicationDbContext();
        }


        public async Task<int> RegistraUsuario(UsuariosAuth auth)
        {
            int idUsuario = 0;
            try
            {
                _context.UsuariosAuth.Add(auth);
                int res = await _context.SaveChangesAsync();
                if (res > 0)
                {
                    idUsuario = auth.Id;
                }
            }
            catch (Exception ex)
            {
                idUsuario = -1;
            }
            return idUsuario;
        }

        public async Task<UsuariosAuth> ObtieneUsuarioPorId(int id)
        {
            UsuariosAuth? user = new UsuariosAuth();
            try
            {
                user = await _context.UsuariosAuth.FindAsync(id);
                if (user == null)
                {
                    return new UsuariosAuth();
                }
            }
            catch (Exception ex)
            {
                user = null;
            }
            return user;
        }

        public async Task<bool> ObtieneUsuarioPorCorreo(string correo)
        {
            var existe = false;
            try
            {
                var user = await _context.UsuariosAuth.FirstOrDefaultAsync(x => x.Email == correo);
                if (user == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return existe;
        }

        public async Task<UsuariosAuth?> GetByEmailAsync(string email)
        {
            return await _context.UsuariosAuth
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EliminaUsuario(int id)
        {
            int idAuth = 0;
            bool eliminado = false;
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
                if (cliente != null)
                {
                    idAuth = cliente.IdAuth;
                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                }
                var user = await _context.UsuariosAuth.FindAsync(idAuth);
                if (user != null)
                {
                    _context.UsuariosAuth.Remove(user);
                    int res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {
                        eliminado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                eliminado = false;
            }
            return eliminado;
        }

        public async Task<bool> ActualizaUsuario(UsuariosAuth auth,Cliente cliente)
        {
            bool actualizado = false;
            try
            {
                _context.UsuariosAuth.Update(auth);

                _context.Clientes.Update(cliente);


                int res = await _context.SaveChangesAsync();
                if (res > 0)
                {
                    actualizado = true;
                }
            }
            
            catch (Exception ex)
            {
                actualizado = false;
            }
            return actualizado;
        }

        public async Task<List<ClienteConRolDto>> ObtenerUsuarios()
        {
            var resultado = _context.Clientes
             .Where(c => c.UsuarioAuth.Rol.Nombre != "Cliente")
             .Select(c => new
             {
                 c.Id,
                 c.Nombre,
                 c.ApellidoPaterno,
                 c.ApellidoMaterno,
                 Email = c.UsuarioAuth.Email,
                 RolId = c.UsuarioAuth.Rol.Id,
                 RolNombre = c.UsuarioAuth.Rol.Nombre
             })
             .ToList();
            return resultado.Select(z => new ClienteConRolDto()
            {
                Id = z.Id,
                Nombre = z.Nombre,
                ApellidoPaterno = z.ApellidoPaterno,
                ApellidoMaterno = z.ApellidoMaterno,
                Email = z.Email,
                RolId = z.RolId,
                RolNombre = z.RolNombre
            }).ToList();
        }
    }
}
