using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DijkstraMVC.Models;

namespace DijkstraMVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DijkstraController : Controller
    {
        [HttpPost("EjecutarDijkstra")]
        public IActionResult EjecutarDijkstra([FromBody] GrafoData grafoData)
        {
            Grafo grafo = new Grafo();
            Dictionary<string, Nodo> nodosDic = new Dictionary<string, Nodo>();

            // Crear nodos
            foreach (var nodo in grafoData.Nodos)
            {
                Nodo nuevoNodo = new Nodo(nodo.Nombre);
                grafo.AgregarNodo(nuevoNodo);
                nodosDic[nodo.Nombre] = nuevoNodo;
            }

            // Crear aristas
            foreach (var arista in grafoData.Aristas)
            {
                Nodo nodoOrigen = nodosDic[arista.Origen];
                Nodo nodoDestino = nodosDic[arista.Destino];
                grafo.AgregarArista(nodoOrigen.Nombre, nodoDestino.Nombre, arista.Distancia);
            }

            // Ejecutar Dijkstra
            Nodo nodoInicio = nodosDic[grafoData.NodoInicio];
            var distancias = grafo.Dijkstra(nodoInicio);

            // Convertir a formato que se puede enviar como JSON
            var resultado = distancias.ToDictionary(d => d.Key.Nombre, d => d.Value);
            return Ok(resultado);
        }

        [HttpPost("EncontrarCaminos")]
        public IActionResult EncontrarCaminos([FromBody] CaminosData caminosData)
        {
            Grafo grafo = new Grafo();
            Dictionary<string, Nodo> nodosDic = new Dictionary<string, Nodo>();

            // Crear nodos
            foreach (var nodo in caminosData.Nodos)
            {
                Nodo nuevoNodo = new Nodo(nodo.Nombre);
                grafo.AgregarNodo(nuevoNodo);
                nodosDic[nodo.Nombre] = nuevoNodo;
            }

            // Crear aristas
            foreach (var arista in caminosData.Aristas)
            {
                Nodo nodoOrigen = nodosDic[arista.Origen];
                Nodo nodoDestino = nodosDic[arista.Destino];
                grafo.AgregarArista(nodoOrigen.Nombre, nodoDestino.Nombre, arista.Distancia);
            }

            // Encontrar caminos
            var caminos = grafo.EncontrarCaminos(caminosData.NodoInicio, caminosData.NodoDestino);

            // Convertir a formato que se puede enviar como JSON
            var resultado = caminos.Select(c => c.Select(n => n.Nombre).ToList()).ToList();
            return Ok(resultado);
        }
    }

    // Clases para recibir los datos del grafo
    public class NodoData
    {
        public string Nombre { get; set; }
    }

    public class AristaData
    {
        public string Origen { get; set; }
        public string Destino { get; set; }
        public int Distancia { get; set; }
    }

    public class GrafoData
    {
        public List<NodoData> Nodos { get; set; }
        public List<AristaData> Aristas { get; set; }
        public string NodoInicio { get; set; }
    }

    public class CaminosData
    {
        public List<NodoData> Nodos { get; set; }
        public List<AristaData> Aristas { get; set; }
        public string NodoInicio { get; set; }
        public string NodoDestino { get; set; }
    }
}
