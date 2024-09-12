using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class CompraModel
    {
        [BsonId]
        public ObjectId _IdCompra { get; set; }

        [BsonElement("NombreProducto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string NombreProducto { get; set; }

        [BsonElement("CantidadProductoCompra")]
        [Required(ErrorMessage = "Ingresar la cantidad a comprar es obligatorio.")]
        public int CantidadProductoCompra { get; set; }

        [BsonElement("PrecioUnidad")]
        public int PrecioUnidad { get; set; }

        [BsonElement("PrecioAcumulado")]
        public int PrecioAcumulado { get; set; }

        [BsonElement("IdDetalleCompra")]
        public string IdDetalleCompra { get; set; }    
    }
}