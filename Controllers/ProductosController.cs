using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;

namespace PinguRock.Controllers
{
    public class ProductosController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<ProductosModel> productosCollection;

        public ProductosController()
        {
            dbcontext = new MongoDBContext();
            productosCollection = dbcontext.database.GetCollection<ProductosModel>("Productos");
        }

        // GET: Productos
        public ActionResult Index()
        {
            List<ProductosModel> productos = productosCollection.AsQueryable<ProductosModel>().ToList();
            return View(productos);
        }

        // GET: Productos/Details/5
        public ActionResult Details(string id)
        {
            var Id = new ObjectId(id);
            var producto = productosCollection.AsQueryable<ProductosModel>().SingleOrDefault(x => x.IdProducto == Id);
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        public ActionResult Create(ProductosModel producto)
        {
            try
            {
                // Asignar valores para los niveles de stock
                int stockMinimo = 10; // Ajusta estos valores según tus necesidades
                int stockBajo = 20;
                int stockModerado = 30;
                int stockSuficiente = 40;
                int stockOptimo = 50;

                producto.CalcularEstadoStock(stockMinimo, stockBajo, stockModerado, stockSuficiente, stockOptimo);

                productosCollection.InsertOne(producto);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Productos/Edit/5
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var producto = productosCollection.AsQueryable<ProductosModel>().SingleOrDefault(x => x.IdProducto == Id);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ProductosModel producto)
        {
            try
            {
                var filter = Builders<ProductosModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ProductosModel>.Update
                    .Set("NombreProducto", producto.NombreProducto)
                    .Set("CantidadProducto", producto.CantidadProducto)
                    .Set("EstadoStock", producto.EstadoStock);

                var result = productosCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(string id)
        {
            var Id = new ObjectId(id);
            var producto = productosCollection.AsQueryable<ProductosModel>().SingleOrDefault(x => x.IdProducto == Id);
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, ProductosModel producto)
        {
            try
            {
                productosCollection.DeleteOne(Builders<ProductosModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
