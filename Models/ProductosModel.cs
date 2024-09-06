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

        [BsonElement("PrecioProducto")]
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        public string PrecioProducto { get; set; }

        [BsonElement("NombreProveedorFK")]
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        public string NombreProveedorFK { get; set; }

        [BsonElement("Estatus")]
        [Required(ErrorMessage = "El estatus del producto es obligatorio.")]
        public string Estatus { get; set; }

        [BsonElement("CantidadProducto")]
        [Required(ErrorMessage = "La cantidad de producto en inventario es obligatorio.")]
        public int CantidadProducto { get; set; }

        [BsonElement("EstadoStock")]
        public string EstadoStock { get; set; }

        //Este es el umbral crítico donde se debe reponer el stock urgentemente
        [BsonElement("StockMinimo")]
        [Required(ErrorMessage = "La cantidad de Stock mínimo es obligatorio.")]
        public int StockMinimo { get; set; }

        //Está en un nivel intermedio, suficiente para operar, pero requiere monitoreo,
        //podría considerarse como un estado de precaución


        //El stock está en su nivel ideal y no requiere acción inmediata
        [BsonElement("StockOptimo")]
        [Required(ErrorMessage = "La cantidad de Stock óptimo es obligatorio.")]
        public int StockOptimo { get; set; }

        public void CalcularEstadoStock(int stockMinimo, int stockModerado, int stockOptimo)
        {
            if (CantidadProducto <= stockMinimo)
            {
                EstadoStock = "StockMinimo";
            }
            else if (CantidadProducto >= stockOptimo)
            {
                EstadoStock = "StockOptimo";
            }
            else
            {
                EstadoStock = "StockModerado";
            }

        }
    }
}