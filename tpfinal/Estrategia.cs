
using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{

    public class Estrategia
    {
        private int CalcularDistancia(string str1, string str2)
        {
            // using the method
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
            string resutl = "Implementar";
            return resutl;
        }


        public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
        {
            string result = "Implementar";

            return result;
        }



        public String Consulta3(ArbolGeneral<DatoDistancia> arbol)
        {
            string result = "Implementar";

            return result;
        }

        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
        {
            // Si el árbol está vacío 
            if (arbol.getDatoRaiz().texto == ".")
            {
                // Reemplazar el nodo raíz vacío con el nuevo dato
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