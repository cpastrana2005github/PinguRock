//using MongoDB.Bson;
//using MongoDB.Driver;
//using PinguRock.App_Start;
//using PinguRock.Models;

//public class InventarioService
//{
//    private readonly IMongoCollection<ProductosModel> productosCollection;

//    public InventarioService(MongoDBContext dbcontext)
//    {
//        productosCollection = dbcontext.database.GetCollection<ProductosModel>("Productos");
//    }

//    public bool ActualizarInventario(string nombreProducto, int cantidadRestar)
//    {
//        // Buscar el producto en la base de datos usando Find y Filter
//        var filter = Builders<ProductosModel>.Filter.Eq(p => p.NombreProducto, nombreProducto);
//        var producto = productosCollection.Find(filter).FirstOrDefault();

//            if (producto.CantidadProducto >= cantidadRestar)
//            {
//                // Actualizar la cantidad del producto en el inventario
//                var updateFilter = Builders<ProductosModel>.Filter.Eq(p => p.IdProducto, producto.IdProducto);
//                var update = Builders<ProductosModel>.Update.Inc(p => p.CantidadProducto, cantidadRestar);
//                productosCollection.UpdateOne(updateFilter, update);
//                return true;
//            }
//            else
//            {
//                // Si no hay suficiente inventario, devolver false
//                return false;
//            }

//    }
//}
