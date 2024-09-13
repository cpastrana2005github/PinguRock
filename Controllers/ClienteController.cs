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
    public class ClienteController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<ClienteModel> clienteCollection;

        public ClienteController()
        {
            dbcontext = new MongoDBContext();
            clienteCollection = dbcontext.database.GetCollection<ClienteModel>("Cliente");
        }

        // GET: Cliente
        public ActionResult Index()
        {
            List<ClienteModel> cliente = clienteCollection.AsQueryable<ClienteModel>().ToList();
            return View(cliente);
        }



        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(ClienteModel cliente)
        {
            try
            {
                clienteCollection.InsertOne(cliente);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var cliente = clienteCollection.AsQueryable<ClienteModel>().SingleOrDefault(x => x._IdCliente == Id);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ClienteModel cliente)
        {
            try
            {
                var filter = Builders<ClienteModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ClienteModel>.Update
                    .Set("IdCliente", cliente.IdCliente)
                    .Set("NombreCliente", cliente.NombreCliente)
                    .Set("DirecciónCliente", cliente.DirecciónCliente)
                    .Set("CorreoCliente", cliente.CorreoCliente);

                var result = clienteCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
