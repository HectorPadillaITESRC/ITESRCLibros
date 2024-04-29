using ITESRCLibrosMAUI.Models.DTOs;
using ITESRCLibrosMAUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ITESRCLibrosMAUI.Services
{
    public class LibrosService
    {
        HttpClient cliente;
        Repositories.LibrosRepository librosRepository = new();

        public LibrosService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://libros.itesrc.net/")
            };


        }

        public async Task Agregar(LibrosDTO dto)
        {
            //    var json = JsonSerializer.Serialize(dto);
            //    var response = await cliente.PostAsync("api/libros", new StringContent(json, Encoding.UTF8,
            //        "application/json"));


            var response = await cliente.PostAsJsonAsync("api/libros", dto);

            if (response.IsSuccessStatusCode)
            {
                await GetLibros(); //Baja los libros de la api a la BD local
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }

        }

        public async Task GetLibros()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);


                var response = await cliente.GetFromJsonAsync<List<LibrosDTO>>($"api/libros/{fecha:yyyy-MM-dd}T{fecha:HH:mm:ss}");
                if (response != null)
                {
                    foreach (LibrosDTO libro in response)
                    {
                        var entidad = librosRepository.Get(libro.Id ?? 0);

                        if (entidad == null && libro.Eliminado == false) //SI no estaba en BD Local, lo agrego
                        {
                            entidad = new()
                            {
                                Id = libro.Id ?? 0,
                                Autor = libro.Autor,
                                Portada = libro.Portada,
                                Titulo = libro.Titulo
                            };
                            librosRepository.Insert(entidad);
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                if (entidad.Eliminado)
                                {
                                    librosRepository.Delete(entidad);
                                }
                                else
                                {
                                    librosRepository.Update(entidad);
                                }
                            }
                        }


                    }

                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.Fecha));
                }
            }
            catch
            {

            }
        }

    }
}
