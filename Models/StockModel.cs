using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;

namespace PinguRock.Models
{
    public class StockModel
    {
        //Establece el stock mínimo, bajo, moderado, suficiente y óptimo de un producto

        [BsonId]
        public ObjectId IdStock { get; set; }

        [BsonElement("NombreProductoStock")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string NombreProducto { get; set; }

        //Este es el umbral crítico donde se debe reponer el stock urgentemente
        [BsonElement("StockMinimo")]
        [Required(ErrorMessage = "La cantidad de Stock mínimo es obligatorio.")]
        public int StockMinimo { get; set; }

        //El stock está por debajo de lo ideal, pero no crítico,
        //es un aviso de que pronto podría llegar a ser necesario reponer
        [BsonElement("StockBajo")]
        public int? StockBajo { get; set; }

        //Está en un nivel intermedio, suficiente para operar, pero requiere monitoreo,
        //podría considerarse como un estado de precaución
        [BsonElement("StockModerado")]
        public int StockModerado { get; set; }

        //El stock está en un rango saludable, aunque no en su punto óptimo.
        //No es necesario reponer
        [BsonElement("StockSuficiente")]
        public int? StockSuficiente { get; set; }


        //El stock está en su nivel ideal y no requiere acción inmediata
        [BsonElement("StockOptimo")]
        [Required(ErrorMessage = "La cantidad de Stock óptimo es obligatorio.")]
        public int StockOptimo { get; set; }

        [BsonElement("StockActual")]
        public int StockActual { get; set; }

        public string CalcularEstadoStock()
        {
            if (StockActual <= StockMinimo)
                return "StockMinimo";
            else if (StockActual <= StockBajo)
                return "StockBajo";
            else if (StockActual <= StockModerado)
                return "StockModerado";
            else if (StockActual <= StockSuficiente)
                return "StockSuficiente";
            else
                return "StockOptimo";
        }
    }
}