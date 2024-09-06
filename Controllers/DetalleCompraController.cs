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
    public class DetalleCompraController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<DetalleCompraModel> detalleCompraCollection;
        private IMongoCollection<ClienteModel> clienteCollection;


        public DetalleCompraController()
        {
            dbcontext = new MongoDBContext();
            detalleCompraCollection = dbcontext.database.GetCollection<DetalleCompraModel>("DetalleCompra");
            clienteCollection = dbcontext.database.GetCollection<ClienteModel>("Cliente");

        }
        // GET: DetalleCompra
        public ActionResult IndexDetalleCompra()
        {
            List<DetalleCompraModel> detalleCompra = detalleCompraCollection.AsQueryable<DetalleCompraModel>().ToList();
            return View(detalleCompra);
        }


        // GET: DetalleCompra/Create
        public ActionResult Create()
        {
            var clientes = clienteCollection.AsQueryable<ClienteModel>().ToList();
            ViewBag.NombreClientes = new SelectList(clientes, "NombreCliente", "NombreCliente");

            return View();
        }


        [HttpPost]
        public ActionResult Create(DetalleCompraModel detalleCompra)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    detalleCompraCollection.InsertOne(detalleCompra);
                    return RedirectToAction("IndexDetalleCompra");
                }
                else
                {
                    // Recargar la lista de clientes si hay un error en el model state
                    var clientes = clienteCollection.AsQueryable<ClienteModel>().ToList();
                    ViewBag.NombreClientes = new SelectList(clientes, "NombreCliente", "NombreCliente");
                    return View(detalleCompra);
                }
            }
            catch
            {
                // También recarga la lista si hay una excepción
                var clientes = clienteCollection.AsQueryable<ClienteModel>().ToList();
                ViewBag.NombreClientes = new SelectList(clientes, "NombreCliente", "NombreCliente");
                return View(detalleCompra);
            }
        }


        // GET: DetalleCompra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DetalleCompra/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DetalleCompra/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DetalleCompra/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
