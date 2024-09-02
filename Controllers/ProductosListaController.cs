using MongoDB.Bson;
using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
    public class ProductosListaController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<ProductosListaModel> productoCollection;
        private IMongoCollection<ProveedorModel> proveedorCollection;


        public ProductosListaController()
        {
            dbcontext = new MongoDBContext();
            productoCollection = dbcontext.database.GetCollection<ProductosListaModel>("Producto");
            proveedorCollection = dbcontext.database.GetCollection<ProveedorModel>("Proveedor");

        }
        // GET: Producto
        public ActionResult Index()
        {
            List<ProductosListaModel> producto = productoCollection.AsQueryable<ProductosListaModel>().ToList();
            return View(producto);
        }

        // GET: Producto/Details/5
        public ActionResult Details(string id)
        {
            var Id = new ObjectId(id);
            var producto = productoCollection.AsQueryable<ProductosListaModel>().SingleOrDefault(x => x.IdProductoLista == Id);
            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
            ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        public ActionResult Create(ProductosListaModel producto)
        {

            try
            {
                productoCollection.InsertOne(producto);
                var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
                ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");

                return RedirectToAction("Index");
            }
            catch
            {
                var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
                ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");
                return View();
            }

        }

        // GET: Producto/Edit/5
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var producto = productoCollection.AsQueryable<ProductosListaModel>().SingleOrDefault(x => x.IdProductoLista == Id);

            // Obtener la lista de proveedores
            var proveedores = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
            ViewBag.NombreProveedores = new SelectList(proveedores, "NombreProveedor", "NombreProveedor");

            return View(producto);
        }


        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ProductosListaModel producto)
        {
            try
            {
                var filter = Builders<ProductosListaModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ProductosListaModel>.Update
                    .Set("NombreProducto", producto.NombreProducto)
                    .Set("PrecioProducto", producto.PrecioProducto)
                    .Set("NombreProveedorFK", producto.NombreProveedorFK)
                    .Set("Estatus", producto.Estatus);


                var result = productoCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(string id)
        {
            var Id = new ObjectId(id);
            var proveedor = productoCollection.AsQueryable<ProductosListaModel>().SingleOrDefault(x => x.IdProductoLista == Id);
            return View(proveedor);
        }

        // POST: Producto/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, ProductosListaModel producto)
        {
            try
            {
                productoCollection.DeleteOne(Builders<ProductosListaModel>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
