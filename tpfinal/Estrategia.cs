using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{
    public class Estrategia
    {
        private int CalcularDistancia(string str1, string str2)
        {
            String[] strlist1 = str1.ToLower().Split(' ');
            String[] strlist2 = str2.ToLower().Split(' ');
            int distance = 1000;
            foreach (String s1 in strlist1)
            {
                foreach (String s2 in strlist2)
                {
                    distance = Math.Min(distance, Utils.calculateLevenshteinDistance(s1, s2));
                }
            }
            return distance;
        }

        public String Consulta1(ArbolGeneral<DatoDistancia> arbol)
        {
            string resultado = "HOJAS DEL ARBOL\n\n";

            if (arbol.getDatoRaiz().texto == ".")
            {
                return "El arbol esta vacio";
            }

            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();
            cola.encolar(arbol);

            while (!cola.esVacia())
            {
                ArbolGeneral<DatoDistancia> nodo = cola.desencolar();

                if (nodo.esHoja())
                {
                    resultado += nodo.getDatoRaiz().texto + "\n";
                }

                foreach (var hijo in nodo.getHijos())
                {
                    cola.encolar(hijo);
                }
            }

            return resultado;
        }

        public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
        {
            string resultado = "CAMINOS HASTA CADA HOJA\n\n";

            if (arbol.getDatoRaiz().texto == ".")
            {
                return "El arbol esta vacio";
            }

            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();
            Cola<List<string>> colaCaminos = new Cola<List<string>>();

            cola.encolar(arbol);
            List<string> caminoInicial = new List<string>();
            caminoInicial.Add(arbol.getDatoRaiz().texto);
            colaCaminos.encolar(caminoInicial);

            while (!cola.esVacia())
            {
                ArbolGeneral<DatoDistancia> nodo = cola.desencolar();
                List<string> camino = colaCaminos.desencolar();

                if (nodo.esHoja())
                {
                    resultado += string.Join(" - ", camino) + "\n";
                }
                else
                {
                    foreach (var hijo in nodo.getHijos())
                    {
                        cola.encolar(hijo);
                        List<string> nuevoCamino = new List<string>(camino);
                        nuevoCamino.Add(hijo.getDatoRaiz().texto);
                        colaCaminos.encolar(nuevoCamino);
                    }
                }
            }

            return resultado;
        }

        public String Consulta3(ArbolGeneral<DatoDistancia> arbol)
        {
            string resultado = "ARBOL POR NIVELES\n\n";

            if (arbol.getDatoRaiz().texto == ".")
            {
                return "El arbol esta vacio";
            }

            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();
            Cola<int> colaNiveles = new Cola<int>();

            cola.encolar(arbol);
            colaNiveles.encolar(0);

            int nivelActual = -1;

            while (!cola.esVacia())
            {
                ArbolGeneral<DatoDistancia> nodoActual = cola.desencolar();
                int nivel = colaNiveles.desencolar();

                if (nivel != nivelActual)
                {
                    if (nivelActual >= 0)
                    {
                        resultado += "\n";
                    }
                    resultado += "Nivel " + nivel + "\n";
                    nivelActual = nivel;
                }

                resultado += "  " + nodoActual.getDatoRaiz().texto + "\n";

                foreach (var hijo in nodoActual.getHijos())
                {
                    cola.encolar(hijo);
                    colaNiveles.encolar(nivel + 1);
                }
            }

            return resultado;
        }

        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
        {
            if (arbol.getDatoRaiz().texto == ".")
            {
                arbol.getDatoRaiz().texto = dato.texto;
                arbol.getDatoRaiz().descripcion = dato.descripcion;
                arbol.getDatoRaiz().distancia = 0;
                return;
            }

            int distancia = CalcularDistancia(dato.texto, arbol.getDatoRaiz().texto);

            ArbolGeneral<DatoDistancia> hijoEncontrado = null;
            foreach (var hijo in arbol.getHijos())
            {
                if (hijo.getDatoRaiz().distancia == distancia)
                {
                    hijoEncontrado = hijo;
                    break;
                }
            }

            if (hijoEncontrado != null)
            {
                AgregarDato(hijoEncontrado, dato);
            }
            else
            {
                DatoDistancia nuevoNodo = new DatoDistancia(distancia, dato.texto, dato.descripcion);
                ArbolGeneral<DatoDistancia> nuevoSubarbol = new ArbolGeneral<DatoDistancia>(nuevoNodo);
                arbol.agregarHijo(nuevoSubarbol);
            }
        }

        public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
        {
            if (arbol.getDatoRaiz().texto == ".")
            {
                return;
            }

            int distanciaActual = CalcularDistancia(elementoABuscar, arbol.getDatoRaiz().texto);

            if (distanciaActual <= umbral)
            {
                DatoDistancia resultado = new DatoDistancia(distanciaActual,
                                                            arbol.getDatoRaiz().texto,
                                                            arbol.getDatoRaiz().descripcion);
                collected.Add(resultado);
            }

            foreach (var hijo in arbol.getHijos())
            {
                int distanciaHijo = hijo.getDatoRaiz().distancia;
                int diferencia = distanciaHijo - distanciaActual;

                if (diferencia < 0)
                {
                    diferencia = -diferencia;
                }

                if (diferencia <= umbral)
                {
                    Buscar(hijo, elementoABuscar, umbral, collected);
                }
            }
        }
    }
}
