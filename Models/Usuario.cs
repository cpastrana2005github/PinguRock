using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class Usuario
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("NombreUsuario")]
        public string NombreUsuario { get; set; }

        [BsonElement("Contrasena")]
        public string Contrasena { get; set; }
    }
}