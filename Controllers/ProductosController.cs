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
        private IMongoCollection<ProveedorModel> proveedorCollection;


        public ProductosController()
        {
            dbcontext = new MongoDBContext();
            productosCollection = dbcontext.database.GetCollection<ProductosModel>("Productos");
            proveedorCollection = dbcontext.database.GetCollection<ProveedorModel>("Proveedor");

        }

        // GET: Productos
        public ActionResult Index()
        {
            List<ProductosModel> productos = productosCollection.AsQueryable<ProductosModel>().ToList();
            return View(productos);
        }

        public ActionResult IndexStock()
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
            var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
            ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");
            return View();
        }


        // POST: Productos/Create
        [HttpPost]
        public ActionResult Create(ProductosModel producto)
        {
            try
            {
                productosCollection.InsertOne(producto);
                return RedirectToAction("Index");
            }
            catch
            {
                var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
                ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");


                // Volvemos a mostrar el formulario con los datos
                return View(producto);
            }
        }

        // GET: Edit de Productos
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var producto = productosCollection.AsQueryable<ProductosModel>().SingleOrDefault(x => x.IdProducto == Id);
            var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
            ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");
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
                    .Set("PrecioProducto", producto.PrecioProducto)
                    .Set("NombreProveedorFK", producto.NombreProveedorFK);

                var result = productosCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
                ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");

                return View();
            }
        }

        // GET: Edit de Stock
        public ActionResult EditStock(string id)
        {
            var Id = new ObjectId(id);
            var producto = productosCollection.AsQueryable<ProductosModel>().SingleOrDefault(x => x.IdProducto == Id);
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        public ActionResult EditStock(string id, ProductosModel producto)
        {
            try
            {
                var filter = Builders<ProductosModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ProductosModel>.Update
                    .Set("NombreProducto", producto.NombreProducto)
                    .Set("CantidadProducto", producto.CantidadProducto)
                    .Set("StockMinimo", producto.StockMinimo)
                    .Set("StockOptimo", producto.StockOptimo);

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
