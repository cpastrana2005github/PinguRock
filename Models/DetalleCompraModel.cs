using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class DetalleCompraModel
    {
        [BsonId]
        public ObjectId IdCompra { get; set; }

        [BsonElement("NombreCompra")]
        [Required(ErrorMessage = "Ingresar el nombre de la compra es obligatorio.")]
        public string NombreCompra { get; set; }

        [BsonElement("NombreCliente")]
        [Required(ErrorMessage = "Ingresar el nombre del cliente es obligatorio.")]
        public string NombreClienteFK { get; set; }

        [BsonElement("PrecioCompra")]
        public int PrecioCompra { get; set; }

        [BsonElement("MedioPago")]
        public string MedioPago { get; set; }

        [BsonElement("EstadoPago")]
        public string EstadoPago { get; set; }

        [BsonElement("FechaCompra")]
        public string FechaCompra { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

    }
}