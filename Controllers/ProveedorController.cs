using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using PinguRock.App_Start;
using MongoDB.Driver;
using PinguRock.Models;

namespace PinguRock.Controllers
{

    public class ProveedorController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<ProveedorModel> proveedorCollection;

        public ProveedorController()
        {
            dbcontext = new MongoDBContext();
            proveedorCollection = dbcontext.database.GetCollection<ProveedorModel>("Proveedor");
        }
        // GET: Proveedor
        public ActionResult Index()
        {
            List<ProveedorModel> proveedor = proveedorCollection.AsQueryable<ProveedorModel>().ToList();
            return View(proveedor);
        }

        // GET: Proveedor/Details/5
        public ActionResult Details(string id)
        {
            var Id = new ObjectId(id);
            var proveedor = proveedorCollection.AsQueryable<ProveedorModel>().SingleOrDefault(x => x.IdProveedor == Id);
            return View(proveedor);
        }

        // GET: Proveedor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedor/Create
        [HttpPost]
        public ActionResult Create(ProveedorModel proveedor)
        {
            try
            {
                proveedorCollection.InsertOne(proveedor);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proveedor/Edit/5
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var proveedor = proveedorCollection.AsQueryable<ProveedorModel>().SingleOrDefault(x => x.IdProveedor == Id);
            return View(proveedor);
        }

        // POST: Proveedor/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ProveedorModel proveedor)
        {
            try
            {
                var filter = Builders<ProveedorModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ProveedorModel>.Update
                    .Set("NitProveedor", proveedor.NitProveedor)
                    .Set("NombreProveedor", proveedor.NombreProveedor);

                var result = proveedorCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proveedor/Delete/5
        public ActionResult Delete(string id)
        {
            var Id = new ObjectId(id);
            var proveedor = proveedorCollection.AsQueryable<ProveedorModel>().SingleOrDefault(x => x.IdProveedor == Id);
            return View(proveedor);
        }

        // POST: Proveedor/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, ProveedorModel proveedor)
        {
            try
            {
                proveedorCollection.DeleteOne(Builders<ProveedorModel>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
