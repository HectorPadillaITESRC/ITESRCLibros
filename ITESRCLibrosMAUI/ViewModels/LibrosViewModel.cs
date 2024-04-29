using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ITESRCLibrosMAUI.Models.DTOs;
using ITESRCLibrosMAUI.Models.Entities;
using ITESRCLibrosMAUI.Models.Validators;
using ITESRCLibrosMAUI.Repositories;
using ITESRCLibrosMAUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITESRCLibrosMAUI.ViewModels
{
    public partial class LibrosViewModel : ObservableObject
    {
        LibrosRepository librosRepository = new();

        public ObservableCollection<Libro> Libros { get; set; } = new();



        LibrosService service = new();

        [ObservableProperty]
        private LibrosDTO? libro;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo()
        {
            Libro = new();
            Shell.Current.GoToAsync("//Agregar");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Libro = null;
            Error = "";
            Shell.Current.GoToAsync("//Lista");
        }

        LibroValidator validador = new();

        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                if (Libro != null)
                {
                    var resultado = validador.Validate(Libro);
                    if (resultado.IsValid)
                    {
                        await service.Agregar(Libro);
                        ActualizarLibros();
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }


        public LibrosViewModel()
        {
            ActualizarLibros();
            _=service.GetLibros();
        }

        void ActualizarLibros()
        {
            Libros.Clear();
            foreach (var libro in librosRepository.GetAll())
            {
                Libros.Add(libro);
            }

        }
    }
}
