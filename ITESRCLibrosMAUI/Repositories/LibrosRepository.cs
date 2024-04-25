using ITESRCLibrosMAUI.Models.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITESRCLibrosMAUI.Repositories
{
    public class LibrosRepository
    {
        SQLiteConnection context;


        public LibrosRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/libros.db3";
            context = new SQLiteConnection(ruta);
            context.CreateTable<Libro>();
        }

        public void Insert(Libro L)
        {
            context.Insert(L);
        }

        public IEnumerable<Libro> GetAll()
        {
            return context.Table<Libro>()
               .OrderBy(x => x.Titulo);
        }

        public Libro? Get(int id)
        {
            return context.Find<Libro>(id);
        }

        public void InsertOrReplace(Libro L)
        {
            context.InsertOrReplace(L);
        }

        public void Update(Libro L)
        {
            context.Update(L);
        }

        public void Delete(Libro L)
        {
            context.Delete(L);
        }

    }
}
