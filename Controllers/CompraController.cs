using MongoDB.Bson;
using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
    public class CompraController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<CompraModel> CompraCollection;
        private IMongoCollection<ProductosModel> productosCollection;


        public CompraController()
        {
            dbcontext = new MongoDBContext();
            CompraCollection = dbcontext.database.GetCollection<CompraModel>("Compra");
            productosCollection = dbcontext.database.GetCollection<ProductosModel>("Productos");

        }

        // GET: Compra
        public ActionResult IndexCompra(string idDetalleCompra)
        {
            var compras = CompraCollection.AsQueryable<CompraModel>()
                .Where(c => c.IdDetalleCompra == idDetalleCompra)
                .ToList();

            ViewBag.IdDetalleCompra = idDetalleCompra;  // Para mantener el contexto
            return View(compras);
        }

        public ActionResult Create(string idDetalleCompra)
        {
            // Filtra solo los productos cuyo estatus sea "Continuado"
            var productos = productosCollection.AsQueryable<ProductosModel>()
                .Where(p => p.Estatus == "Continuado")
                .ToList();

            ViewBag.NombreProductos = new SelectList(productos, "NombreProducto", "NombreProducto");

            // Enviamos los datos de los productos con nombre y precio para usarlos en el JavaScript
            ViewBag.ProductosData = productos.Select(p => new { p.NombreProducto, p.PrecioProducto }).ToList();
            ViewBag.IdDetalleCompra = idDetalleCompra;

            return View();
        }

        // POST: Compra/Create
        [HttpPost]
        public ActionResult Create(CompraModel compra)
        {
            try
            {
                var productos = productosCollection.AsQueryable<ProductosModel>().ToList();
                ViewBag.NombreProductos = new SelectList(productos, "NombreProducto", "NombreProducto");
                CompraCollection.InsertOne(compra);
                return RedirectToAction("IndexCompra", new { idDetalleCompra = compra.IdDetalleCompra });
            }
            catch
            {
                return View();
            }
        }

        // GET: Compra/Edit/5
        public ActionResult Edit(string id)
        {
            // Filtra solo los productos cuyo estatus sea "Continuado"
            var productos = productosCollection.AsQueryable<ProductosModel>()
                .Where(p => p.Estatus == "Continuado")
                .ToList();

            ViewBag.NombreProductos = new SelectList(productos, "NombreProducto", "NombreProducto");

            // Enviamos los datos de los productos con nombre y precio para usarlos en el JavaScript
            ViewBag.ProductosData = productos.Select(p => new { p.NombreProducto, p.PrecioProducto }).ToList();

            var Id = new ObjectId(id);
            var compra = CompraCollection.AsQueryable<CompraModel>().SingleOrDefault(x => x._IdCompra == Id);

            if (compra == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdDetalleCompra = compra.IdDetalleCompra;

            return View(compra);
        }




        // POST: Compra/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, CompraModel compra)
        {
            try
            {
                var filter = Builders<CompraModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<CompraModel>.Update
                    .Set("NombreProducto", compra.NombreProducto)
                    .Set("CantidadProductoCompra", compra.CantidadProductoCompra)
                    .Set("PrecioUnidad", compra.PrecioUnidad) // Asegúrate de que estos valores se actualicen correctamente
                    .Set("PrecioAcumulado", compra.PrecioAcumulado);

                CompraCollection.UpdateOne(filter, update);
                return RedirectToAction("IndexCompra", new { idDetalleCompra = compra.IdDetalleCompra });
            }
            catch
            {
                return View();
            }
        }




        // GET: Compra/Delete/5
        public ActionResult Delete(string id)
        {
            var Id = new ObjectId(id);
            var compra = CompraCollection.AsQueryable<CompraModel>().SingleOrDefault(x => x._IdCompra == Id);
            return View(compra);
        }

        // POST: Compra/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, CompraModel compra)
        {
            try
            {
                CompraCollection.DeleteOne(Builders<CompraModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("IndexCompra", new { idDetalleCompra = compra.IdDetalleCompra });
            }
            catch
            {
                return View();
            }
        }
    }
}
