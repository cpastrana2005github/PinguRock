using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class ClienteModel
    {
        [BsonId]
        public ObjectId _IdCliente { get; set; }

        [BsonElement("IdCliente")]
        [Required(ErrorMessage = "Ingresar el número identificador del cliente es obligatorio.")]
        public string IdCliente { get; set; }

        [BsonElement("NombreCliente")]
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string NombreCliente { get; set; }

        [BsonElement("Correo")]
        [Required(ErrorMessage = "El Correo es obligatorio.")]
        public string CorreoCliente { get; set; }

        [BsonElement("Dirección")]
        [Required(ErrorMessage = "La dirección del cliente es obligatorio.")]
        public string DirecciónCliente { get; set; }

    }
}