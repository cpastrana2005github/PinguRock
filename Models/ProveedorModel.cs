using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class ProveedorModel
    {
        [BsonId]
        public ObjectId IdProveedor { get; set; }

        [BsonElement("NitProveedor")]
        [Required(ErrorMessage = "El número de NIT del proveedor es obligatorio.")]
        public string NitProveedor { get; set; }

        [BsonElement("NombreProveedor")]
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        public string NombreProveedor { get; set; }
        

    }
}