using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;

namespace PinguRock.Controllers
{
    public class StockController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<StockModel> stockCollection;

        public StockController()
        {
            dbcontext = new MongoDBContext();
            stockCollection = dbcontext.database.GetCollection<StockModel>("Stock");
        }

        // GET: Stock
        public ActionResult Index()
        {
            List<StockModel> stocks = stockCollection.AsQueryable<StockModel>().ToList();
            return View(stocks);
        }

       

        // GET: Stock/Details/5
        public ActionResult Details(string id)
        {
            var Id = new ObjectId(id);
            var stock = stockCollection.AsQueryable<StockModel>().SingleOrDefault(x => x.IdStock == Id);
            return View(stock);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(StockModel stock)
        {
            try
            {
                // Insertar el nuevo stock en la base de datos
                stockCollection.InsertOne(stock);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(string id)
        {
            var Id = new ObjectId(id);
            var stock = stockCollection.AsQueryable<StockModel>().SingleOrDefault(x => x.IdStock == Id);
            return View(stock);
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, StockModel stock)
        {
            try
            {
                var filter = Builders<StockModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<StockModel>.Update
                    .Set("NombreProducto", stock.NombreProducto)
                    .Set("StockMinimo", stock.StockMinimo)
                    .Set("StockBajo", stock.StockBajo)
                    .Set("StockModerado", stock.StockModerado)
                    .Set("StockSuficiente", stock.StockSuficiente)
                    .Set("StockOptimo", stock.StockOptimo)
                    .Set("StockActual", stock.StockActual);

                var result = stockCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(string id)
        {
            var Id = new ObjectId(id);
            var stock = stockCollection.AsQueryable<StockModel>().SingleOrDefault(x => x.IdStock == Id);
            return View(stock);
        }

        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, StockModel stock)
        {
            try
            {
                stockCollection.DeleteOne(Builders<StockModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
