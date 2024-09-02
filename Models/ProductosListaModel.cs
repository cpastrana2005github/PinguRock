using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class ProductosListaModel
    {
        [BsonId]
        public ObjectId IdProductoLista { get; set; }

        [BsonElement("NombreProducto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string NombreProducto { get; set; }

        [BsonElement("PrecioProducto")]
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        public string PrecioProducto { get; set; }

        [BsonElement("NombreProveedorFK")]
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public string NombreProveedorFK { get; set; }

        [BsonElement("Estatus")]
        [Required(ErrorMessage = "El estatus del producto es obligatorio.")]
        public string Estatus { get; set; }
    }
}