using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinguRock.Models
{
    public class ProductosModel
    {

        [BsonId]
        public ObjectId IdProducto { get; set; }

        [BsonElement("NombreProducto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string NombreProducto { get; set; }

        [BsonElement("CantidadProducto")]
        [Required(ErrorMessage = "La cantidad de producto en inventario es obligatorio.")]
        public int CantidadProducto { get; set; }

        [BsonElement("EstadoStock")]
        public string EstadoStock { get; set; }

        public void CalcularEstadoStock(int stockMinimo, int stockBajo, int stockModerado, int stockSuficiente, int stockOptimo)
        {
            if (CantidadProducto <= stockMinimo)
            {
                EstadoStock = "StockMinimo";
            }
            else if (CantidadProducto <= stockBajo)
            {
                EstadoStock = "StockBajo";
            }
            else if (CantidadProducto <= stockModerado)
            {
                EstadoStock = "StockModerado";
            }
            else if (CantidadProducto <= stockSuficiente)
            {
                EstadoStock = "StockSuficiente";
            }
            else
            {
                EstadoStock = "StockOptimo";
            }

        }
    }
}