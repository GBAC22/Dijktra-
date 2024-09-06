using System;
using System.Collections.Generic;

namespace DijkstraMVC.Models
{
    public class Nodo
    {
        public string Nombre { get; set; }
        public List<Arista> Aristas { get; set; }

        public Nodo(string nombre)
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre), "El nombre del nodo no puede ser null.");
            Aristas = new List<Arista>();
        }
    }

    public class Arista
    {
        public Nodo Destino { get; set; }
        public int Distancia { get; set; }

        public Arista(Nodo destino, int distancia)
        {
            Destino = destino ?? throw new ArgumentNullException(nameof(destino), "El nodo destino no puede ser null.");
            Distancia = distancia;
        }
    }

    public class Grafo
    {
        public List<Nodo> Nodos { get; set; }

        public Grafo()
        {
            Nodos = new List<Nodo>();
        }

        public void AgregarNodo(Nodo nodo)
        {
            if (nodo == null)
                throw new ArgumentNullException(nameof(nodo), "El nodo no puede ser null.");

            Nodos.Add(nodo);
        }

        public void AgregarArista(string origen, string destino, int distancia)
        {
            Nodo nodoOrigen = Nodos.Find(n => n.Nombre == origen);
            Nodo nodoDestino = Nodos.Find(n => n.Nombre == destino);

            if (nodoOrigen == null)
                throw new InvalidOperationException($"El nodo origen '{origen}' no se encontró.");

            if (nodoDestino == null)
                throw new InvalidOperationException($"El nodo destino '{destino}' no se encontró.");

            nodoOrigen.Aristas.Add(new Arista(nodoDestino, distancia));
            nodoDestino.Aristas.Add(new Arista(nodoOrigen, distancia)); // Si es bidireccional
        }

        public Dictionary<Nodo, int> Dijkstra(Nodo inicio)
        {
            if (inicio == null)
                throw new ArgumentNullException(nameof(inicio), "El nodo de inicio no puede ser null.");

            var distancias = new Dictionary<Nodo, int>();
            var visitados = new HashSet<Nodo>();
            var cola = new PriorityQueue<Nodo, int>();

            foreach (var nodo in Nodos)
            {
                distancias[nodo] = int.MaxValue;
            }
            distancias[inicio] = 0;
            cola.Enqueue(inicio, 0);

            while (cola.Count > 0)
            {
                Nodo actual = cola.Dequeue();
                visitados.Add(actual);

                foreach (var arista in actual.Aristas)
                {
                    if (!visitados.Contains(arista.Destino))
                    {
                        int nuevaDistancia = distancias[actual] + arista.Distancia;
                        if (nuevaDistancia < distancias[arista.Destino])
                        {
                            distancias[arista.Destino] = nuevaDistancia;
                            cola.Enqueue(arista.Destino, nuevaDistancia);
                        }
                    }
                }
            }
            return distancias;
        }

        public List<List<Nodo>> EncontrarCaminos(string inicio, string destino)
        {
            Nodo nodoInicio = Nodos.Find(n => n.Nombre == inicio);
            Nodo nodoDestino = Nodos.Find(n => n.Nombre == destino);

            var caminos = new List<List<Nodo>>();
            if (nodoInicio == null || nodoDestino == null) return caminos;

            EncontrarCaminosRecursivo(nodoInicio, nodoDestino, new List<Nodo>(), caminos);
            return caminos;
        }

        private void EncontrarCaminosRecursivo(Nodo actual, Nodo destino, List<Nodo> caminoActual, List<List<Nodo>> caminos)
        {
            caminoActual.Add(actual);

            if (actual == destino)
            {
                caminos.Add(new List<Nodo>(caminoActual));
            }
            else
            {
                foreach (var arista in actual.Aristas)
                {
                    if (!caminoActual.Contains(arista.Destino))
                    {
                        EncontrarCaminosRecursivo(arista.Destino, destino, caminoActual, caminos);
                    }
                }
            }

            caminoActual.Remove(actual);
        }
    }
}
