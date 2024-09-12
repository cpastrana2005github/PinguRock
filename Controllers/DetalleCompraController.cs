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
    public class DetalleCompraController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<DetalleCompraModel> detalleCompraCollection;
        private IMongoCollection<ClienteModel> clienteCollection;
        private IMongoCollection<ProductosModel> productosCollection;



        public DetalleCompraController()
        {
            dbcontext = new MongoDBContext();
            detalleCompraCollection = dbcontext.database.GetCollection<DetalleCompraModel>("DetalleCompra");
            clienteCollection = dbcontext.database.GetCollection<ClienteModel>("Cliente");
            productosCollection = dbcontext.database.GetCollection<ProductosModel>("Productos");


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
                    // Calcular PrecioCompra
                    var filterCompra = Builders<CompraModel>.Filter.Eq("IdDetalleCompra", detalleCompra.IdDetalleCompra.ToString());
                    var compras = dbcontext.database.GetCollection<CompraModel>("Compra").Find(filterCompra).ToList();
                    detalleCompra.PrecioCompra = compras.Sum(c => c.PrecioAcumulado);

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
        public ActionResult EditDetalleCompra(string id)
        {
            var Id = new ObjectId(id);
            var detalleCompra = detalleCompraCollection.AsQueryable<DetalleCompraModel>().SingleOrDefault(x => x.IdDetalleCompra == Id);
            return View(detalleCompra);
        }

        [HttpPost]
        public ActionResult EditDetalleCompra(string id, DetalleCompraModel detalleCompra)
        {
            try
            {
                // Verifica si EstadoPago es "Pagado"
                if (detalleCompra.EstadoPago == "Pagado")
                {
                    // Obtener las compras asociadas al DetalleCompra
                    var filterCompra = Builders<CompraModel>.Filter.Eq("IdDetalleCompra", id);
                    var compras = dbcontext.database.GetCollection<CompraModel>("Compra").Find(filterCompra).ToList();

                    // Primero verificamos si hay suficiente stock para todos los productos
                    foreach (var compra in compras)
                    {
                        string nombreAux = compra.NombreProducto.Trim();
                        int cantidadAux = compra.CantidadProductoCompra;

                        // Buscar el producto en el inventario
                        var filterProducto = Builders<ProductosModel>.Filter.Eq("NombreProducto", nombreAux);
                        var producto = productosCollection.Find(filterProducto).FirstOrDefault();

                        if (producto == null)
                        {
                            // Producto no encontrado, mostrar un error
                            ModelState.AddModelError("", $"El producto {nombreAux} no se encuentra en el inventario.");
                            return View(detalleCompra);
                        }

                        // Verificar si hay suficiente cantidad en el inventario
                        if (producto.CantidadProducto < cantidadAux)
                        {
                            // No hay suficiente stock, mostrar un error y detener el proceso
                            ModelState.AddModelError("", $"No hay suficiente stock de {nombreAux}. Solo hay {producto.CantidadProducto} unidades disponibles.");
                            return View(detalleCompra);
                        }
                    }

                    // Si todos los productos tienen suficiente stock, procedemos a hacer la reducción
                    foreach (var compra in compras)
                    {
                        string nombreAux = compra.NombreProducto.Trim();
                        int cantidadAux = compra.CantidadProductoCompra;

                        // Buscar el producto en el inventario
                        var filterProducto = Builders<ProductosModel>.Filter.Eq("NombreProducto", nombreAux);
                        var producto = productosCollection.Find(filterProducto).FirstOrDefault();

                        // Restar la cantidad de productos comprados del inventario
                        producto.CantidadProducto -= cantidadAux;

                        // Actualizar el inventario
                        var updateProducto = Builders<ProductosModel>.Update.Set("CantidadProducto", producto.CantidadProducto);
                        var updateResult = productosCollection.UpdateOne(filterProducto, updateProducto);

                        if (updateResult.ModifiedCount == 0)
                        {
                            // No se realizó ninguna actualización, mostrar un error
                            ModelState.AddModelError("", $"No se pudo actualizar el stock del producto {nombreAux}.");
                            return View(detalleCompra);
                        }
                    }
                }

                // Calcular PrecioCompra
                var filterCompraUpdate = Builders<CompraModel>.Filter.Eq("IdDetalleCompra", id);
                var comprasUpdate = dbcontext.database.GetCollection<CompraModel>("Compra").Find(filterCompraUpdate).ToList();
                detalleCompra.PrecioCompra = comprasUpdate.Sum(c => c.PrecioAcumulado);

                // Filtro para encontrar el documento por id
                var filter = Builders<DetalleCompraModel>.Filter.Eq("_id", ObjectId.Parse(id));

                // Actualiza el documento en la base de datos
                var update = Builders<DetalleCompraModel>.Update
                    .Set("NombreCompra", detalleCompra.NombreCompra)
                    .Set("MedioPago", detalleCompra.MedioPago)
                    .Set("EstadoPago", detalleCompra.EstadoPago)
                    .Set("DetallesPago", detalleCompra.DetallesPago)
                    .Set("PrecioCompra", detalleCompra.PrecioCompra);  // Asegura que se actualice el campo PrecioCompra

                var updateResultDetalleCompra = detalleCompraCollection.UpdateOne(filter, update);

                if (updateResultDetalleCompra.ModifiedCount == 0)
                {
                    // No se realizó ninguna actualización, mostrar un error
                    ModelState.AddModelError("", "No se pudo actualizar los detalles de la compra.");
                    return View(detalleCompra);
                }

                return RedirectToAction("IndexDetalleCompra");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Se produjo un error: {ex.Message}");
                return View(detalleCompra);
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

        // POST: DetalleCompra/ActualizarPrecio/5
        [HttpPost]
        public ActionResult ActualizarPrecio(string id)
        {
            try
            {
                var detalleCompra = detalleCompraCollection.AsQueryable<DetalleCompraModel>().SingleOrDefault(x => x.IdDetalleCompra == new ObjectId(id));

                if (detalleCompra != null)
                {
                    var filterCompra = Builders<CompraModel>.Filter.Eq("IdDetalleCompra", id);
                    var compras = dbcontext.database.GetCollection<CompraModel>("Compra").Find(filterCompra).ToList();
                    detalleCompra.PrecioCompra = compras.Sum(c => c.PrecioAcumulado);

                    var filter = Builders<DetalleCompraModel>.Filter.Eq("_id", new ObjectId(id));
                    var update = Builders<DetalleCompraModel>.Update.Set("PrecioCompra", detalleCompra.PrecioCompra);
                    detalleCompraCollection.UpdateOne(filter, update);
                }

                return RedirectToAction("IndexDetalleCompra");
            }
            catch
            {
                return View("Error");
            }
        }

    }
}
